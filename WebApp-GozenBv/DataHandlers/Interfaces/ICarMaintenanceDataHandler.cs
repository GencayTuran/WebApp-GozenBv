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
        Task CreateCarMaintenance(CarMaintenance maintenance);
    }
}
