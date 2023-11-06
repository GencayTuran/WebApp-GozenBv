using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers.Interfaces
{
    public interface IUserLogManager
    {
        Task ManageUserLog(UserLog userLog);
        Task<List<UserLog>> GetLogsByControllerAsync(int controllerId);
        Task<List<UserLog>> GetUserLogsByEntityAsync(string entityId, int controllerId);
        Task<List<UserLog>> GetUserLogsByUserAsync(int id);
        Task<List<UserLog>> GetUserLogsAsync();
        Task<List<UserLogViewModel>> MapLogsByControllerAsync(int controllerId);
        Task<List<UserLogViewModel>> MapLogsByEntityAsync(string entityId, int controllerId);
        Task<List<UserLogViewModel>> MapLogsByUserAsync(int id);
        Task<List<UserLogViewModel>> MapUserLogsAsync();
    }
}