using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Services;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.ViewComponents
{
    public class UserLogViewComponent : ViewComponent
    {
        private readonly IUserLogService _userLogService;
        private readonly IUserService _userService;
        public UserLogViewComponent(IUserLogService userLogService, IUserService userService)
        {
            _userLogService = userLogService;
            _userService= userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int view, string entityId, int controller)
        {
            List<UserLogViewModel> userLogs = new();

            switch (view)
            {
                case ViewTypeConst.Controller:
                    userLogs = await _userLogService.GetLogsByController(controller);
                    break;
                case ViewTypeConst.Entity:
                    userLogs = await _userLogService.GetLogsByEntity(entityId, controller);
                    break;
                case ViewTypeConst.User:
                    var user = _userService.GetCurrentUser();
                    userLogs = await _userLogService.GetLogsByUser(user.Id);
                    break;
                case ViewTypeConst.All:
                    userLogs = await _userLogService.GetLogs();
                    break;
                default:
                    break;
            }

            return View(userLogs);
        }
    }
}
