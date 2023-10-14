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

namespace WebApp_GozenBv.Managers
{
    public class CarParkManager : ICarParkManager
    {
        private readonly ICarParkDataHandler _carParkData;

        public CarParkManager(ICarParkDataHandler carParkData)
        {
            _carParkData = carParkData;
        }

        public async Task<List<CarIndexViewModel>> GetCarsAndFutureMaintenances()
        {
            List<CarIndexViewModel> carIndexViewModel = new();
            var cars = await _carParkData.GetCars();

            foreach (var car in cars)
            {
                var carMaintenaces = await _carParkData.GetCarMaintenances(maintenance => maintenance.CarId == car.Id && !maintenance.Done);
                carIndexViewModel.Add(new CarIndexViewModel
                {
                    Car = car,
                    CarMaintenances = carMaintenaces.ToList()
                }); ;
            }

            return carIndexViewModel;
        }

        public async Task<List<CarIndexViewModel>> GetCarsAndAllMaintenances()
        {
            throw new NotImplementedException();
        }
    }
}

