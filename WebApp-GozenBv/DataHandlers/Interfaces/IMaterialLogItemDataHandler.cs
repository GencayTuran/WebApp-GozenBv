using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
	public interface IMaterialLogItemDataHandler
	{
        Task<List<MaterialLogItem>> GetItemsByLogIdAsync(string logId);
        List<MaterialLogItem> GetItemsByLogId(string logId);
        Task<List<MaterialLogItem>> GetItemsByLogId(string logId, Expression<Func<MaterialLogItem, bool>> filter);
        Task<List<MaterialLogItem>> GetDamagedItemsByLogId(string logId);
        Task<List<MaterialLogItem>> GetUnDamagedItemsByLogId(string logId);
        Task CreateItemsAsync(List<MaterialLogItem> items);
        Task UpdateItemsAsync(List<MaterialLogItem> items);
        Task DeleteItemsAsync(List<MaterialLogItem> items);
        void CreateItems(List<MaterialLogItem> items);
        void UpdateItems(List<MaterialLogItem> items);
        void DeleteItems(List<MaterialLogItem> items);
    }
}

