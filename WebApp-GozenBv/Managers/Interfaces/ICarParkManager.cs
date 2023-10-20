using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers.Interfaces
{
    public interface ICarParkManager
    {
        Task<List<CarIndexViewModel>> MapCarsAndFutureMaintenances();
        Task<List<CarIndexViewModel>> MapCarsAndAllMaintenances();
        Task<CarDetailsViewModel> MapCarDetails(int? id);
        Task MapNewCar(CarCreateViewModel carCreate);
    }
}

