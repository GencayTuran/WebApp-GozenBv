using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services.Interfaces
{
    public interface ICarParkService
    {
        Task HandleCreate(CarCreateViewModel viewModel);
        Task HandleEdit(CarEditViewModel viewModel);
        Task HandleEdit(CarMaintenancesEditViewModel viewModel);
        Task HandleDelete(CarCreateViewModel viewModel);
    }
}