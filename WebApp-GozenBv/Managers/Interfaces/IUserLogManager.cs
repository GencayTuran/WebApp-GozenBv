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
        Task<List<UserLog>> MapLogsByControllerAsync(int controllerId);
        Task<List<UserLog>> MapUserLogsByEntityAsync(string entityId, int controllerId);
        Task<List<UserLog>> MapUserLogsByUserAsync(int id);
        Task<List<UserLog>> MapUserLogsAsync();
    }
}