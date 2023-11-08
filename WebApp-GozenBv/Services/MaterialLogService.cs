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

namespace WebApp_GozenBv.Services
{
    public class MaterialLogService : IMaterialLogService
    {
        private readonly IMaterialLogManager _logManager;
        private readonly IMaterialManager _materialManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IMaterialHelper _materialHelper;
        private readonly IEqualityHelper _equalityHelper;
        private readonly IRepairTicketManager _repairManager;

        public MaterialLogService(
            IMaterialLogManager logManager,
            IMaterialManager materialManager,
            IEqualityHelper equalityHelper,
            IEmployeeManager employeeManager,
            IMaterialHelper materialHelper,
            IRepairTicketManager repairManager)
        {
            _logManager = logManager;
            _materialManager = materialManager;
            _equalityHelper = equalityHelper;
            _materialHelper = materialHelper;
            _employeeManager = employeeManager;
            _repairManager = repairManager;
        }
        public async Task<string> HandleCreate(MaterialLogCreateViewModel incomingViewModel)
        {
            var selectedItems = JsonSerializer.Deserialize<List<MaterialLogSelectedItemViewModel>>(incomingViewModel.SelectedProducts,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            var mappedItems = _logManager.MapSelectedItems(selectedItems);
            var incomingItems = await ValidateItems(mappedItems);

            string logId = Guid.NewGuid().ToString();

            //new Log
            var newLog = new MaterialLog()
            {
                LogId = logId,
                Status = MaterialLogStatusConst.Created,
                LogDate = incomingViewModel.MaterialLogDate,
                EmployeeId = incomingViewModel.EmployeeId
            };
            await _logManager.ManageMaterialLogAsync(newLog, EntityOperation.Create);

            var newItems = _logManager.MapNewItems(incomingItems, logId);
            await _logManager.ManageMaterialLogItemsAsync(newItems, EntityOperation.Create);

            return logId;
        }

        public async Task HandleEdit(MaterialLogDetailViewModel incomingLog)
        {
            //get original model
            var originalLogDetails = await _logManager.GetMaterialLogDTO(incomingLog.MaterialLog.LogId);
            var originalLog = originalLogDetails.MaterialLog;
            var originalItems = originalLogDetails.MaterialLogItems;
            string statusName;


            switch (originalLog.Status)
            {
                case MaterialLogStatusConst.Created:
                    statusName = MaterialLogStatusConst.CreatedName;
                    var incomingItems = await ValidateItems(incomingLog.Items);

                    if (originalLog.Approved)
                    {
                        //TODO: catch higher
                        throw new Exception($"Is already approved at '{statusName}' state and so is readonly state. No edit possible.");
                    }

                    if (!LogModified(originalLog, incomingLog.MaterialLog) && !LogItemsModified(originalItems, incomingItems, originalLog.Status))
                    {
                        //TODO: catch higher
                        throw new Exception($"Nothing no modify. Are you sure you have made any changes?");
                    }

                    if (LogModified(originalLog, incomingLog.MaterialLog))
                    {
                        //map original to history
                        var mappedHistory = await _logManager.MapLogHistoryAsync(originalLog);
                        //create record for history
                        await _logManager.ManageMaterialLogHistoryAsync(mappedHistory);

                        //map the update
                        var updatedLog = _logManager.MapUpdatedMaterialLog(originalLog, incomingLog.MaterialLog);
                        //update the log
                        await _logManager.ManageMaterialLogAsync(updatedLog, EntityOperation.Update);
                    }

                    if (LogItemsModified(originalItems, incomingItems, originalLog.Status))
                    {
                        //map original items to history
                        var mappedHistory = await _logManager.MapLogItemsHistoryAsync(originalItems);
                        //create records for history
                        await _logManager.ManageMaterialLogItemsHistoryAsync(mappedHistory);

                        //replace items
                        await _logManager.ManageMaterialLogItemsAsync(originalItems, EntityOperation.Delete);
                        await _logManager.ManageMaterialLogItemsAsync(incomingItems, EntityOperation.Create);

                        var newItems = _logManager.ReplaceItems_StatusCreated(originalItems, incomingItems);
                        //update items
                        await _logManager.ManageMaterialLogItemsAsync(newItems, EntityOperation.Create);
                    }
                    break;

                case MaterialLogStatusConst.Returned:
                    statusName = MaterialLogStatusConst.ReturnedName;

                    if (originalLog.Approved)
                    {
                        //TODO: catch higher
                        throw new Exception($"Is already approved at '{statusName}' state and so is readonly state. No edit possible.");
                    }

                    if (!LogItemsModified(originalItems, incomingLog.Items, originalLog.Status))
                    {
                        //TODO: catch higher
                        throw new Exception($"Nothing no modify. Are you sure you have made any changes?");
                    }

                    //only damaged amount, inrepairamount and deletedamount editable. also not damaged items can be damaged edited
                    if (LogItemsModified(originalItems, incomingLog.Items, originalLog.Status))
                    {
                        //map original items to history
                        var mappedHistory = await _logManager.MapLogItemsHistoryAsync(originalItems);
                        //create records for history
                        await _logManager.ManageMaterialLogItemsHistoryAsync(mappedHistory);

                        //map the update
                        var updatedItems = _logManager.MapUpdatedItems_StatusReturned(originalItems, incomingLog.Items);
                        //update items
                        await _logManager.ManageMaterialLogItemsAsync(updatedItems, EntityOperation.Update);
                    }
                    break;
                default:
                    throw new Exception("No edit possible at other than Created or Returned state.");
            }
        }
        public async Task HandleReturn(MaterialLogDetailViewModel incomingReturn)
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
            log.Status = MaterialLogStatusConst.Returned;
            //reset approved
            log.Approved = false;

            List<MaterialLogItem> modifiedItems = new();

            if (damaged)
            {
                var damagedMaterials = JsonSerializer.Deserialize<List<ReturnItemsDamagedViewModel>>(incomingReturn.DamagedMaterial,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

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
                    }
                    modifiedItems.Add(item);
                }
            }

            await _logManager.ManageMaterialLogItemsAsync(modifiedItems, EntityOperation.Update);
            await _logManager.ManageMaterialLogAsync(log, EntityOperation.Update);
        }

        public async Task HandleApprove(string logId)
        {
            var materialLogDTO = await _logManager.GetMaterialLogDTO(logId);
            var status = materialLogDTO.MaterialLog.Status;

            switch (status)
            {
                case MaterialLogStatusConst.Created:
                    await ApproveCreate(materialLogDTO);
                    break;
                case MaterialLogStatusConst.Returned:
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
            //TODO
            //check status
            //rollback extract or add amount back based on status
            //check for damaged undamaged items

            var materialLog = await _logManager.GetMaterialLogAsync(logId);

            var materialLogItems = await _logManager.GetMaterialLogItemsAsync(logId);

            //update material & remove its materiallogitems
            foreach (var item in materialLogItems)
            {
                //var material = await GetMaterialAsync(item.MaterialId);
                //_context.Update(MaterialHelper.UpdateMaterialQty(material, item.MaterialAmount, item.Used));
                //_context.MaterialLogItems.Remove(item);
            }

            //remove from database
        }

        private bool LogModified(MaterialLog original, MaterialLog incoming)
        {
            //only editable fields
            return original.LogDate != incoming.LogDate
                || original.ReturnDate != incoming.ReturnDate
                || original.EmployeeId != incoming.EmployeeId;
        }
        private bool LogItemsModified(List<MaterialLogItem> original, List<MaterialLogItem> incoming, int status)
            => !_equalityHelper.AreEditableFieldsEqual(original, incoming, status);
        private bool MaterialAmountModified(MaterialLogItem original, MaterialLogItem incoming)
            =>
            original.MaterialAmount != incoming.MaterialAmount ||
            original.DamagedAmount != incoming.DamagedAmount ||
            original.RepairAmount != incoming.RepairAmount ||
            original.DeleteAmount != incoming.DeleteAmount;

        private async Task<List<MaterialLogItem>> ValidateItems(List<MaterialLogItem> selectedItems)
        {
            var mergedItems = MergeItems(selectedItems);

            foreach (var item in mergedItems)
            {
                var material = await _materialManager.GetMaterialAsync(item.MaterialId);
                _materialHelper.ValidateQuantity(material, item.MaterialAmount, item.Used);
            }

            return mergedItems;
        }

        //TODO: check if this works correctly and if necessary (you can restrict user from view maybe)
        private List<MaterialLogItem> MergeItems(List<MaterialLogItem> selectedItems)
        {
            return selectedItems
                .GroupBy(x => new { x.MaterialId, x.Used })
                .Select(group => new MaterialLogItem
                {
                    MaterialId = group.Key.MaterialId,
                    MaterialAmount = group.Sum(x => x.MaterialAmount),
                    Used = group.Key.Used
                }).ToList();
        }

    }
}
