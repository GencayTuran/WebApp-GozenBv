using System.Threading.Tasks;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services.Interfaces
{
    public interface IMaterialLogService
    {
        Task<string> HandleCreate(MaterialLogCreateViewModel incomingViewModel);
        Task HandleEdit(string logId, MaterialLogAndItemsViewModel incomingEdit);
        Task HandleReturn(MaterialLogAndItemsViewModel incomingReturn);

        Task HandleDelete(string logId);
        Task DeleteCreate(MaterialLogDTO materialLogDTO);

        Task HandleApprove(string logId);
        Task ApproveCreate(MaterialLogDTO materialLogDTO);
        Task ApproveReturn(MaterialLogDTO materialLogDTO);
        
    }
}
