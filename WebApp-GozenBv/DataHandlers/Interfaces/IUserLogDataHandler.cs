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
        Task<List<UserLog>> GetLogsByControllerIdAsync(int controllerId);
        Task<List<UserLog>> GetLogsByEntityIdAsync(string entityId, int controllerId);
        Task<List<UserLog>> GetUserLogsAsync();
        Task<List<UserLog>> GetUserLogsByIdAsync(int id);
    }
}

