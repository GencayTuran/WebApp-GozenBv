using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.ViewModels;


namespace WebApp_GozenBv.Services
{
    public class UserLogService : IUserLogService
    {
        private readonly IUserManager _userManager;
        private readonly IUserLogManager _userLogManager;

        public UserLogService(
            IUserManager userManager,
            IUserLogManager userLogManager)
        {
            _userLogManager = userLogManager;
            _userManager = userManager;
        }

        public async Task StoreLogAsync(int controller, int action, string entityId)
        {
            //var user = await _userManager.GetCurrentUser();
            //var userId = _userManager.GetCurrentUserId(user);

            var user = await _userManager.GetCurrentUserAsync();

            var userLog = new UserLog
            {
                UserId = user.Id,
                ControllerId = controller,
                Action = action,
                EntityId = entityId,
                LogDate = DateTime.Now
            };

            await _userLogManager.ManageUserLog(userLog);
        }
    }
}
