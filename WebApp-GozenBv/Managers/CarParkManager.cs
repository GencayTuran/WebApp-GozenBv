using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;
using System.Linq;
using System.Linq.Expressions;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.DataHandlers.Interfaces;
using WebApp_GozenBv.Constants;

namespace WebApp_GozenBv.Managers
{
    public class CarParkManager : ICarParkManager
    {
        private readonly ICarParkDataHandler _carData;
        private readonly ICarMaintenanceDataHandler _maintenanceData;


        public CarParkManager(ICarParkDataHandler carParkData)
        {
            _carData = carParkData;
        }

        public async Task<List<CarIndexViewModel>> MapCarsAndFutureMaintenances()
        {
            List<CarIndexViewModel> carIndexViewModel = new();
            var cars = await _carData.GetCars();

            foreach (var car in cars)
            {
                var carMaintenaces = await _maintenanceData.GetCarMaintenances(maintenance => maintenance.CarId == car.Id && !maintenance.Done);
                carIndexViewModel.Add(new CarIndexViewModel
                {
                    Car = car,
                    CarMaintenances = carMaintenaces.ToList()
                });
            }

            return carIndexViewModel;
        }

        public async Task<CarDetailsViewModel> MapCarDetails(int? id)
        {
            var car = await _carData.GetCarById(id);
            var maintenances = await _maintenanceData.GetCarMaintenances(c => c.CarId == id && !c.Done);

            CarDetailsViewModel carDetailsViewModel = new CarDetailsViewModel
            {
                Car = car,
                CarMaintenances = maintenances.ToList()
            };

            return carDetailsViewModel;
        }

        public async Task<List<CarIndexViewModel>> MapCarsAndAllMaintenances()
        {
            throw new NotImplementedException();
        }

        public async Task MapNewCar(CarCreateViewModel carCreate)
        {
            await _carData.CreateCar(carCreate.Car);

            if (carCreate.CarMaintenance.MaintenanceDate != null && !String.IsNullOrEmpty(carCreate.CarMaintenance.MaintenanceInfo))
            {
                await _maintenanceData.CreateCarMaintenance(new CarMaintenance
                {
                    CarId = carCreate.Car.Id,
                    MaintenanceDate = carCreate.CarMaintenance.MaintenanceDate,
                    MaintenanceInfo = carCreate.CarMaintenance.MaintenanceInfo
                });
            }

            if (carCreate.CarMaintenance.MaintenanceKm != null)
            {
                await _maintenanceData.CreateCarMaintenance(new CarMaintenance
                {
                    CarId = carCreate.Car.Id,
                    MaintenanceKm = carCreate.CarMaintenance.MaintenanceKm,
                });
            }
        }
    }
}

