using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers.Interfaces
{
    public interface ICarMaintenanceDataHandler
    {
        Task<IEnumerable<CarMaintenance>> GetCarMaintenances(Expression<Func<CarMaintenance, bool>> filterExpression);
        Task<CarMaintenance> GetCarMaintenanceById(int? id);
        Task CreateCarMaintenance(CarMaintenance maintenance);
        Task UpdateCarMaintenance(CarMaintenance maintenance);
        Task DeleteCarMaintenance(CarMaintenance maintenance);
    }
}
