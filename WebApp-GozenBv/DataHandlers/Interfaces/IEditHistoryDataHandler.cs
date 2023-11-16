using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers.Interfaces
{
    public interface IEditHistoryDataHandler
    {
        Task<List<LogEditHistory>> QueryMaterialLogsHistoryAsync(string logId);

        Task CreateMaterialLogHistoryAsync(LogEditHistory entity);    
        Task CreateMaterialLogItemsHistoryAsync(List<ItemEditHistory> collection);

        Task<LogEditHistory> QueryLatestLog(string logId);
        Task<ItemEditHistory> QueryLatestLogItem(string logId);

        Task<LogEditHistory> QueryLogHistoryByVersion(string logId, int version);
        Task<List<ItemEditHistory>> QueryLogItemsHistoryByVersion(string logId, int version);
    }
}