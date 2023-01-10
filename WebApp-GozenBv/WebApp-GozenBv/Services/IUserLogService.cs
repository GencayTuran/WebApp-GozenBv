using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services
{
    public interface IUserLogService
    {
        void Create(int userId, int controller, int action, string entityId);
        Task<List<UserLogViewModel>> GetLogs(); //all logs
        Task<List<UserLogViewModel>> GetLogsByEntity(string entityId); //per entity (details)
        Task<List<UserLogViewModel>> GetLogsByController(int controller); //per index
        Task<List<UserLogViewModel>> GetLogsByUser(int userId); //per user
        
    }
}
