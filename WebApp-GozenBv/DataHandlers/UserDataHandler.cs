using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
	public class UserDataHandler : IUserDataHandler
	{
        private readonly DataDbContext _context;
        public UserDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task<User> QueryCurrentUserAsync()
        {
            //TODO change to current user instead of fix email
            return await _context.Users
            .Where(u => u.Email == "gencay.turan@hotmail.com")
            .FirstOrDefaultAsync();
        }
    }
}

