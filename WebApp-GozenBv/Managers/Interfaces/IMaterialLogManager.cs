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
		Task<MaterialLogDetailViewModel> MapMaterialLogDetails(string logCode);

        Task<List<MaterialLogItem>> MapItemsByLogId(int? logId);
        //Task<List<MaterialLogItem>> MapDamagedItemsByLogId(int? logId);
        Task<MaterialLogItem> MapMaterialLogItem(string logCode);

		Task<string> MapIncomingLog(MaterialLogCreateViewModel incomingViewModel);

		Task ManageMaterialLog(MaterialLog log, EntityOperation operation);
        Task ManageMaterialLogItems(List<MaterialLogItem> items, EntityOperation operation);
	}
}

