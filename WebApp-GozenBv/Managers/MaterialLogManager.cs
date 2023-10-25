using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Helpers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers
{
	public class MaterialLogManager : IMaterialLogManager
	{
        private readonly IMaterialLogDataHandler _logData;
        private readonly IMaterialLogItemDataHandler _itemData;

        private readonly IMaterialManager _materialManager;
        public MaterialLogManager(
            IMaterialLogDataHandler logData,
            IMaterialLogItemDataHandler itemData,
            IMaterialManager materialManager)
		{
            _logData = logData;
            _itemData = itemData;
            _materialManager = materialManager;
		}

        public async Task ManageMaterialLog(MaterialLog log, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _logData.CreateMaterialLog(log);
                    break;
                case EntityOperation.Update:
                    await _logData.UpdateMaterialLog(log);
                    break;
                case EntityOperation.Delete:
                    await _logData.DeleteMaterialLog(log);
                    break;
            }
        }

        public async Task ManageMaterialLogItems(List<MaterialLogItem> items, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _itemData.CreateItems(items);
                    break;
                case EntityOperation.Update:
                    await _itemData.UpdateItems(items);
                    break;
                case EntityOperation.Delete:
                    await _itemData.DeleteItems(items);
                    break;
            }
        }

        public async Task<string> MapIncomingLog(MaterialLogCreateViewModel incomingViewModel)
        {
            var selectedItems = JsonSerializer.Deserialize<List<MaterialLogSelectedItemViewModel>>(incomingViewModel.SelectedProducts,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            string logCode = Guid.NewGuid().ToString();

            //new materiallog
            var newLog = new MaterialLog()
            {
                LogCode = logCode,
                Status = MaterialLogStatusConst.Created,
                MaterialLogDate = incomingViewModel.MaterialLogDate,
                EmployeeId = incomingViewModel.EmployeeId
            };
            await ManageMaterialLog(newLog, EntityOperation.Create);

            List<MaterialLogItem> newItems = new();
            List<Material> updatedMaterials = new();
            foreach (var item in selectedItems)
            {
                //new MaterialLogItem
                MaterialLogItem newItem = new();
                var material = await _materialManager.MapMaterial(item.MaterialId);

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
                newItem.LogCode = logCode;
                newItem.ProductNameCode = (material.ProductName + " " + material.ProductCode).ToUpper();

                newItems.Add(newItem);
                updatedMaterials.Add(MaterialHelper.TakeMaterial(material, item.Amount, item.Used));
            }
            await _materialManager.ManageMaterials(updatedMaterials, EntityOperation.Update);
            await _itemData.CreateItems(newItems);

            return logCode;
        }

        public Task<List<MaterialLogItem>> MapItemsByLogId(int? logId)
        {
            throw new NotImplementedException();
        }

        public async Task<MaterialLogDetailViewModel> MapMaterialLogDetails(string logCode)
        {
            var log = await _logData.GetMaterialLogByLogCode(logCode);
            List<MaterialLogItem> items, damagedItems = new();

            if (log.Damaged)
            {
                items = await _itemData.GetUnDamagedItemsByLogCode(logCode);
                damagedItems = await _itemData.GetDamagedItemsByLogCode(logCode);
            }
            else
            {
                items = await _itemData.GetItemsByLogCode(logCode);
            }

            return new MaterialLogDetailViewModel
            {
                MaterialLogId = log.Id,
                MaterialLogDate = log.MaterialLogDate,
                EmployeeFullName = (log.Employee.Name + " " + log.Employee.Surname).ToUpper(),
                LogCode = log.LogCode,
                MaterialLogItems = items,
                MaterialLogItemsDamaged = damagedItems,
                ReturnDate = log.ReturnDate,
                Status = log.Status,
                IsDamaged = log.Damaged,
            };
        }

        public Task<MaterialLogItem> MapMaterialLogItem(string logCode)
        {
            throw new NotImplementedException();
        }

        public Task<List<MaterialLog>> MapMaterialLogs()
        {
            throw new NotImplementedException();
        }
    }
}

