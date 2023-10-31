using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Services.Interfaces
{
    public interface IUserService
    {
        //int GetCurrentUserId(Microsoft.Graph.User mgUser);
        //Task<Microsoft.Graph.User> GetCurrentUser();

        int GetCurrentUserId(User user);
        Task<User> GetCurrentUser();
    }
}
