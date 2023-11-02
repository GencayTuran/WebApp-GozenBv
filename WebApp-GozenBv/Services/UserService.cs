using System.Linq;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Data;
using Microsoft.Graph;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;

namespace WebApp_GozenBv.Services
{
    public class UserService : IUserService
    {
        private readonly IUserManager _manager;
        //private readonly GraphServiceClient _graphServiceClient;

        public UserService(IUserManager manager) //, GraphServiceClient graphServiceClient
        {
            _manager = manager;
            //_graphServiceClient = graphServiceClient;
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

        public async Task<Models.User> ArrangeCurrentUserAsync()
        {
            var user = await _manager.MapCurrentUserAsync();
            return user;
        }

        public async Task<int> ArrangeCurrentUserIdAsync(Models.User user)
        {
            return (await _manager.MapCurrentUserAsync()).Id;
        }
    }
}

