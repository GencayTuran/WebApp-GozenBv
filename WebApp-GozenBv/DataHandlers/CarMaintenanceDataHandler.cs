using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.DataHandlers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public class CarMaintenanceDataHandler : ICarMaintenanceDataHandler
    {
        private readonly DataDbContext _context;

        public CarMaintenanceDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarMaintenance>> GetCarMaintenances(Expression<Func<CarMaintenance, bool>> filterExpression)
        {
            return filterExpression != null ?
                await _context.CarMaintenances.Where(filterExpression).ToListAsync()
                : await _context.CarMaintenances.Select(x => x).ToListAsync();
        }

        public async Task CreateCarMaintenance(CarMaintenance maintenance)
        {
            await _context.AddAsync(maintenance);
            await _context.SaveChangesAsync();
        }
    }
}
