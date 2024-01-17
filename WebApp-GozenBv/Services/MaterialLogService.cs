using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Helpers;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.ViewModels;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Helpers.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Mappers.Interfaces;

namespace WebApp_GozenBv.Services
{
    public class MaterialLogService : IMaterialLogService
    {
        private readonly IMaterialLogManager _logManager;
        private readonly IMaterialLogMapper _logMapper;
        private readonly IMaterialManager _materialManager;
        private readonly IMaterialHelper _materialHelper;
        private readonly IRepairTicketManager _repairManager;

        public MaterialLogService(
            IMaterialLogManager logManager,
            IMaterialManager materialManager,
            IMaterialHelper materialHelper,
            IRepairTicketManager repairManager,
            IMaterialLogMapper logMapper)
        {
            _logManager = logManager;
            _materialManager = materialManager;
            _materialHelper = materialHelper;
            _repairManager = repairManager;
            _logMapper = logMapper;
        }
        public async Task<string> HandleCreate(MaterialLogCreateViewModel incomingViewModel)
        {
            var selectedItems = JsonSerializer.Deserialize<List<MaterialLogSelectedItemViewModel>>(incomingViewModel.SelectedProducts,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (!selectedItems.Any())
            {
                throw new ArgumentNullException("No items in created log.");
            }

            string logId = Guid.NewGuid().ToString();
            try
            {
                //new items
                var mappedItems = _logMapper.MapSelectedItems(selectedItems, logId);
                var validatedItems = await ValidateSelectedItems(mappedItems);

                await _logManager.ManageMaterialLogItemsAsync(validatedItems, EntityOperation.Create);

                //new log
                var newLog = new MaterialLog()
                {
                    LogId = logId,
                    Status = MaterialLogStatus.Created,
                    LogDate = incomingViewModel.MaterialLogDate,
                    EmployeeId = incomingViewModel.EmployeeId
                };
                await _logManager.ManageMaterialLogAsync(newLog, EntityOperation.Create);
            }
            catch (Exception e)
            {
                //TODO: return exception to view (user)
                
            }

            return logId;
        }

        public async Task HandleEdit(MaterialLogEditViewModel incomingEdit)
        {
            var logId = incomingEdit.LogId;

            //get original model
            var originalLogDTO = await _logManager.GetMaterialLogDTO(logId);
            var originalLog = originalLogDTO.MaterialLog;
            var originalItems = originalLogDTO.MaterialLogItems;

            string statusName;

            switch (originalLog.Status)
            {
                case MaterialLogStatus.Created:
                    statusName = MaterialLogStatus.CreatedName;

                    var mappedItemsCreated = _logMapper.MapUpdatedItems_Created(logId, incomingEdit.CreatedEditViewModel.ItemsCreatedEditViewModel);
                    var incomingItems = await ValidateSelectedItems(mappedItemsCreated);

                    if (originalLog.Approved)
                    {
                        //TODO: catch higher
                        throw new Exception($"Is already approved at '{statusName}' state and so is readonly state. No edit possible.");
                    }

                    if (!IsModified(originalLogDTO, incomingEdit))
                    {
                        //TODO: catch higher
                        throw new Exception($"Nothing no modify. Are you sure you have made any changes?");
                    }

                    //map original to history
                    var mappedLogHistory = await _logMapper.MapLogHistoryAsync(originalLog);
                    //create record for history
                    await _logManager.ManageMaterialLogHistoryAsync(mappedLogHistory);

                    //map original items to history
                    var mappedItemHistory = await _logMapper.MapLogItemsHistoryAsync(originalItems);
                    //create records for history
                    await _logManager.ManageMaterialLogItemsHistoryAsync(mappedItemHistory);

                    //map the log
                    var updatedLog = _logMapper.MapUpdatedMaterialLog(originalLog, incomingEdit);
                    //update the log
                    await _logManager.ManageMaterialLogAsync(updatedLog, EntityOperation.Update);

                    //replace items
                    await _logManager.ManageMaterialLogItemsAsync(originalItems, EntityOperation.Delete);
                    await _logManager.ManageMaterialLogItemsAsync(incomingItems, EntityOperation.Create);
                    break;

                case MaterialLogStatus.Returned:
                    statusName = MaterialLogStatus.ReturnedName;

                    if (originalLog.Approved)
                    {
                        //TODO: catch higher
                        throw new Exception($"Is already approved at '{statusName}' state and so is readonly state. No edit possible.");
                    }

                    if (!IsModified(originalLogDTO, incomingEdit))
                    {
                        throw new Exception($"Nothing no modify. Are you sure you have made any changes?");
                    }

                    //map original items to history
                    var mappedHistory = await _logMapper.MapLogItemsHistoryAsync(originalItems);
                    //create records for history
                    await _logManager.ManageMaterialLogItemsHistoryAsync(mappedHistory);

                    //map the update
                    var mappedItemsReturned = _logMapper.MapUpdatedItems_Returned(originalItems, incomingEdit.ReturnedEditViewModel.ItemsReturnedEditViewModel);
                    //update items
                    await _logManager.ManageMaterialLogItemsAsync(mappedItemsReturned, EntityOperation.Update);
                    break;

                default:
                    throw new Exception("No edit possible at other than Created or Returned state.");
            }
        }
        public async Task HandleReturn(MaterialLogAndItemsViewModel incomingReturn)
        {
            //incoming
            string logId = incomingReturn.MaterialLog.LogId;
            var damaged = incomingReturn.MaterialLog.Damaged;

            //original
            var log = await _logManager.GetMaterialLogAsync(logId);
            var originalLogItems = await _logManager.GetMaterialLogItemsAsync(logId);

            if (log == null)
            {
                throw new NullReferenceException($"log with Id {logId} does not exist");
            }

            if (!log.Approved)
            {
                throw new Exception($"Log with id {logId} is not approved yet. It needs to be approved before returning it.");
            }

            log.ReturnDate = DateTime.Now;
            log.Status = MaterialLogStatus.Returned;
            //reset approved
            log.Approved = false;

            List<MaterialLogItem> modifiedItems = new();

            if (damaged)
            {
                var damagedMaterials = incomingReturn.MaterialLogItems.Where(x => x.IsDamaged).ToList();

                log.Damaged = damaged;

                foreach (var item in originalLogItems)
                {
                    var damagedItem = damagedMaterials.FirstOrDefault(di => di.MaterialId == item.MaterialId);

                    if (damagedItem != null)
                    {
                        item.DamagedAmount = damagedItem.DamagedAmount;
                        item.RepairAmount = damagedItem.RepairAmount;
                        item.DeleteAmount = damagedItem.DeleteAmount;
                        item.IsDamaged = true;
                        modifiedItems.Add(item);
                    }
                }
                await _logManager.ManageMaterialLogItemsAsync(modifiedItems, EntityOperation.Update);
            }

            await _logManager.ManageMaterialLogAsync(log, EntityOperation.Update);
        }

        public async Task HandleApprove(string logId)
        {
            var materialLogDTO = await _logManager.GetMaterialLogDTO(logId);
            var status = materialLogDTO.MaterialLog.Status;
            var alreadyApproved = materialLogDTO.MaterialLog.Approved;

            if (alreadyApproved)
            {
                throw new Exception($"Log with id {logId} is already approved.");
            }

            switch (status)
            {
                case MaterialLogStatus.Created:
                    await ApproveCreate(materialLogDTO);
                    break;
                case MaterialLogStatus.Returned:
                    await ApproveReturn(materialLogDTO);
                    break;
                default:
                    throw new Exception("Fatal error. Status is not Created or Returned.");
            }
        }

        public async Task ApproveCreate(MaterialLogDTO materialLogDTO)
        {
            //set to approved
            materialLogDTO.MaterialLog.Approved = true;

            //modify materials
            var modifiedMaterials = new List<Material>();
            foreach (var item in materialLogDTO.MaterialLogItems)
            {
                var material = await _materialManager.GetMaterialAsync(item.MaterialId);
                modifiedMaterials.Add(_materialHelper.TakeQuantity(material, item.MaterialAmount, item.Used));
            }
            //update
            _materialManager.ManageMaterials(modifiedMaterials, EntityOperation.Update);
            _logManager.ManageMaterialLog(materialLogDTO.MaterialLog, EntityOperation.Update);
        }

        public async Task ApproveReturn(MaterialLogDTO materialLogDTO)
        {
            //set to approved
            materialLogDTO.MaterialLog.Approved = true;

            //modify materials
            var modifiedMaterials = new List<Material>();
            foreach (var item in materialLogDTO.MaterialLogItems)
            {
                var material = await _materialManager.GetMaterialAsync(item.MaterialId);

                if (item.IsDamaged)
                {
                    //if repair any -> repairticket/item
                    if (item.RepairAmount > 0)
                    {
                        material = _materialHelper.ToRepairQuantity(material, (int)item.RepairAmount);

                        var tickets = new List<RepairTicket>();
                        for (int i = 0; i < item.RepairAmount; i++)
                        {
                            tickets.Add(new RepairTicket()
                            {
                                LogId = item.LogId,
                                Status = RepairTicketStatus.AwaitingAction,
                                MaterialId = item.MaterialId,
                                Material = material
                            });
                        }
                        await _repairManager.ManageTicketsAsync(tickets, EntityOperation.Create);

                    }

                    //if delete any -> deleteamount
                    if (item.DeleteAmount > 0)
                    {
                        material = _materialHelper.DeleteQuantity(material, (int)item.DeleteAmount);
                    }

                    //undamaged items in MaterialAmount
                    if (item.MaterialAmount > item.DamagedAmount)
                    {
                        var undamagedAmount = item.MaterialAmount - (int)item.DamagedAmount;
                        material = _materialHelper.ReturnQuantity(material, undamagedAmount);
                    }
                }
                else
                {
                    material = _materialHelper.ReturnQuantity(material, item.MaterialAmount);
                }
                modifiedMaterials.Add(material);
            }
            //update
            _materialManager.ManageMaterials(modifiedMaterials, EntityOperation.Update);
            _logManager.ManageMaterialLog(materialLogDTO.MaterialLog, EntityOperation.Update);
        }

        public async Task HandleDelete(string logId)
        {
            var logDTO = await _logManager.GetMaterialLogDTO(logId);
            var status = logDTO.MaterialLog.Status;
            var approved = logDTO.MaterialLog.Approved;

            //only deleting when created is not approved.
            if (status != MaterialLogStatus.Created || approved)
            {
                //TODO: catch higher
                throw new Exception("Cannot delete log after approved creation.");
            }

            //remove log and its materiallogitems
            await _logManager.ManageMaterialLogAsync(logDTO.MaterialLog, EntityOperation.Delete);
            await _logManager.ManageMaterialLogItemsAsync(logDTO.MaterialLogItems, EntityOperation.Delete);
        }

        private bool IsModified(MaterialLogDTO original, MaterialLogEditViewModel incoming)
        {
            var originalLog = original.MaterialLog;
            var originalItems = original.MaterialLogItems;

            var createdVm = incoming.CreatedEditViewModel;
            var returnedVm = incoming.ReturnedEditViewModel;

            var status = originalLog.Status;
            bool isModified = false;

            switch (status)
            {
                case MaterialLogStatus.Created:

                    isModified =
                        originalLog.EmployeeId != createdVm.EmployeeId
                        || originalLog.LogDate.Date != createdVm.LogDate.Date;
                    if (isModified)
                    {
                        return isModified;
                    }

                    isModified = originalItems.Count != createdVm.ItemsCreatedEditViewModel.Count;
                    if (isModified)
                    {
                        return isModified;
                    }

                    //TODO: are the sequences in the same order? set to same order in validation?
                    for (int i = 0; i < originalItems.Count; i++)
                    {
                        isModified =
                            originalItems[i].MaterialId != createdVm.ItemsCreatedEditViewModel[i]?.MaterialId
                            || originalItems[i].MaterialAmount != createdVm.ItemsCreatedEditViewModel[i]?.MaterialAmount
                            || originalItems[i].Used != createdVm.ItemsCreatedEditViewModel[i]?.Used;
                        if (isModified)
                        {
                            return isModified;
                        }
                    }
                    break;

                case MaterialLogStatus.Returned:

                    isModified = originalLog.ReturnDate.Date != returnedVm.ReturnDate.Date;
                    if (isModified)
                    {
                        return isModified;
                    }

                    //no count check needed bc cannot delete item in return status

                    //TODO: are the sequences in the same order? set to same order in validation?
                    for (int i = 0; i < originalItems.Count; i++)
                    {
                        isModified = originalItems[i].MaterialId != returnedVm.ItemsReturnedEditViewModel[i]?.MaterialId
                            || originalItems[i].MaterialAmount != returnedVm.ItemsReturnedEditViewModel[i]?.MaterialAmount
                            || originalItems[i].Used != returnedVm.ItemsReturnedEditViewModel[i]?.Used
                            || originalItems[i].IsDamaged != returnedVm.ItemsReturnedEditViewModel[i]?.IsDamaged
                            || originalItems[i].DamagedAmount != returnedVm.ItemsReturnedEditViewModel[i]?.DamagedAmount
                            || originalItems[i].RepairAmount != returnedVm.ItemsReturnedEditViewModel[i]?.RepairAmount
                            || originalItems[i].DeleteAmount != returnedVm.ItemsReturnedEditViewModel[i]?.DeleteAmount;

                        if (isModified)
                        {
                            return isModified;
                        }
                    }
                    break;
            }
            return isModified;
        }

        private async Task<List<MaterialLogItem>> ValidateSelectedItems(List<MaterialLogItem> selectedItems)
        {
            //merge same items
            var mergedItems = selectedItems
                .GroupBy(x => new { x.LogId, x.MaterialId, x.Used })
                .Select(group => new MaterialLogItem
                {
                    LogId = group.Key.LogId,
                    MaterialId = group.Key.MaterialId,
                    MaterialAmount = group.Sum(x => x.MaterialAmount),
                    Used = group.Key.Used
                }).ToList();

            //validate qty items
            foreach (var item in mergedItems)
            {
                var material = await _materialManager.GetMaterialAsync(item.MaterialId);
                _materialHelper.ValidateQuantity(material, item.MaterialAmount, item.Used);
            }

            return mergedItems;
        }

        public async Task ValidateAllLogsApproved()
        {
            var logs = await _logManager.GetMaterialLogsAsync();
            if (logs.Any(x => !x.Approved))
            {
                var exceptionMessage = "Not all logs are approved. Approve before create or edit logs";
                throw new Exception(exceptionMessage);
            }
        }
    }
}
