using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.DataHandlers
{
    public interface IUserLogDataHandler
    {
        Task CreateUserLogAsync(UserLog userLog);
        Task<List<UserLog>> QueryLogsByControllerIdAsync(int controllerId);
        Task<List<UserLog>> QueryLogsByEntityIdAsync(string entityId, int controllerId);
        Task<List<UserLog>> QueryUserLogsAsync();
        Task<List<UserLog>> QueryUserLogsByIdAsync(int id);
    }
}

