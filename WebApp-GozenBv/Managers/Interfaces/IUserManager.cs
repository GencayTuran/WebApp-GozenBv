﻿using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<User> MapCurrentUserAsync();
    }
}