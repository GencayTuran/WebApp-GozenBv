using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers
{
    public class UserLogManager : IUserLogManager
    {
        private readonly IUserLogDataHandler _userLogData;

        public UserLogManager(IUserLogDataHandler dataHandler)
        {
            _userLogData = dataHandler;
        }

        public async Task ManageUserLog(UserLog userLog)
        {
            await _userLogData.CreateUserLogAsync(userLog);
        }

        public async Task<List<UserLog>> MapLogsByControllerAsync(int controllerId)
        {
            return await _userLogData.GetLogsByControllerIdAsync(controllerId);
        }

        public async Task<List<UserLog>> MapUserLogsAsync()
        {
            return await _userLogData.GetUserLogsAsync();
        }

        public async Task<List<UserLog>> MapUserLogsByEntityAsync(string entityId, int controllerId)
        {
            return await _userLogData.GetLogsByEntityIdAsync(entityId, controllerId);
        }

        public async Task<List<UserLog>> MapUserLogsByUserAsync(int id)
        {
            return await _userLogData.GetUserLogsByIdAsync(id);
        }
    }
}
