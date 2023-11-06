using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services.Interfaces
{
    public interface IUserLogService
    {
        Task StoreLogAsync(int controller, int action, string entityId);
    }
}
