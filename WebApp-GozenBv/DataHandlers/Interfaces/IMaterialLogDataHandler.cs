using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
	public interface IMaterialLogDataHandler
	{
		Task<List<MaterialLog>> QueryMaterialLogs();
		Task<List<MaterialLog>> QueryMaterialLogsAsync();
		Task<MaterialLog> QueryMaterialLogByLogIdAsync(string logId);
		MaterialLog QueryMaterialLogByLogId(string logId);

        Task CreateMaterialLogAsync(MaterialLog log);
        void CreateMaterialLog(MaterialLog log);
        Task UpdateMaterialLogAsync(MaterialLog log);
        void UpdateMaterialLog(MaterialLog log);
		Task DeleteMaterialLogAsync(MaterialLog log);
		void DeleteMaterialLog(MaterialLog log);
	}
}

