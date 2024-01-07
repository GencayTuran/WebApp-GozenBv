using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Mappers;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services
{
    public class CarParkService : ICarParkService
    {
        private readonly ICarParkManager _manager;
        private readonly ICarParkMapper _mapper;

        public CarParkService(ICarParkManager manager, ICarParkMapper mapper) 
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task HandleCreate(CarCreateViewModel viewModel)
        {
            await _manager.ManageCar(viewModel.Car, EntityOperation.Create);

            if (!viewModel.CarMaintenances.IsNullOrEmpty())
            {
                var mappedMaintenances = _mapper.MapViewModelToModel(viewModel);
                await _manager.ManageCarMaintenances(mappedMaintenances, EntityOperation.Create);
            }
        }

        public async Task HandleEdit(CarPark car)
        {
            await _manager.ManageCar(car, EntityOperation.Update);
        }

        public async Task HandleEdit(CarMaintenancesEditViewModel viewModel)
        {
            //TODO: check if new maintenances were added
            //TODO: replace original (unfinished) maintenances with incoming maintenances. => merge new added maintenances with incoming here.
            if (viewModel.CarMaintenances != null)
            {
                await _manager.ManageCarMaintenances(viewModel.CarMaintenances, EntityOperation.Update);
            }
        }

        public async Task HandleDelete(int carId)
        {
            var car = await _manager.GetCar(carId);
            //will also delete all related maintenances
            await _manager.ManageCar(car, EntityOperation.Delete);
        }
    }
}
