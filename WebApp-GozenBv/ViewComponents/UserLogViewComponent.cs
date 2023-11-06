using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.ViewComponents
{
    public class UserLogViewComponent : ViewComponent
    {
        private readonly IUserLogManager _userLogManager;
        private readonly IUserManager _userManager;
        public UserLogViewComponent(IUserLogManager userLogManager, IUserManager userManager)
        {
            _userLogManager = userLogManager;
            _userManager= userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int view, string entityId, int controllerId)
        {
            List<UserLogViewModel> userLogs = new();

            switch (view)
            {
                case ViewTypeConst.Controller:
                    userLogs = await _userLogManager.MapLogsByControllerAsync(controllerId);
                    break;
                case ViewTypeConst.Entity:
                    userLogs = await _userLogManager.MapLogsByEntityAsync(entityId, controllerId);
                    break;
                case ViewTypeConst.User:
                    var user = _userManager.MapCurrentUserAsync();
                    userLogs = await _userLogManager.MapLogsByUserAsync(user.Id);
                    break;
                case ViewTypeConst.All:
                    userLogs = await _userLogManager.MapUserLogsAsync();
                    break;
                case ViewTypeConst.Limit:
                    var logs = await _userLogManager.MapUserLogsAsync();
                    userLogs.AddRange(logs.Take(20));
                    break;
                default:
                    break;
            }

            return View(userLogs);
        }
    }
}
