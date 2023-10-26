using System.Threading.Tasks;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Helpers.Interfaces
{
    public interface IMaterialLogHelper
    {
        Task HandleEdit(MaterialLogDetailViewModel incomingLog);
    }
}
