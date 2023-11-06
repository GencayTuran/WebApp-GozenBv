using System;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public interface IUserDataHandler
    {
        Task<User> QueryCurrentUserAsync();
    }
}

