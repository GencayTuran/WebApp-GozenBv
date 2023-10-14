using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.DataHandlers
{
	public interface ICarParkDataHandler
	{
		Task<IEnumerable<CarPark>> GetCars();
		Task<IEnumerable<CarMaintenance>> GetCarMaintenances(Expression<Func<CarMaintenance, bool>> filterExpression);
    }
}

