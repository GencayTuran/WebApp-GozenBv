using Humanizer;
using Microsoft.AspNetCore.JsonPatch.Internal;
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
        private readonly IEditHistoryDataHandler _historyData;

        public MaterialLogManager(
            IMaterialLogDataHandler logData,
            IMaterialLogItemDataHandler itemData,
            IEditHistoryDataHandler historyData)
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
        public async Task<List<MaterialLog>> GetMaterialLogsAsync()
        {
            return await _logData.QueryMaterialLogsAsync();
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

        public async Task ManageMaterialLogHistoryAsync(LogEditHistory entity)
        {
            await _historyData.CreateMaterialLogHistoryAsync(entity);
        }

        public async Task ManageMaterialLogItemsHistoryAsync(List<ItemEditHistory> collection)
        {
            await _historyData.CreateMaterialLogItemsHistoryAsync(collection);
        }

        public async Task<List<LogEditHistory>> GetLogHistoryByLogId(string logId)
        {
            return await _historyData.QueryMaterialLogsHistoryAsync(logId);
        }

        public async Task<MaterialLogHistoryDTO> GetHistoryDetails(string logId, int version)
        {
            var logHistory = await _historyData.QueryLogHistoryByVersion(logId, version);
            var itemsHistory = await _historyData.QueryLogItemsHistoryByVersion(logId, version);

            return new MaterialLogHistoryDTO()
            {
                LogEditHistory = logHistory,
                ItemsEditHistory = itemsHistory
            };
        }
    }
}

