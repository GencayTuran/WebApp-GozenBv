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
		Task<List<MaterialLog>> GetMaterialLogs();
		Task<List<MaterialLog>> GetMaterialLogsAsync(Expression<Func<MaterialLog, bool>> filter);
		List<MaterialLog> GetMaterialLogs(Expression<Func<MaterialLog, bool>> filter);
		Task<MaterialLog> GetMaterialLogByLogIdAsync(string logId);
		MaterialLog GetMaterialLogByLogId(string logId);

        Task CreateMaterialLogAsync(MaterialLog log);
        void CreateMaterialLog(MaterialLog log);
        Task UpdateMaterialLogAsync(MaterialLog log);
        void UpdateMaterialLog(MaterialLog log);
		Task DeleteMaterialLogAsync(MaterialLog log);
		void DeleteMaterialLog(MaterialLog log);
	}
}

