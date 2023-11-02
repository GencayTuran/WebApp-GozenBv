using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.DataHandlers
{
    public class UserLogDataHandler : IUserLogDataHandler
    {
        private readonly DataDbContext _context;
        public UserLogDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task CreateUserLogAsync(UserLog userLog)
        {
            await _context.AddAsync(userLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserLog>> GetLogsByControllerIdAsync(int controllerId)
        {
            return await _context.UserLogs
                .Include(u => u.User)
                .Where(x => x.ControllerId == controllerId)
                .OrderByDescending(x => x.LogDate)
                .ToListAsync();
        }

        public Task<List<UserLog>> GetLogsByEntityIdAsync(string entityId, int controllerId)
        {
            return _context.UserLogs
                .Include(u => u.User)
                .Where(x => x.EntityId == entityId)
                .Where(x => x.ControllerId == controllerId)
                .OrderByDescending(x => x.LogDate)
                .ToListAsync();
        }

        public async Task<List<UserLog>> GetUserLogsAsync()
        {
            return await _context.UserLogs
                .Include(u => u.User)
                .OrderByDescending(x => x.LogDate)
                .ToListAsync();
        }

        public async Task<List<UserLog>> GetUserLogsByIdAsync(int id)
        {
            return await _context.UserLogs
                        .Include(u => u.User)
                            .Where(x => x.UserId == id)
                            .OrderByDescending(x => x.LogDate)
                            .ToListAsync();
        }
    }
}

