using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
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
		Task<MaterialLogDetailViewModel> GetMaterialLogDetails(string logCode);

        Task ManageMaterialLogAsync(MaterialLog log, EntityOperation operation);
        void ManageMaterialLog(MaterialLog log, EntityOperation operation);
        Task ManageMaterialLogItemsAsync(List<MaterialLogItem> items, EntityOperation operation);
        void ManageMaterialLogItems(List<MaterialLogItem> items, EntityOperation operation);
        Task ManageMaterialLogHistoryAsync(MaterialLogHistory entity);
        Task ManageMaterialLogItemsHistoryAsync(List<MaterialLogItemHistory> collection);


        //
        //MaterialLogItem MapMaterialLogItemStatusCreated(MaterialLogItem incoming);
        //MaterialLogItem MapMaterialLogItemStatusReturned(MaterialLogItem incoming);
        //MaterialLogItem MapMaterialLogItemStatusReturnedDamaged(MaterialLogItem incoming);

        //public MaterialLog MapReturnedLog(MaterialLog log);
        //public MaterialLogItem MapReturnedItem(MaterialLogItem item);
        //

        Task<int> GetLatestLogVersion(string logId);
        Task<int> GetLatestLogItemsVersion(string logId);

        Task<MaterialLogHistory> MapLogHistoryAsync(MaterialLog log);
        Task<List<MaterialLogItemHistory>> MapLogItemsHistoryAsync(List<MaterialLogItem> items);
        MaterialLog MapUpdatedMaterialLog(MaterialLog original, MaterialLog incoming);
        Task<List<MaterialLogItem>> MapUpdatedMaterialLogItems(List<MaterialLogItem> originalItems, List<MaterialLogItem> incomingItems, int status);
    }
}

