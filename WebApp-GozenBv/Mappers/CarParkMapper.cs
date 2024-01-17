using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using WebApp_GozenBv.DataHandlers.Interfaces;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Mappers
{
    public class CarParkMapper : ICarParkMapper
    {
        private readonly ICarParkDataHandler _carData;
        private readonly ICarMaintenanceDataHandler _maintenanceData;

        public CarParkMapper(ICarParkDataHandler carParkData, ICarMaintenanceDataHandler carMaintenanceData)
        {
            _carData = carParkData;
            _maintenanceData = carMaintenanceData;
        }

        //TODO: seperate Gets and Maps and place the Maps here

        public CarMaintenancesEditViewModel MapEditMaintenances(CarParkDTO dto)
        {
            return new CarMaintenancesEditViewModel()
            {
                Car = dto.Car,
                CarMaintenances = dto.CarMaintenances.Where(m => !m.IsFinished).ToList(),
            };
        }

        public List<CarMaintenance> MapViewModelToModel(CarCreateViewModel viewModel)
        {
            var maintenancesDeserialized = JsonSerializer.Deserialize<List<CarMaintenanceCreateViewModel>>(viewModel.CarMaintenances,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            var maintenances = new List<CarMaintenance>();
            foreach (var item in maintenancesDeserialized)
            {
                var maintenance = new CarMaintenance()
                {
                    CarId = viewModel.Car.Id,
                    MaintenanceType = item.MaintenanceType,
                    MaintenanceDate = item.MaintenanceDate,
                    MaintenanceKm = item.MaintenanceKm,
                    MaintenanceInfo = item.MaintenanceInfo,
                    IsFinished = false
                };
                maintenances.Add(maintenance);
            }
            return maintenances;
        }

    }
}
