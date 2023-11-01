using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services.Interfaces
{
    public interface IUserLogService
    {
        Task StoreLogAsync(int controller, int action, string entityId);
        Task<List<UserLogViewModel>> ArrangeUserLogsAsync(); //all logs
        Task<List<UserLogViewModel>> ArrangeLogsByEntityAsync(string entityId, int controller); //per entity per controller (details)
        Task<List<UserLogViewModel>> GetLogsByControllerAsync(int controller); //per index
        Task<List<UserLogViewModel>> ArrangeLogsByUserAsync(int userId); //per user

    }
}
