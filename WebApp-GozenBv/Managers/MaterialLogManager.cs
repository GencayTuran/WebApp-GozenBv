using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public MaterialLogManager(
            IMaterialLogDataHandler logData,
            IMaterialLogItemDataHandler itemData)
        {
            _logData = logData;
            _itemData = itemData;
        }

        public async Task ManageMaterialLogAsync(MaterialLog log, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _logData.CreateMaterialLogAsync(log);
                    break;
                case EntityOperation.Update:
                    await _logData.UpdateMaterialLogAsync(log);
                    break;
                case EntityOperation.Delete:
                    await _logData.DeleteMaterialLogAsync(log);
                    break;
            }
        }

        public async Task ManageMaterialLogItemsAsync(List<MaterialLogItem> items, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _itemData.CreateItemsAsync(items);
                    break;
                case EntityOperation.Update:
                    await _itemData.UpdateItemsAsync(items);
                    break;
                case EntityOperation.Delete:
                    await _itemData.DeleteItemsAsync(items);
                    break;
            }
        }

        public async Task<MaterialLog> MapMaterialLogAsync(string logId)
        {
            return await _logData.GetMaterialLogByLogIdAsync(logId);
        }

        public async Task<List<MaterialLogItem>> MapMaterialLogItemsAsync(string logId)
        {
            return await _itemData.GetItemsByLogIdAsync(logId);
        }

        public async Task<List<MaterialLog>> MapMaterialLogs()
        {
            return await _logData.GetMaterialLogs();
        }
        public async Task<MaterialLogDetailViewModel> MapMaterialLogDetails(string logId)
        {
            var log = await _logData.GetMaterialLogByLogIdAsync(logId);

            if (log == null)
            {
                throw new ArgumentNullException($"MaterialLog with logId {logId} does not exist.");
            }

            List<MaterialLogItem> undamagedItems, damagedItems = new();

            if (log.Damaged)
            {
                undamagedItems = await _itemData.GetUnDamagedItemsByLogId(logId);
                damagedItems = await _itemData.GetDamagedItemsByLogId(logId);
            }
            else
            {
                undamagedItems = await _itemData.GetItemsByLogIdAsync(logId);
            }

            return new MaterialLogDetailViewModel
            {
                MaterialLog = new MaterialLog
                {
                    Id = log.Id,
                    LogId = log.LogId,
                    LogDate = log.LogDate,
                    Employee = log.Employee,
                    EmployeeId = log.EmployeeId,
                    ReturnDate = log.ReturnDate,
                    Status = log.Status,
                    Damaged = log.Damaged,
                },
                Items = undamagedItems,
                ItemsDamaged = damagedItems,
                EmployeeFullName = (log.Employee.Name + " " + log.Employee.Surname).ToUpper(),
            };
        }

        public void ManageMaterialLogItems(List<MaterialLogItem> items, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    _itemData.CreateItems(items);
                    break;
                case EntityOperation.Update:
                    _itemData.UpdateItems(items);
                    break;
                case EntityOperation.Delete:
                    _itemData.DeleteItems(items);
                    break;
            }
        }

        public void ManageMaterialLog(MaterialLog log, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    _logData.CreateMaterialLog(log);
                    break;
                case EntityOperation.Update:
                    _logData.UpdateMaterialLog(log);
                    break;
                case EntityOperation.Delete:
                    _logData.DeleteMaterialLog(log);
                    break;
            }
        }

        public MaterialLog MapMaterialLog(string logId)
        {
            return _logData.GetMaterialLogByLogId(logId);
        }

        public List<MaterialLogItem> MapMaterialLogItems(string logId)
        {
            return _itemData.GetItemsByLogId(logId);
        }

        public MaterialLog MapMaterialLogStatusCreated(MaterialLog original, MaterialLog incoming)
        {
            return new MaterialLog()
            {
                LogDate = incoming.LogDate,
                EmployeeId = incoming.EmployeeId,
                LogId = incoming.LogId,
                ReturnDate = incoming.ReturnDate,
                Damaged = false,
                Status = EditStatus.Created,
                Approved = false,
                Version = original.Version++
            };
        }

        public MaterialLogItem MapMaterialLogItemStatusCreated(MaterialLogItem incoming)
        {
            return new MaterialLogItem()
            {
                LogId = incoming.LogId,
                MaterialId = incoming.MaterialId,
                MaterialAmount = incoming.MaterialAmount,
                ProductNameCode = incoming.ProductNameCode,
                NoReturn = incoming.NoReturn,
                Cost = incoming.Cost,
                Used = incoming.Used,
                IsDamaged = false,
                DamagedAmount = 0,
                RepairedAmount = 0,
                DeletedAmount = 0,
                EditStatus = EditStatus.Created,
                Version = incoming.Version++
            };
        }

        public MaterialLogItem MapMaterialLogItemStatusReturned(MaterialLogItem incoming)
        {
            return new MaterialLogItem()
            {
                LogId = incoming.LogId,
                MaterialId = incoming.MaterialId,
                MaterialAmount = incoming.MaterialAmount,
                ProductNameCode = incoming.ProductNameCode,
                NoReturn = incoming.NoReturn,
                Cost = incoming.Cost,
                Used = incoming.Used,
                IsDamaged = false,
                DamagedAmount = 0,
                RepairedAmount = 0,
                DeletedAmount = 0,
                EditStatus = EditStatus.Returned,
                Version = incoming.Version++
            };
        }

        public MaterialLogItem MapMaterialLogItemStatusReturnedDamaged(MaterialLogItem incoming)
        {
            return new MaterialLogItem()
            {
                LogId = incoming.LogId,
                MaterialId = incoming.MaterialId,
                MaterialAmount = incoming.MaterialAmount,
                ProductNameCode = incoming.ProductNameCode,
                NoReturn = incoming.NoReturn,
                Cost = incoming.Cost,
                Used = incoming.Used,
                IsDamaged = false,
                DamagedAmount = incoming.DamagedAmount,
                RepairedAmount = incoming.RepairedAmount,
                DeletedAmount = incoming.DeletedAmount,
                EditStatus = EditStatus.Returned,
                Version = incoming.Version++
            };
        }

    }
}

