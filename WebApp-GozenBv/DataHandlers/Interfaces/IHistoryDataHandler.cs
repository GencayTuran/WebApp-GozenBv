using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers.Interfaces
{
    public interface IHistoryDataHandler
    {
        Task<List<MaterialLogHistory>> QueryMaterialLogsHistoryAsync(string logId);
        Task<List<MaterialLogItemHistory>> QueryMaterialLogItemsHistoryAsync(string logId);

        Task CreateMaterialLogHistoryAsync(MaterialLogHistory entity);    
        Task CreateMaterialLogItemsHistoryAsync(List<MaterialLogItemHistory> collection);
        Task<int> QueryLatestLogVersion(string logId);
        Task<int> QueryLatestLogItemsVersion(string logId);
    }
}