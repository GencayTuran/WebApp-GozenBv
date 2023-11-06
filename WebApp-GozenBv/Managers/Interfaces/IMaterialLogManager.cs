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

        MaterialLog MapMaterialLogStatusCreated(MaterialLog original, MaterialLog incoming);
        MaterialLogItem MapMaterialLogItemStatusCreated(MaterialLogItem incoming);
        MaterialLogItem MapMaterialLogItemStatusReturned(MaterialLogItem incoming);
        MaterialLogItem MapMaterialLogItemStatusReturnedDamaged(MaterialLogItem incoming);

        public MaterialLog MapReturnedLog(MaterialLog log);
        public MaterialLogItem MapReturnedItem(MaterialLogItem item);
    }
}

