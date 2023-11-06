using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Graph.ExternalConnectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.DataHandlers.Interfaces;
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
        private readonly IHistoryDataHandler _historyData;

        public MaterialLogManager(
            IMaterialLogDataHandler logData,
            IMaterialLogItemDataHandler itemData,
            IHistoryDataHandler historyData)
        {
            _logData = logData;
            _itemData = itemData;
            _historyData = historyData;
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

        

        //public MaterialLogItem MapMaterialLogItemStatusCreated(MaterialLogItem incoming)
        //{
        //    return new MaterialLogItem()
        //    {
        //        LogId = incoming.LogId,
        //        MaterialId = incoming.MaterialId,
        //        MaterialAmount = incoming.MaterialAmount,
        //        ProductNameCode = incoming.ProductNameCode,
        //        NoReturn = incoming.NoReturn,
        //        Cost = incoming.Cost,
        //        Used = incoming.Used,
        //        IsDamaged = false,
        //        DamagedAmount = 0,
        //        RepairAmount = 0,
        //        DeleteAmount = 0,
        //        EditStatus = EditStatus.Created,
        //        Version = incoming.Version++
        //    };
        //}

        //public MaterialLogItem MapMaterialLogItemStatusReturned(MaterialLogItem incoming)
        //{
        //    return new MaterialLogItem()
        //    {
        //        LogId = incoming.LogId,
        //        MaterialId = incoming.MaterialId,
        //        MaterialAmount = incoming.MaterialAmount,
        //        ProductNameCode = incoming.ProductNameCode,
        //        NoReturn = incoming.NoReturn,
        //        Cost = incoming.Cost,
        //        Used = incoming.Used,
        //        IsDamaged = false,
        //        DamagedAmount = 0,
        //        RepairAmount = 0,
        //        DeleteAmount = 0,
        //        EditStatus = EditStatus.Returned,
        //        Version = incoming.Version++
        //    };
        //}

        //public MaterialLogItem MapMaterialLogItemStatusReturnedDamaged(MaterialLogItem incoming)
        //{
        //    return new MaterialLogItem()
        //    {
        //        LogId = incoming.LogId,
        //        MaterialId = incoming.MaterialId,
        //        MaterialAmount = incoming.MaterialAmount,
        //        ProductNameCode = incoming.ProductNameCode,
        //        NoReturn = incoming.NoReturn,
        //        Cost = incoming.Cost,
        //        Used = incoming.Used,
        //        IsDamaged = false,
        //        DamagedAmount = incoming.DamagedAmount,
        //        RepairAmount = incoming.RepairAmount,
        //        DeleteAmount = incoming.DeleteAmount,
        //        EditStatus = EditStatus.Returned,
        //        Version = incoming.Version++
        //    };
        //}

        //public MaterialLog MapReturnedLog(MaterialLog log)
        //{
        //    return new MaterialLog()
        //    {
        //        LogDate = log.LogDate,
        //        EmployeeId = log.EmployeeId,
        //        LogId = log.LogId,
        //        ReturnDate = log.ReturnDate,
        //        Damaged = log.Damaged,
        //        Status = MaterialLogStatusConst.Returned,
        //        Approved = false,
        //        Version = log.Version++
        //    };
        //}

        //public MaterialLogItem MapReturnedItem(MaterialLogItem item)
        //{
        //    return new MaterialLogItem()
        //    {
        //        LogId = item.LogId,
        //        MaterialId = item.MaterialId,
        //        MaterialAmount = item.MaterialAmount,
        //        ProductNameCode = item.ProductNameCode,
        //        NoReturn = item.NoReturn,
        //        Cost = item.Cost,
        //        Used = item.Used,
        //        IsDamaged = item.IsDamaged,
        //        DamagedAmount = item.DamagedAmount,
        //        RepairAmount = item.RepairAmount,
        //        DeleteAmount = item.DeleteAmount,
        //        EditStatus = EditStatus.Returned,
        //        Version = item.Version++
        //    };
        //}

        public async Task ManageMaterialLogHistoryAsync(MaterialLogHistory entity)
        {
           await _historyData.CreateMaterialLogHistoryAsync(entity);
        }

        public async Task ManageMaterialLogItemsHistoryAsync(List<MaterialLogItemHistory> collection)
        {
            await _historyData.CreateMaterialLogItemsHistoryAsync(collection);
        }

        public async Task<int> GetLatestLogVersion(string logId)
        {
            return await _historyData.QueryLatestLogVersion(logId);
        }

        public async Task<int> GetLatestLogItemsVersion(string logId)
        {
            return await _historyData.QueryLatestLogItemsVersion(logId);
        }

        public async Task<MaterialLogHistory> MapLogHistoryAsync(MaterialLog log)
        {
            //TODO: does this return null or 0? if 0, just add ++ to map, else go do a check.
            var latestVersion = await _historyData.QueryLatestLogVersion(log.LogId);

            return new MaterialLogHistory()
            {
                LogId = log.LogId,
                LogDate = log.LogDate,
                ReturnDate = log.ReturnDate,
                EmployeeId = log.EmployeeId,
                Damaged = log.Damaged,
                EditTimestamp = DateTime.Now,
                Version = latestVersion++
            };
        }

        public async Task<List<MaterialLogItemHistory>> MapLogItemsHistoryAsync(List<MaterialLogItem> items)
        {
            //TODO: does this return null or 0? if 0, just add ++ to map, else go do a check.
            var latestVersion = await _historyData.QueryLatestLogItemsVersion(items[0]?.LogId);

            var mappedItems = new List<MaterialLogItemHistory>();
            foreach (var item in items)
            {
                mappedItems.Add(new MaterialLogItemHistory()
                {
                    LogId = item.LogId,
                    MaterialId = item.MaterialId,
                    MaterialAmount = item.MaterialAmount,
                    Used = item.Used,
                    IsDamaged = item.IsDamaged,
                    DamagedAmount = item.DamagedAmount,
                    RepairAmount = item.RepairAmount,
                    DeleteAmount = item.DeleteAmount,
                    EditTimestamp = DateTime.Now,
                    Version =  latestVersion++
                });
            }

            return mappedItems;
        }

        public MaterialLog MapUpdatedMaterialLog(MaterialLog original, MaterialLog incoming)
        {
            original.LogDate = incoming.LogDate;
            original.ReturnDate = incoming.ReturnDate;
            original.EmployeeId = incoming.EmployeeId;

            return original;
        }

        //TODO: this isnt only a Map anymore. rename to Handle?
        public async Task<List<MaterialLogItem>> MapUpdatedMaterialLogItems(List<MaterialLogItem> originalItems, List<MaterialLogItem> incomingItems, int status)
        {
            var mappedItems = new List<MaterialLogItem>();

            //check if status created or returned
            switch (status)
            {
                case MaterialLogStatusConst.Created:
                    //iterate original
                    foreach (var item in originalItems)
                    {
                        //find match
                        //TODO: is this a good expression?
                        var match = incomingItems.FirstOrDefault(x => x.Id == item.Id && x.LogId == x.LogId);

                        if (match != null)
                        {
                            item.MaterialId = match.MaterialId;
                            item.MaterialAmount = match.MaterialAmount;
                            item.Used = match.Used;

                            //add to updateList
                            mappedItems.Add(item);
                        }
                    }
                    break;

                case MaterialLogStatusConst.Returned:
                    foreach (var item in originalItems)
                    {
                        //find match
                        //TODO: is this a good expression?
                        var match = incomingItems.FirstOrDefault(x => x.Id == item.Id && x.LogId == x.LogId);

                        if (match == null)
                        {
                            throw new ArgumentNullException("No matching item found. Fatal error.");
                        }

                        item.MaterialAmount = match.MaterialAmount;
                        item.Used = match.Used;
                        item.IsDamaged = match.IsDamaged;
                        item.DamagedAmount = match.DamagedAmount;
                        item.RepairAmount = match.RepairAmount;
                        item.DeleteAmount = match.DeleteAmount;

                        //add to updateList
                        mappedItems.Add(item);
                    }
                    
                    break;

                default:
                    throw new Exception($"Status id {status} is invalid.");
            }
            return mappedItems;
        }
    }
}

