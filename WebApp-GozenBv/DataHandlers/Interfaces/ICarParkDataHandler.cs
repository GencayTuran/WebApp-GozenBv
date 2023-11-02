using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.DataHandlers.Interfaces
{
	public interface ICarParkDataHandler
	{
		Task<List<CarPark>> GetCars();
		Task<CarPark> GetCarById(int? id);
		Task CreateCar(CarPark car);
		Task UpdateCar(CarPark car);
        Task DeleteCar(CarPark car);
    }
}

