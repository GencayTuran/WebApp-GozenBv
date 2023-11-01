using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Services.Interfaces;
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

        public async Task<IViewComponentResult> InvokeAsync(int view, string entityId, int controllerId)
        {
            List<UserLogViewModel> userLogs = new();

            switch (view)
            {
                case ViewTypeConst.Controller:
                    userLogs = await _userLogService.GetLogsByControllerAsync(controllerId);
                    break;
                case ViewTypeConst.Entity:
                    userLogs = await _userLogService.ArrangeLogsByEntityAsync(entityId, controllerId);
                    break;
                case ViewTypeConst.User:
                    var user = _userService.ArrangeCurrentUserAsync();
                    userLogs = await _userLogService.ArrangeLogsByUserAsync(user.Id);
                    break;
                case ViewTypeConst.All:
                    userLogs = await _userLogService.ArrangeUserLogsAsync();
                    break;
                case ViewTypeConst.Limit:
                    var logs = await _userLogService.ArrangeUserLogsAsync();
                    userLogs.AddRange(logs.Take(20));
                    break;
                default:
                    break;
            }

            return View(userLogs);
        }
    }
}
