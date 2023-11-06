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

namespace WebApp_GozenBv.Services
{
    public class MaterialLogService : IMaterialLogService
    {
        private readonly IMaterialLogManager _logManager;
        private readonly IMaterialManager _materialManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IMaterialHelper _materialHelper;
        private readonly IEqualityHelper _equalityHelper;

        public MaterialLogService(
            IMaterialLogManager logManager,
            IMaterialManager materialManager,
            IEqualityHelper equalityHelper,
            IEmployeeManager employeeManager,
            IMaterialHelper materialHelper)
        {
            _logManager = logManager;
            _materialManager = materialManager;
            _equalityHelper = equalityHelper;
            _materialHelper = materialHelper;
            _employeeManager = employeeManager;
        }
        public async Task<string> HandleCreate(MaterialLogCreateViewModel incomingViewModel)
        {
            var selectedItems = JsonSerializer.Deserialize<List<MaterialLogSelectedItemViewModel>>(incomingViewModel.SelectedProducts,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            await ValidateItems(selectedItems);

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

            List<MaterialLogItem> newItems = new();
            foreach (var item in selectedItems)
            {
                //new Item
                MaterialLogItem newItem = new();
                var material = await _materialManager.GetMaterialAsync(item.MaterialId);

                if (material.NoReturn)
                {
                    newItem.DamagedAmount = null;
                    newItem.RepairAmount = null;
                    newItem.DeleteAmount = null;
                }
                newItem.MaterialId = item.MaterialId;
                newItem.MaterialAmount = item.Amount;
                newItem.Used = item.Used;
                newItem.LogId = logId;

                newItems.Add(newItem);
            }
            await _logManager.ManageMaterialLogItemsAsync(newItems, EntityOperation.Create);

            return logId;
        }

        private async Task ValidateItems(List<MaterialLogSelectedItemViewModel> selectedItems)
        {
            foreach (var item in selectedItems)
            {
                var material = await _materialManager.GetMaterialAsync(item.MaterialId);
                _materialHelper.ValidateQuantity(material, item.Amount, item.Used);
            }
        }

        public async Task HandleEdit(MaterialLogDetailViewModel incomingLog)
        {
            //get original model
            var originalLogDetails = await _logManager.GetMaterialLogDTO(incomingLog.MaterialLog.LogId);
            var originalLog = originalLogDetails.MaterialLog;
            var originalItems = originalLogDetails.MaterialLogItems;
            string statusName;

            List<MaterialLogItem> modifiedItems = new();



            switch (originalLog.Status)
            {
                case MaterialLogStatusConst.Created:
                    statusName = MaterialLogStatusConst.CreatedName;

                    if (originalLog.Approved)
                    {
                        //TODO: catch higher
                        throw new Exception($"Is already approved at '{statusName}' state and so is readonly state. No edit possible.");
                    }

                    if (!LogModified(originalLog, incomingLog.MaterialLog) && !LogItemsModified(originalItems, incomingLog.Items, originalLog.Status))
                    {
                        //TODO: catch higher
                        throw new Exception($"Nothing no modify. Are you sure you have made any changes?");
                    }

                    if (LogModified(originalLog, incomingLog.MaterialLog))
                    {
                        //map the update
                        var updatedLog = _logManager.MapUpdatedMaterialLog(originalLog, incomingLog.MaterialLog);
                        //update the log
                        await _logManager.ManageMaterialLogAsync(updatedLog, EntityOperation.Update);

                        //map original to history
                        var mappedHistory = await _logManager.MapLogHistoryAsync(originalLog);
                        //create record for history
                        await _logManager.ManageMaterialLogHistoryAsync(mappedHistory);

                    }

                    if (LogItemsModified(originalItems, incomingLog.Items, originalLog.Status))
                    {
                        //map the update
                        var updatedItems = _logManager.MapUpdatedItems_StatusCreated(originalItems, incomingLog.Items);
                        //update items
                        await _logManager.ManageMaterialLogItemsAsync(updatedItems, EntityOperation.Update);

                        //map original items to history
                        var mappedHistory = await _logManager.MapLogItemsHistoryAsync(originalItems);
                        //create records for history
                        await _logManager.ManageMaterialLogItemsHistoryAsync(mappedHistory);

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
                        //map the update
                        var updatedItems = _logManager.MapUpdatedItems_StatusReturned(originalItems, incomingLog.Items);
                        //update items
                        await _logManager.ManageMaterialLogItemsAsync(updatedItems, EntityOperation.Update);

                        //map original items to history
                        var mappedHistory = await _logManager.MapLogItemsHistoryAsync(originalItems);
                        //create records for history
                        await _logManager.ManageMaterialLogItemsHistoryAsync(mappedHistory);
                    }
                    break;
                default:
                    throw new Exception("No edit possible at other than Created or Returned state.");
            }
        }
        public void HandleReturn(MaterialLogDetailViewModel incomingReturn)
        {
            //incoming
            string logId = incomingReturn.MaterialLog.LogId;
            var damaged = incomingReturn.MaterialLog.Damaged;

            //original
            var log = _logManager.GetMaterialLog(logId);
            var originalLogItems = _logManager.GetMaterialLogItems(logId);

            if (log == null)
            {
                throw new NullReferenceException($"log with Id {logId} does not exist");
            }

            log.ReturnDate = DateTime.Now;
            log.Status = MaterialLogStatusConst.Returned;

            List<MaterialLogItem> mappedItems = new();

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

                    var mappedItem = _logManager.MapReturnedItem(item);
                    mappedItems.Add(mappedItem);
                }
            }
            else
            {
                foreach (var item in originalLogItems)
                {
                    var mappedItem = _logManager.MapReturnedItem(item);
                    mappedItems.Add(mappedItem);
                }
            }

            _logManager.ManageMaterialLogItems(mappedItems, EntityOperation.Create);

            var mappedLog = _logManager.MapReturnedLog(log);
            _logManager.ManageMaterialLog(mappedLog, EntityOperation.Create);
        }

        public async Task HandleDamaged(MaterialLogDetailViewModel incomingComplete)
        {
            var logId = incomingComplete.MaterialLog.LogId;
            var originalLog = await _logManager.GetMaterialLogAsync(logId);
            var originalItems = await _logManager.GetMaterialLogItemsAsync(logId);

            List<Material> modifiedMaterials = new();
            List<MaterialLogItem> modifiedItems = new();

            if (originalLog == null)
            {
                throw new NullReferenceException($"log with Id {logId} does not exist");
            }

            originalLog.Status = MaterialLogStatusConst.Returned;

            var completeDamagedMaterials = JsonSerializer.Deserialize<List<CompleteDamagedMaterialViewModel>>(incomingComplete.DamagedMaterial,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            //update MaterialLogItems
            foreach (var item in originalItems)
            {
                var damagedItem = completeDamagedMaterials.FirstOrDefault(di => di.MaterialId == item.MaterialId);

                if (damagedItem != null)
                {
                    var originalMaterial = await _materialManager.MapMaterialAsync(damagedItem.MaterialId);
                    item.RepairAmount = damagedItem.RepairedAmount;
                    item.DeleteAmount = damagedItem.DeletedAmount;

                    modifiedItems.Add(item);
                    modifiedMaterials.Add(_materialHelper.AddToUsed(originalMaterial, damagedItem.RepairedAmount));
                }
            }
            await _logManager.ManageMaterialLogAsync(originalLog, EntityOperation.Update);
            await _logManager.ManageMaterialLogItemsAsync(modifiedItems, EntityOperation.Update);
            await _materialManager.ManageMaterialsAsync(modifiedMaterials, EntityOperation.Update);
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

        public async Task HandleApprove(string logId)
        {
            //check for status

            //if created
            //
        }
    }
}
