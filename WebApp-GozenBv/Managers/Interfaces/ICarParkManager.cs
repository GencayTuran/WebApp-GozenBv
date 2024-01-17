using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers.Interfaces
{
    public interface ICarParkManager
    {
        Task<CarParkDTO> GetCarParkDTO(int id);
        Task ManageCar(CarPark car, EntityOperation operation);
        Task<CarPark> GetCar(int? id);
        Task<List<CarIndexViewModel>> GetCarsAndFutureMaintenances();
        Task<CarDetailsViewModel> GetCarAndAllMaintenances(int? id);

        Task ManageCarMaintenance(CarMaintenance carMaintenance, EntityOperation operation);
        Task ManageCarMaintenances(List<CarMaintenance> carMaintenances, EntityOperation update);
        Task<CarMaintenance> GetCarMaintenance(int? id);

        Task<List<CarAlertViewModel>> GetCarAlerts();
        Task<int> GetLastCreatedCarId();
    }
}
