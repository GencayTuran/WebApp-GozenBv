using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
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

        public async Task<List<UserLog>> GetLogsByControllerAsync(int controllerId)
        {
            return await _userLogData.QueryLogsByControllerIdAsync(controllerId);
        }

        public async Task<List<UserLog>> GetUserLogsAsync()
        {
            return await _userLogData.QueryUserLogsAsync();
        }

        public async Task<List<UserLog>> GetUserLogsByEntityAsync(string entityId, int controllerId)
        {
            return await _userLogData.QueryLogsByEntityIdAsync(entityId, controllerId);
        }

        public async Task<List<UserLog>> GetUserLogsByUserAsync(int id)
        {
            return await _userLogData.QueryUserLogsByIdAsync(id);
        }

        public async Task<List<UserLogViewModel>> MapLogsByControllerAsync(int controllerId)
        {
            return SetViewModel(await GetLogsByControllerAsync(controllerId));
        }

        public async Task<List<UserLogViewModel>> MapLogsByEntityAsync(string entityId, int controllerId)
        {
            return SetViewModel(await GetUserLogsByEntityAsync(entityId, controllerId));
        }

        public async Task<List<UserLogViewModel>> MapLogsByUserAsync(int id)
        {
            return SetViewModel(await GetUserLogsByUserAsync(id));
        }

        public async Task<List<UserLogViewModel>> MapUserLogsAsync()
        {
            return SetViewModel(await GetUserLogsAsync());
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
