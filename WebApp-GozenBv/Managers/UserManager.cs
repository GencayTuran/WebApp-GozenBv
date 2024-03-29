﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserDataHandler _userData;
        public UserManager(IUserDataHandler userData)
        {
            _userData = userData;
        }
        public async Task<User> GetCurrentUserAsync()
        {
            return await _userData.QueryCurrentUserAsync();
        }

        public async Task<User> MapCurrentUserAsync()
        {
            var user = await GetCurrentUserAsync();
            return user;
        }

        public async Task<int> MapCurrentUserIdAsync(User user)
        {
            return (await GetCurrentUserAsync()).Id;
        }
    }
}
