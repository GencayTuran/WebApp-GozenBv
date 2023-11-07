using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers.Interfaces
{
    public interface IEditHistoryDataHandler
    {
        Task<List<LogEditHistory>> QueryMaterialLogsHistoryAsync(string logId);
        Task<List<ItemEditHistory>> QueryMaterialLogItemsHistoryAsync(string logId);

        Task CreateMaterialLogHistoryAsync(LogEditHistory entity);    
        Task CreateMaterialLogItemsHistoryAsync(List<ItemEditHistory> collection);
        Task<int> QueryLatestLogVersion(string logId);
        Task<int> QueryLatestLogItemsVersion(string logId);
    }
}