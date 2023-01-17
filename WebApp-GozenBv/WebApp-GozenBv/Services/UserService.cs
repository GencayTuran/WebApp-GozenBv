using System.Linq;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Data;
using Microsoft.Graph;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WebApp_GozenBv.Services
{
    public class UserService : IUserService
    {
        private readonly DataDbContext _context;
        private readonly GraphServiceClient _graphServiceClient;
        public UserService(DataDbContext context, GraphServiceClient graphServiceClient)
        {
            _context = context;
            _graphServiceClient = graphServiceClient;
        }

        public async Task<Microsoft.Graph.User> GetCurrentUser()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();
            return user;
        }

        public int GetCurrentUserId(Microsoft.Graph.User mgUser)
        {
            var user = _context.Users.Where(u => u.Email.Contains(mgUser.Mail)).FirstOrDefault();
            if (user != null)
            {
                return user.Id;
            }
            else
            {
                var userId = SaveNewUser(mgUser);

                return userId;
            }

        }

        //test purpose (without azure ad)
        public async Task<Models.User> GetUserFromSeed()
        {
            var user = _context.Users
                .Where(u => u.Email == "gencay.turan@hotmail.com")
                .FirstOrDefault();
            return user;
        }

        private int SaveNewUser(Microsoft.Graph.User mgUser)
        {

            var newUser = new Models.User
            {
                Email = mgUser.Mail,
                Name = mgUser.DisplayName
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            var userId = _context.Users
                .Where(u => u.Email.Contains(mgUser.Mail))
                .Select(u => u.Id)
                .FirstOrDefault();

            return userId;
        }
    }
}
