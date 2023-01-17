using System.Threading.Tasks;

namespace WebApp_GozenBv.Services
{
    public interface IUserService
    {
        int GetCurrentUserId(Microsoft.Graph.User mgUser);
        Task<Microsoft.Graph.User> GetCurrentUser();
    }
}
