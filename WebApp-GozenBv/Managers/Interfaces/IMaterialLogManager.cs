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
		Task<List<MaterialLog>> MapMaterialLogs();
        Task<MaterialLog> MapMaterialLogAsync(string logId);
        MaterialLog MapMaterialLog(string logId);
        Task<List<MaterialLogItem>> MapMaterialLogItemsAsync(string logId);
        List<MaterialLogItem> MapMaterialLogItems(string logId);
		Task<MaterialLogDetailViewModel> MapMaterialLogDetails(string logCode);

        Task ManageMaterialLogAsync(MaterialLog log, EntityOperation operation);
        void ManageMaterialLog(MaterialLog log, EntityOperation operation);
        Task ManageMaterialLogItemsAsync(List<MaterialLogItem> items, EntityOperation operation);
        void ManageMaterialLogItems(List<MaterialLogItem> items, EntityOperation operation);
	}
}

