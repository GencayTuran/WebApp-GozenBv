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
        Task<List<MaterialLog>> GetMaterialLogsAsync();
        Task<MaterialLog> GetMaterialLogAsync(string logId);
        MaterialLog GetMaterialLog(string logId);
        Task<List<MaterialLogItem>> GetMaterialLogItemsAsync(string logId);
        List<MaterialLogItem> GetMaterialLogItems(string logId);
        Task<MaterialLogDTO> GetMaterialLogDTO(string logId);

        Task ManageMaterialLogAsync(MaterialLog log, EntityOperation operation);
        void ManageMaterialLog(MaterialLog log, EntityOperation operation);
        Task ManageMaterialLogItemsAsync(List<MaterialLogItem> items, EntityOperation operation);
        void ManageMaterialLogItems(List<MaterialLogItem> items, EntityOperation operation);
        Task ManageMaterialLogHistoryAsync(LogEditHistory entity);
        Task ManageMaterialLogItemsHistoryAsync(List<ItemEditHistory> collection);
        Task<List<LogEditHistory>> GetLogHistoryByLogId(string logId);
        Task<MaterialLogHistoryDTO> GetHistoryDetails(string logId, int version);
    }
}

