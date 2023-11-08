using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers.Interfaces
{
	public interface IMaterialLogManager
	{
        Task<List<MaterialLog>> GetMaterialLogs();
        Task<MaterialLog> GetMaterialLogAsync(string logId);
        MaterialLog GetMaterialLog(string logId);
        Task<List<MaterialLogItem>> GetMaterialLogItemsAsync(string logId);
        List<MaterialLogItem> GetMaterialLogItems(string logId);
		Task<MaterialLogDetailViewModel> GetMaterialLogDetails(string logId);
        Task<MaterialLogDTO> GetMaterialLogDTO(string logId);

        Task ManageMaterialLogAsync(MaterialLog log, EntityOperation operation);
        void ManageMaterialLog(MaterialLog log, EntityOperation operation);
        Task ManageMaterialLogItemsAsync(List<MaterialLogItem> items, EntityOperation operation);
        void ManageMaterialLogItems(List<MaterialLogItem> items, EntityOperation operation);
        Task ManageMaterialLogHistoryAsync(LogEditHistory entity);
        Task ManageMaterialLogItemsHistoryAsync(List<ItemEditHistory> collection);

        Task<int> GetLatestLogVersion(string logId);
        Task<int> GetLatestLogItemsVersion(string logId);

        Task<LogEditHistory> MapLogHistoryAsync(MaterialLog log);
        Task<List<ItemEditHistory>> MapLogItemsHistoryAsync(List<MaterialLogItem> items);
        MaterialLog MapUpdatedMaterialLog(MaterialLog original, MaterialLog incoming);
        List<MaterialLogItem> ReplaceItems_StatusCreated(List<MaterialLogItem> originalItems, List<MaterialLogItem> incomingItems);
        List<MaterialLogItem> MapUpdatedItems_StatusReturned(List<MaterialLogItem> originalLogItems, List<MaterialLogItem> incomingLogItems);
        List<MaterialLogItem> MapSelectedItems(List<MaterialLogSelectedItemViewModel> selectedItems);
        List<MaterialLogItem> MapNewItems(List<MaterialLogItem> incomingItems, string logId);
    }
}

