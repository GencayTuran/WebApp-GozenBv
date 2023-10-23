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


        public CarParkManager(ICarParkDataHandler carParkData, ICarMaintenanceDataHandler carMaintenanceData)
        {
            _carData = carParkData;
            _maintenanceData = carMaintenanceData;
        }

        public async Task<List<CarIndexViewModel>> MapCarsAndFutureMaintenances()
        {
            List<CarIndexViewModel> carIndexViewModel = new();
            var cars = await _carData.GetCars();

            foreach (var car in cars)
            {
                var carMaintenaces = (await _maintenanceData.GetCarMaintenances(maintenance => maintenance.CarId == car.Id && !maintenance.Done)).ToList();
                carIndexViewModel.Add(new CarIndexViewModel
                {
                    Car = car,
                    CarMaintenances = carMaintenaces
                });
            }

            return carIndexViewModel;
        }

        public async Task<CarDetailsViewModel> MapCarAndAllMaintenances(int? id)
        {
            var car = await _carData.GetCarById(id);
            var maintenances = (await _maintenanceData.GetCarMaintenances(c => c.CarId == id && !c.Done)).ToList();

            CarDetailsViewModel carDetailsViewModel = new CarDetailsViewModel
            {
                Car = car,
                CarMaintenances = maintenances
            };

            return carDetailsViewModel;
        }

        public async Task<CarPark> MapCar(int? id)
        {
            return await _carData.GetCarById(id);
        }

        public async Task ManageCar(CarPark car, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _carData.CreateCar(car);
                    break;
                case EntityOperation.Update:
                    await _carData.UpdateCar(car);
                    break;
                case EntityOperation.Delete:
                    await _carData.DeleteCar(car);

                    //deleting all maintenances realted to this car
                    var maintenances = await _maintenanceData.GetCarMaintenances(m => m.CarId == car.Id);
                    foreach (var maintenance in maintenances)
                    {
                        await _maintenanceData.DeleteCarMaintenance(maintenance);
                    }
                    break;
            }
        }

        public async Task ManageCarMaintenance(CarMaintenance carMaintenance, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:

                    var carId = (await _carData.GetCars()).LastOrDefault().Id;

                    if (carMaintenance.MaintenanceDate != null && !String.IsNullOrEmpty(carMaintenance.MaintenanceInfo))
                    {
                        await _maintenanceData.CreateCarMaintenance(new CarMaintenance
                        {
                            CarId = carId,
                            MaintenanceDate = carMaintenance.MaintenanceDate,
                            MaintenanceInfo = carMaintenance.MaintenanceInfo
                        });
                    }

                    if (carMaintenance.MaintenanceKm != null)
                    {
                        await _maintenanceData.CreateCarMaintenance(new CarMaintenance
                        {
                            CarId = carId,
                            MaintenanceKm = carMaintenance.MaintenanceKm,
                        });
                    }

                    break;
                case EntityOperation.Update:
                    await _maintenanceData.UpdateCarMaintenance(carMaintenance);
                    break;
                case EntityOperation.Delete:
                    //is for deleting certain maintenances.
                    await _maintenanceData.DeleteCarMaintenance(carMaintenance);
                    break;
            }
        }

        public async Task ManageCarMaintenances(List<CarMaintenance> carMaintenances, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:

                    var carId = (await _carData.GetCars()).LastOrDefault().Id;

                    foreach (var maintenance in carMaintenances)
                    {
                        if (maintenance.MaintenanceDate != null && !String.IsNullOrEmpty(maintenance.MaintenanceInfo))
                        {
                            await _maintenanceData.CreateCarMaintenance(new CarMaintenance
                            {
                                CarId = carId,
                                MaintenanceDate = maintenance.MaintenanceDate,
                                MaintenanceInfo = maintenance.MaintenanceInfo
                            });
                        }

                        if (maintenance.MaintenanceKm != null)
                        {
                            await _maintenanceData.CreateCarMaintenance(new CarMaintenance
                            {
                                CarId = carId,
                                MaintenanceKm = maintenance.MaintenanceKm,
                            });
                        }
                    }
                    break;
                case EntityOperation.Update:
                    foreach (var maintenance in carMaintenances)
                    {
                        await _maintenanceData.UpdateCarMaintenance(maintenance);
                    }
                    break;
                case EntityOperation.Delete:
                    foreach (var maintenance in carMaintenances)
                    {
                        await _maintenanceData.DeleteCarMaintenance(maintenance);
                    }
                    break;
                default:
                    break;
            }
        }

        public async Task<CarMaintenance> MapCarMaintenance(int? id)
        {
            return await _maintenanceData.GetCarMaintenanceById(id);
        }
    }
}

