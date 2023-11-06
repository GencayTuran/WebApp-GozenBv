using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services.Interfaces
{
    public interface IMaterialLogService
    {
        Task<string> HandleCreate(MaterialLogCreateViewModel incomingViewModel);
        Task HandleEdit(MaterialLogDetailViewModel incomingLog);
        void HandleReturn(MaterialLogDetailViewModel incomingReturn);
        Task HandleDamaged(MaterialLogDetailViewModel incomingComplete);

        Task HandleDelete(string logId);
        Task HandleApprove(string logId);
    }
}
