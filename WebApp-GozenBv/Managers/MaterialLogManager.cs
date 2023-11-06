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

        public async Task<MaterialLog> GetMaterialLogAsync(string logId)
        {
            return await _logData.QueryMaterialLogByLogIdAsync(logId);
        }

        public async Task<List<MaterialLogItem>> GetMaterialLogItemsAsync(string logId)
        {
            return await _itemData.QueryItemsByLogIdAsync(logId);
        }

        public async Task<List<MaterialLog>> GetMaterialLogs()
        {
            return await _logData.QueryMaterialLogs();
        }
        public async Task<MaterialLogDetailViewModel> GetMaterialLogDetails(string logId)
        {
            var log = await _logData.QueryMaterialLogByLogIdAsync(logId);

            if (log == null)
            {
                throw new ArgumentNullException($"MaterialLog with logId {logId} does not exist.");
            }

            List<MaterialLogItem> undamagedItems, damagedItems = new();

            if (log.Damaged)
            {
                undamagedItems = await _itemData.QueryUnDamagedItemsByLogId(logId);
                damagedItems = await _itemData.QueryDamagedItemsByLogId(logId);
            }
            else
            {
                undamagedItems = await _itemData.QueryItemsByLogIdAsync(logId);
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

        public MaterialLog GetMaterialLog(string logId)
        {
            return _logData.QueryMaterialLogByLogId(logId);
        }

        public List<MaterialLogItem> GetMaterialLogItems(string logId)
        {
            return _itemData.QueryItemsByLogId(logId);
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
                Status = MaterialLogStatusConst.Created,
                Approved = false,
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
                RepairAmount = 0,
                DeleteAmount = 0,
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
                RepairAmount = 0,
                DeleteAmount = 0,
                EditStatus = EditStatus.Returned,
                Version = incoming.Version++
            };
        }

        public MaterialLogItem GetMaterialLogItemStatusReturnedDamaged(MaterialLogItem incoming)
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
                RepairAmount = incoming.RepairAmount,
                DeleteAmount = incoming.DeleteAmount,
                EditStatus = EditStatus.Returned,
                Version = incoming.Version++
            };
        }

        public MaterialLog MapReturnedLog(MaterialLog log)
        {
            return new MaterialLog()
            {
                LogDate = log.LogDate,
                EmployeeId = log.EmployeeId,
                LogId = log.LogId,
                ReturnDate = log.ReturnDate,
                Damaged = log.Damaged,
                Status = MaterialLogStatusConst.Returned,
                Approved = false,
                Version = log.Version++
            };
        }

        public MaterialLogItem MapReturnedItem(MaterialLogItem item)
        {
            return new MaterialLogItem()
            {
                LogId = item.LogId,
                MaterialId = item.MaterialId,
                MaterialAmount = item.MaterialAmount,
                ProductNameCode = item.ProductNameCode,
                NoReturn = item.NoReturn,
                Cost = item.Cost,
                Used = item.Used,
                IsDamaged = item.IsDamaged,
                DamagedAmount = item.DamagedAmount,
                RepairAmount = item.RepairAmount,
                DeleteAmount = item.DeleteAmount,
                EditStatus = EditStatus.Returned,
                Version = item.Version++
            };
        }

    }
}

