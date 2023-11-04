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

            string logId = Guid.NewGuid().ToString();

            //new Log
            var newLog = new MaterialLog()
            {
                LogId = logId,
                Status = MaterialLogStatusConst.Created,
                LogDate = incomingViewModel.MaterialLogDate,
                EmployeeId = incomingViewModel.EmployeeId,
                Version = 1

            };
            await _logManager.ManageMaterialLogAsync(newLog, EntityOperation.Create);

            List<MaterialLogItem> newItems = new();
            foreach (var item in selectedItems)
            {
                //new Item
                MaterialLogItem newItem = new();
                var material = await _materialManager.MapMaterialAsync(item.MaterialId);

                if (material.NoReturn)
                {
                    newItem.DamagedAmount = null;
                    newItem.RepairedAmount = null;
                    newItem.DeletedAmount = null;
                }

                newItem.MaterialId = item.MaterialId;
                newItem.MaterialAmount = item.Amount;
                newItem.Used = item.Used;
                newItem.Cost = material.Cost;
                newItem.LogId = logId;
                newItem.ProductNameCode = (material.Name + " " + material.Brand).ToUpper();

                newItem.Version = 1;
                newItem.EditStatus = EditStatus.Created;

                newItems.Add(newItem);
            }
            await _logManager.ManageMaterialLogItemsAsync(newItems, EntityOperation.Create);

            return logId;
        }
        public async Task HandleEdit(MaterialLogDetailViewModel incomingLog)
        {
            //get original model
            var logDetails = await _logManager.MapMaterialLogDetails(incomingLog.MaterialLog.LogId);
            var log = logDetails.MaterialLog;
            var materialLogItems = logDetails.Items;
            var damagedItems = logDetails.ItemsDamaged;

            List<MaterialLogItem> modifiedItems = new();
            List<string> validationErrors = new();

            string statusName;

            switch (log.Status)
            {
                case MaterialLogStatusConst.Created:
                    statusName = MaterialLogStatusConst.CreatedName;

                    if (log.Approved)
                    {
                        //TODO: catch higher
                        throw new Exception($"Is already approved at '{statusName}' state and so is readonly state. No edit possible.");
                    }

                    if (!LogModified(log, incomingLog.MaterialLog) && !LogItemsModified(materialLogItems, incomingLog.Items))
                    {
                        //TODO: catch higher
                        throw new Exception($"Nothing no modify. Are you sure you have made any changes?");
                    }

                    if (LogModified(log, incomingLog.MaterialLog))
                    {
                        var updatedLog = _logManager.MapMaterialLogStatusCreated(log, incomingLog.MaterialLog);
                        await _logManager.ManageMaterialLogAsync(updatedLog, EntityOperation.Create);
                    }

                    if (LogItemsModified(materialLogItems, incomingLog.Items))
                    {
                        foreach (var item in incomingLog.Items)
                        {
                            var updatedItem = _logManager.MapMaterialLogItemStatusCreated(item);
                            modifiedItems.Add(updatedItem);
                        }
                        await _logManager.ManageMaterialLogItemsAsync(modifiedItems, EntityOperation.Create);
                    }
                    break;

                case MaterialLogStatusConst.Returned:
                    statusName = MaterialLogStatusConst.ReturnedName;

                    if (log.Approved)
                    {
                        //TODO: catch higher
                        throw new Exception($"Is already approved at '{statusName}' state and so is readonly state. No edit possible.");
                    }

                    if (!LogItemsModified(materialLogItems, incomingLog.Items))
                    {
                        //TODO: catch higher
                        throw new Exception($"Nothing no modify. Are you sure you have made any changes?");
                    }

                    //only damaged amount, inrepairamount and deletedamount editable. also not damaged items can be damaged edited

                    if (!log.Damaged)
                    {
                        foreach (var item in incomingLog.Items)
                        {
                            var updatedItem = _logManager.MapMaterialLogItemStatusReturned(item);
                            modifiedItems.Add(item);
                        }
                    }
                    else
                    {
                        foreach (var item in incomingLog.Items)
                        {
                            var updatedItem = _logManager.MapMaterialLogItemStatusReturned(item);
                            modifiedItems.Add(item);
                        }

                        foreach (var item in incomingLog.ItemsDamaged)
                        {
                            var updatedItem = _logManager.MapMaterialLogItemStatusReturnedDamaged(item);
                            modifiedItems.Add(item);
                        }
                    }
                    break;
                default:
                    throw new Exception("No edit possible at other than Created or Returned state.");
            }
        }
        public void HandleReturn(MaterialLogDetailViewModel incomingReturn)
        {
            string logId = incomingReturn.MaterialLog.LogId;
            var damaged = incomingReturn.MaterialLog.Damaged;

            var log = _logManager.MapMaterialLog(logId);
            var logItems = _logManager.MapMaterialLogItems(logId);


            List<Material> modifiedMaterials = new();

            if (log == null)
            {
                throw new NullReferenceException($"log with Id {logId} does not exist");
            }

            log.ReturnDate = DateTime.Now;

            if (damaged)
            {
                var damagedMaterials = JsonSerializer.Deserialize<List<ReturnItemsDamagedViewModel>>(incomingReturn.DamagedMaterial,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                log.Status = MaterialLogStatusConst.DamagedAwaitingAction;
                log.Damaged = damaged;

                List<MaterialLogItem> modifiedItems = new();

                //update MaterialLogItems
                foreach (var item in logItems)
                {
                    var damagedItem = damagedMaterials.FirstOrDefault(di => di.MaterialId == item.MaterialId);
                    var originalMaterial = _materialManager.MapMaterial(item.MaterialId);

                    if (damagedItem != null)
                    {
                        item.DamagedAmount = damagedItem.DamagedAmount;
                        item.IsDamaged = true;
                        modifiedItems.Add(item);

                        //undamaged amount of material adding to used
                        var notDamagedAmount = item.MaterialAmount - damagedItem.DamagedAmount;
                        if (notDamagedAmount != 0)
                        {
                            modifiedMaterials.Add(_materialHelper.AddToUsed(originalMaterial, notDamagedAmount));
                        }
                    }
                    else
                    {
                        //adding all amount of material
                        modifiedMaterials.Add(_materialHelper.AddToUsed(originalMaterial, item.MaterialAmount));
                    }
                }
                _logManager.ManageMaterialLogItems(modifiedItems, EntityOperation.Update);
            }
            else //TODO: else added afterwards.. was this missing? is this correct?
            {
                log.Status = MaterialLogStatusConst.Returned;

                //material add to used
                foreach (var item in logItems)
                {
                    var originalMaterial = _materialManager.MapMaterial(item.MaterialId);
                    modifiedMaterials.Add(_materialHelper.AddToUsed(originalMaterial, item.MaterialAmount));
                }
            }
            _logManager.ManageMaterialLog(log, EntityOperation.Update);
            _materialManager.ManageMaterials(modifiedMaterials, EntityOperation.Update);
        }
        public async Task HandleDamaged(MaterialLogDetailViewModel incomingComplete)
        {
            var logId = incomingComplete.MaterialLog.LogId;
            var originalLog = await _logManager.MapMaterialLogAsync(logId);
            var originalItems = await _logManager.MapMaterialLogItemsAsync(logId);

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
                    item.RepairedAmount = damagedItem.RepairedAmount;
                    item.DeletedAmount = damagedItem.DeletedAmount;

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

            var materialLog = await _logManager.MapMaterialLogAsync(logId);

            var materialLogItems = await _logManager.MapMaterialLogItemsAsync(logId);

            //update material & remove its materiallogitems
            foreach (var item in materialLogItems)
            {
                //var material = await GetMaterialAsync(item.MaterialId);
                //_context.Update(MaterialHelper.UpdateMaterialQty(material, item.MaterialAmount, item.Used));
                //_context.MaterialLogItems.Remove(item);
            }

            //remove from database
        }

        private bool LogModified(MaterialLog original, MaterialLog incoming) => !_equalityHelper.AreEqual(original, incoming);
        private bool LogItemsModified(List<MaterialLogItem> original, List<MaterialLogItem> incoming) => !_equalityHelper.AreCollectionsEqual(original, incoming);
        private bool MaterialAmountModified(MaterialLogItem original, MaterialLogItem incoming)
            =>
            original.MaterialAmount != incoming.MaterialAmount ||
            original.DamagedAmount != incoming.DamagedAmount ||
            original.RepairedAmount != incoming.RepairedAmount ||
            original.DeletedAmount != incoming.DeletedAmount;

        

        
    }
}
