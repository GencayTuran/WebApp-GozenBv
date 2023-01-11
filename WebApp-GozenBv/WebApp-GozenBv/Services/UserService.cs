using System.Linq;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Data;

namespace WebApp_GozenBv.Services
{
    public class UserService : IUserService
    {
        private readonly DataDbContext _context;
        public UserService(DataDbContext context)
        {
            _context = context;
        }
        public int GetCurrentUserId(string userMail, string userName)
        {
            var user = _context.Users.Where(u => u.Email.Contains(userMail)).FirstOrDefault();
            if (user != null)
            {
                return user.Id;
            }
            else
            {
                var newUser = new User
                {
                    Email = userMail,
                    Name = userName
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();

                var userId = _context.Users
                    .Where(u => u.Email.Contains(userMail))
                    .Select(u => u.Id)
                    .FirstOrDefault();

                return userId;
            }

        }
    }
}
