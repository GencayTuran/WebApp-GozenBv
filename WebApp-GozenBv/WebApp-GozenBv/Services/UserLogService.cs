using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services
{
    public class UserLogService : IUserLogService
    {
        private readonly DataDbContext _context;

        public UserLogService(DataDbContext context)
        {
            _context = context;
        }

        public void Create(int userId, int controller, int action, string entityId)
        {
            var userLog = new UserLog
            {
                UserId = userId,
                Controller = controller,
                Action = action,
                EntityId = entityId,
                LogDate = DateTime.Now
            };

            _context.Add(userLog);
            _context.SaveChanges();
        }

        public async Task<List<UserLogViewModel>> GetLogs()
        {
            var userLogs = _context.UserLogs
                .Include(u => u.User)
                .ToList();

            List<UserLogViewModel> userlogsViewModel = new();

            var userLogsViewModel = SetViewModel(userLogs);

            return userLogsViewModel;
        }

        public async Task<List<UserLogViewModel>> GetLogsByController(int controller)
        {
            var userLogs = _context.UserLogs
                .Where(x => x.Controller == controller)
                .ToList();

            var userLogsViewModel = SetViewModel(userLogs);

            return userLogsViewModel;
        }

        public async Task<List<UserLogViewModel>> GetLogsByEntity(string entityId, int controller)
        {
            var userLogs = _context.UserLogs
                .Where(x => x.EntityId == entityId)
                .Where(x => x.Controller == controller)
                .ToList();

            var userLogsViewModel = SetViewModel(userLogs);

            return userLogsViewModel;
        }

        public async Task<List<UserLogViewModel>> GetLogsByUser(int userId)
        {
            var userLogs = _context.UserLogs
                .Where(x => x.UserId == userId)
                .ToList();

            var userLogsViewModel = SetViewModel(userLogs);

            return userLogsViewModel;
        }

        private List<UserLogViewModel> SetViewModel(List<UserLog> userLogs)
        {
            List<UserLogViewModel> userLogsViewModel = new();

            foreach (var log in userLogs)
            {
                userLogsViewModel.Add(new UserLogViewModel
                {
                    UserName = log.User.Name,
                    Action = SetAction(log.Action),
                    Controller = SetController(log.Controller),
                    EntityId = log.EntityId,
                    LogDate = log.LogDate
                });
            }

            return userLogsViewModel;
        }

        private string SetAction(int dataAction)
        {
            string action = "";

            switch (dataAction)
            {
                case ActionsConst.Create:
                    action = "created";
                    break;
                case ActionsConst.Delete:
                    action = "deleted";
                    break;
                case ActionsConst.CompleteDamaged:
                    action = "completed damaged";
                    break;
                case ActionsConst.CompleteReturn:
                    action = "completed return";
                    break;
                case ActionsConst.Edit:
                    action = "edited";
                    break;
                case ActionsConst.Login:
                    action = "logged in";
                    break;
                case ActionsConst.Logout:
                    action = "logged out";
                    break;
                case ActionsConst.Undo:
                    action = "did undo";
                    break;
                default:
                    break;
            }
                    return action;
        }

        private string SetController(int dataController)
        {
            string controller = "";
            switch (dataController)
            {
                case ControllerConst.Stock:
                    controller = "Stock";
                    break;
                case ControllerConst.StockLog:
                    controller = "Stocklog";
                    break;
                case ControllerConst.Employee:
                    controller = "Employee";
                    break;
                case ControllerConst.Firma:
                    controller = "Firma";
                    break;
                case ControllerConst.WagenPark:
                    controller = "Wagenpark";
                    break;
                case ControllerConst.WagenMaintenance:
                    controller = "Wagenmaintenance";
                    break;
                default:
                    break;
            }

            return controller;
        }
    }
    
}
