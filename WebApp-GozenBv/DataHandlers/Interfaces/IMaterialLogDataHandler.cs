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
		Task<List<MaterialLog>> GetMaterialLogs(Expression<Func<MaterialLog, bool>> filter);
		Task<MaterialLog> GetMaterialLogById(int? id);
        Task CreateMaterialLog(MaterialLog log);
        Task UpdateMaterialLog(MaterialLog log);
		Task DeleteMaterialLog(MaterialLog log);
	}
}

