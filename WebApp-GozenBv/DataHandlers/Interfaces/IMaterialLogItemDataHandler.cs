using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
	public interface IMaterialLogItemDataHandler
	{
        Task<List<MaterialLogItem>> GetItemsByLogCode(string logCode);
        Task<List<MaterialLogItem>> GetItemsByLogCode(string logCode, Expression<Func<MaterialLogItem, bool>> filter);
        Task<MaterialLogItem> GetMaterialLogById(int? id);
        Task CreateItems(List<MaterialLogItem> items);
        Task UpdateItems(List<MaterialLogItem> items);
        Task DeleteItems(List<MaterialLogItem> items);
    }
}

