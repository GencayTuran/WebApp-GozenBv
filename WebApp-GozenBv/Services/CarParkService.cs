using System.Threading.Tasks;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services
{
    public class CarParkService : ICarParkService
    {
        private readonly ICarParkManager _manager;
        public CarParkService(ICarParkManager manager) 
        {
            _manager = manager;
        }

        public async Task HandleCreate(CarCreateViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task HandleDelete(CarCreateViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task HandleEdit(CarEditViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task HandleEdit(CarMaintenancesEditViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
