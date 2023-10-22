﻿using System;
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

        public async Task<CarDetailsViewModel> MapCarAndMaintenance(int? id)
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

        public async Task<CarPark> MapCar(int? id)
        {
            return await _carData.GetCarById(id);
        }

        public async Task<CarIndexViewModel> MapCarAndAllMaintenances(int? id)
        {
            var car = await _carData.GetCarById(id);
            var maintenances = await _maintenanceData.GetCarMaintenances(m => m.CarId == id);

            return new CarIndexViewModel
            {
                Car = car,
                CarMaintenances = maintenances.ToList(),
            };
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
                    //TODO: check if related maintenances get deleted as well, else we have to implement it also.
                    break;
                default:
                    break;
            }
        }

        public async Task ManageCarMaintenance(CarMaintenance carMaintenance, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:

                    if (carMaintenance.MaintenanceDate != null && !String.IsNullOrEmpty(carMaintenance.MaintenanceInfo))
                    {
                        await _maintenanceData.CreateCarMaintenance(new CarMaintenance
                        {
                            CarId = carMaintenance.Id,
                            MaintenanceDate = carMaintenance.MaintenanceDate,
                            MaintenanceInfo = carMaintenance.MaintenanceInfo
                        });
                    }

                    if (carMaintenance.MaintenanceKm != null)
                    {
                        await _maintenanceData.CreateCarMaintenance(new CarMaintenance
                        {
                            CarId = carMaintenance.Id,
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

