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
using WebApp_GozenBv.DTOs;
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

        //TODO: check if still needed. DTOs will replace backend ViewModels.
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
        public async Task<MaterialLogDTO> GetMaterialLogDTO(string logId)
        {
            var log = await _logData.QueryMaterialLogByLogIdAsync(logId);

            if (log == null)
            {
                throw new ArgumentNullException($"MaterialLog with logId {logId} does not exist.");
            }

            var items = await _itemData.QueryItemsByLogIdAsync(logId);

            return new MaterialLogDTO
            {
                MaterialLog = log,
                MaterialLogItems = items
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
                    MaterialLogItemId = item.Id,
                    LogId = item.LogId,
                    MaterialId = item.MaterialId,
                    Material = item.Material,
                    MaterialAmount = item.MaterialAmount,
                    Used = item.Used,
                    IsDamaged = item.IsDamaged,
                    DamagedAmount = item.DamagedAmount,
                    RepairAmount = item.RepairAmount,
                    DeleteAmount = item.DeleteAmount,
                    EditTimestamp = DateTime.Now,
                    Version = latestVersion++
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
        public List<MaterialLogItem> MapUpdatedItems_StatusCreated(List<MaterialLogItem> originalItems, List<MaterialLogItem> incomingItems)
        {
            var mappedItems = new List<MaterialLogItem>();

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

            return mappedItems;
        }

        public List<MaterialLogItem> MapUpdatedItems_StatusReturned(List<MaterialLogItem> originalLogItems, List<MaterialLogItem> incomingLogItems)
        {
            var mappedItems = new List<MaterialLogItem>();

            foreach (var item in originalLogItems)
            {
                //find match
                //TODO: is this a good expression?
                var match = originalLogItems.FirstOrDefault(x => x.Id == item.Id && x.LogId == x.LogId);

                if (match == null)
                {
                    throw new ArgumentNullException("No matching item found. Fatal error.");
                }

                //TODO: dont forget in the view to set the amounts to null when item turns ot not damaged. Proper reset!
                item.IsDamaged = match.IsDamaged;
                item.DamagedAmount = match.DamagedAmount;
                item.RepairAmount = match.RepairAmount;
                item.DeleteAmount = match.DeleteAmount;

                //add to updateList
                mappedItems.Add(item);
            }
            return mappedItems;
        }
    }
}

