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

            var user = await _userManager.MapCurrentUserAsync();

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

        public async Task<List<UserLogViewModel>> GetLogsByControllerAsync(int controllerId)
        {
            return SetViewModel(await _userLogManager.MapLogsByControllerAsync(controllerId));
        }

        public async Task<List<UserLogViewModel>> ArrangeLogsByEntityAsync(string entityId, int controllerId)
        {
            return SetViewModel(await _userLogManager.MapUserLogsByEntityAsync(entityId, controllerId));
        }

        public async Task<List<UserLogViewModel>> ArrangeLogsByUserAsync(int id)
        {
            return SetViewModel(await _userLogManager.MapUserLogsByUserAsync(id));
        }

        public async Task<List<UserLogViewModel>> ArrangeUserLogsAsync()
        {
            return SetViewModel(await _userLogManager.MapUserLogsAsync());
        }

        private List<UserLogViewModel> SetViewModel(List<UserLog> userLogs)
        {
            List<UserLogViewModel> userLogsViewModel = new();

            if (userLogs.Any())
            {
                foreach (var log in userLogs)
                {
                    userLogsViewModel.Add(new UserLogViewModel
                    {
                        UserName = log.User.Name,
                        Action = SetAction(log.Action),
                        ControllerName = SetControllerName(log.ControllerId),
                        EntityId = log.EntityId,
                        LogDate = log.LogDate
                    });
                }
                return userLogsViewModel;
            }
            return userLogsViewModel;
        }

        private string SetAction(int dataAction)
        {
            string action = "";

            switch (dataAction)
            {
                case ActionConst.Create:
                    action = "created";
                    break;
                case ActionConst.Delete:
                    action = "deleted";
                    break;
                case ActionConst.CompleteDamaged:
                    action = "completed damaged";
                    break;
                case ActionConst.ReturnItems:
                    action = "returned items";
                    break;
                case ActionConst.Edit:
                    action = "edited";
                    break;
                case ActionConst.Login:
                    action = "logged in";
                    break;
                case ActionConst.Logout:
                    action = "logged out";
                    break;
                case ActionConst.Undo:
                    action = "did undo";
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Action with Id: {dataAction} does not exist for user logging.");
            }
            return action;
        }

        private string SetControllerName(int controllerId)
        {
            string result = "";
            switch (controllerId)
            {
                case ControllerNames.Material:
                    result = "Material";
                    break;
                case ControllerNames.MaterialLog:
                    result = "Materiallog";
                    break;
                case ControllerNames.Employee:
                    result = "Employee";
                    break;
                case ControllerNames.Firma:
                    result = "Firma";
                    break;
                case ControllerNames.CarPark:
                    result = "Carpark";
                    break;
                case ControllerNames.CarMaintenance:
                    result = "Carmaintenance";
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Controller with Id: {controllerId} does not exist for user logging.");
            }

            return result;
        }
    }
}
