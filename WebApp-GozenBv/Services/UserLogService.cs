using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
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
        private readonly IUserService _userService;

        public UserLogService(DataDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        public async Task CreateAsync(int controller, int action, string entityId)
        {
            //var user = await _userService.GetCurrentUser();
            //var userId = _userService.GetCurrentUserId(user);

            var user = await _userService.GetCurrentUser();

            var userLog = new UserLog
            {
                UserId = user.Id,
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
                .OrderByDescending(x => x.LogDate)
                .ToList();

            List<UserLogViewModel> userlogsViewModel = new();

            var userLogsViewModel = SetViewModel(userLogs);

            return userLogsViewModel;
        }

        public async Task<List<UserLogViewModel>> GetLogsByController(int controller)
        {
            var userLogs = _context.UserLogs
                .Include(u => u.User)
                .Where(x => x.Controller == controller)
                .OrderByDescending(x => x.LogDate)
                .ToList();

            var userLogsViewModel = SetViewModel(userLogs);

            return userLogsViewModel;
        }

        public async Task<List<UserLogViewModel>> GetLogsByEntity(string entityId, int controller)
        {
            var userLogs = _context.UserLogs
                .Include(u => u.User)
                .Where(x => x.EntityId == entityId)
                .Where(x => x.Controller == controller)
                .OrderByDescending(x => x.LogDate)
                .ToList();

            var userLogsViewModel = SetViewModel(userLogs);

            return userLogsViewModel;
        }

        public async Task<List<UserLogViewModel>> GetLogsByUser(int userId)
        {
            var userLogs = _context.UserLogs
                .Include(u => u.User)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LogDate)
                .ToList();

            var userLogsViewModel = SetViewModel(userLogs);

            return userLogsViewModel;
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
                        Controller = SetController(log.Controller),
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
                    break;
            }
            return action;
        }

        private string SetController(int dataController)
        {
            string controller = "";
            switch (dataController)
            {
                case ControllerConst.Material:
                    controller = "Material";
                    break;
                case ControllerConst.MaterialLog:
                    controller = "Materiallog";
                    break;
                case ControllerConst.Employee:
                    controller = "Employee";
                    break;
                case ControllerConst.Firma:
                    controller = "Firma";
                    break;
                case ControllerConst.CarPark:
                    controller = "Carpark";
                    break;
                case ControllerConst.CarMaintenance:
                    controller = "Carmaintenance";
                    break;
                default:
                    break;
            }

            return controller;
        }
    }

}
