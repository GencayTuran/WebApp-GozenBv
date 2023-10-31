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

            //new materiallog
            var newLog = new MaterialLog()
            {
                LogId = logId,
                Status = MaterialLogStatusConst.Created,
                LogDate = incomingViewModel.MaterialLogDate,
                EmployeeId = incomingViewModel.EmployeeId
            };
            await _logManager.ManageMaterialLogAsync(newLog, EntityOperation.Create);

            List<MaterialLogItem> newItems = new();
            List<Material> updatedMaterials = new();
            foreach (var item in selectedItems)
            {
                //new MaterialLogItem
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
                newItem.ProductNameCode = (material.ProductName + " " + material.ProductCode).ToUpper();

                newItems.Add(newItem);
                updatedMaterials.Add(_materialHelper.TakeMaterial(material, item.Amount, item.Used));
            }
            await _materialManager.ManageMaterialsAsync(updatedMaterials, EntityOperation.Update);
            await _logManager.ManageMaterialLogItemsAsync(newItems, EntityOperation.Create);

            return logId;
        }
        public async Task HandleEdit(MaterialLogDetailViewModel incomingLog)
        {
            //get original model
            var logDetails = await _logManager.MapMaterialLogDetails(incomingLog.MaterialLog.LogId);
            var log = logDetails.MaterialLog;
            var undamagedItems = logDetails.ItemsUndamaged;
            var damagedItems = logDetails.ItemsDamaged;

            if (IsReadOnlyState(log.Status))
            {
                throw new Exception("Is read-only state. No edit possible");
            }

            //compare each class with incoming
            if (LogModified(log, incomingLog.MaterialLog))
            {
                await _logManager.ManageMaterialLogAsync(incomingLog.MaterialLog, EntityOperation.Update);
            }
            if (LogItemsModified(undamagedItems, incomingLog.ItemsUndamaged))
            {
                List<MaterialLogItem> modifiedItems = new();
                List<Material> modifiedMaterials = new();
                foreach (var item in undamagedItems)
                {
                    var incomingItem = incomingLog.ItemsUndamaged.FirstOrDefault(i => i.Id == i.Id);
                    // check if they arent equal here (override equals)
                    if (!_equalityHelper.AreEqual(item, incomingItem))
                    {
                        if (incomingItem.MaterialAmount != item.MaterialAmount)
                        {
                            var material = await _materialManager.MapMaterialAsync(incomingItem.MaterialId);
                            var amountDifference = item.MaterialAmount - incomingItem.MaterialAmount;
                            //update material amount
                            modifiedMaterials.Add(_materialHelper.UpdateMaterialQty(material, amountDifference, incomingItem.Used));
                        }
                        //TODO: further checks of props
                        
                        modifiedItems.Add(incomingItem);
                    }
                }

                if (modifiedMaterials.Any())
                {
                    await _materialManager.ManageMaterialsAsync(modifiedMaterials, EntityOperation.Update);
                }
                await _logManager.ManageMaterialLogItemsAsync(modifiedItems, EntityOperation.Update);
            }
            if (LogItemsModified(logDetails.ItemsDamaged, incomingLog.ItemsDamaged))
            {
                List<MaterialLogItem> modifiedItems = new();
                List<Material> modifiedMaterials = new();
                foreach (var originalItem in logDetails.ItemsUndamaged)
                {
                    var incomingItem = incomingLog.ItemsUndamaged.FirstOrDefault(item => item.Id == originalItem.Id);
                    // check if they arent equal here (override equals)
                    if (!_equalityHelper.AreEqual(originalItem, incomingItem))
                    {
                        if (MaterialAmountModified(originalItem, incomingItem))
                        {
                            var material = await _materialManager.MapMaterialAsync(incomingItem.MaterialId);

                            if (incomingItem.MaterialAmount != originalItem.MaterialAmount)
                            {
                                var amountDifference = originalItem.MaterialAmount - incomingItem.MaterialAmount;
                                //update material amount
                                modifiedMaterials.Add(_materialHelper.UpdateMaterialQty(material, amountDifference, incomingItem.Used));
                            }

                            //TODO: further checks of props + this function is a copy from above.
                        }


                        modifiedItems.Add(incomingItem);
                    }
                }

                await _logManager.ManageMaterialLogItemsAsync(incomingLog.ItemsDamaged, EntityOperation.Update);
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
        private bool LogItemsModified(List<MaterialLogItem> original, List<MaterialLogItem> incoming) => !original.SequenceEqual(incoming); //TODO: check if this works right
        private bool MaterialAmountModified(MaterialLogItem original, MaterialLogItem incoming)
            =>
            original.MaterialAmount != incoming.MaterialAmount ||
            original.DamagedAmount != incoming.DamagedAmount ||
            original.RepairedAmount != incoming.RepairedAmount ||
            original.DeletedAmount != incoming.DeletedAmount;

        private bool IsReadOnlyState(int status) => status == MaterialLogStatusConst.Returned;
    }
}
