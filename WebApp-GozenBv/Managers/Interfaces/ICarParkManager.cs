using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers.Interfaces
{
    public interface ICarParkManager
    {
        Task<List<CarIndexViewModel>> MapCarsAndFutureMaintenances();
        Task<CarIndexViewModel> MapCarAndAllMaintenances(int? id);
        Task<CarDetailsViewModel> MapCarAndMaintenance(int? id);
        Task ManageCar(CarPark car, EntityOperation operation);
        Task ManageCarMaintenance(CarMaintenance carMaintenance, EntityOperation operation);
        Task<CarPark> MapCar(int? id);
        Task<CarMaintenance> MapCarMaintenance(int? id);
    }
}

