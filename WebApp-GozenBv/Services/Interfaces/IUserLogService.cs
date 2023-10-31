using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services.Interfaces
{
    public interface IUserLogService
    {
        Task CreateAsync(int controller, int action, string entityId);
        Task<List<UserLogViewModel>> GetLogs(); //all logs
        Task<List<UserLogViewModel>> GetLogsByEntity(string entityId, int controller); //per entity per controller (details)
        Task<List<UserLogViewModel>> GetLogsByController(int controller); //per index
        Task<List<UserLogViewModel>> GetLogsByUser(int userId); //per user

    }
}
