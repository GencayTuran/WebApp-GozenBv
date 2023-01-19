using System.Linq;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Data;
using Microsoft.Graph;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Services
{
    public class UserService : IUserService
    {
        private readonly DataDbContext _context;
        //private readonly GraphServiceClient _graphServiceClient;
        //public UserService(DataDbContext context, GraphServiceClient graphServiceClient)
        //{
        //    _context = context;
        //    _graphServiceClient = graphServiceClient;
        //}

        public UserService(DataDbContext context)
        {
            _context = context;
        }

        //public async Task<Microsoft.Graph.User> GetCurrentUser()
        //{
        //    var user = await _graphServiceClient.Me.Request().GetAsync();
        //    return user;
        //}

        //public int GetCurrentUserId(Microsoft.Graph.User mgUser)
        //{
        //    var user = _context.Users.Where(u => u.Email.Contains(mgUser.Mail)).FirstOrDefault();
        //    if (user != null)
        //    {
        //        return user.Id;
        //    }
        //    else
        //    {
        //        var newUser = new Models.User
        //        {
        //            Email = mgUser.Mail,
        //            Name = mgUser.DisplayName
        //        };
        //        _context.Users.Add(newUser);
        //        _context.SaveChanges();

        //        var userId = _context.Users
        //            .Where(u => u.Email.Contains(mgUser.Mail))
        //            .Select(u => u.Id)
        //            .FirstOrDefault();

        //        return userId;
        //    }

        public async Task<Models.User> GetCurrentUser()
        {
            var user = _context.Users
            .Where(u => u.Email == "gencay.turan@hotmail.com")
            .FirstOrDefault();
            return user;
        }

        public int GetCurrentUserId(Models.User user)
        {
            var userId = _context.Users
                .Where(u => u.Email.Contains(user.Email))
                .Select(u => u.Id)
                .FirstOrDefault();

            return userId;
        }

    }
}

