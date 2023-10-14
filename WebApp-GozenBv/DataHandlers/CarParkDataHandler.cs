using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Services;
using System.Linq;
using WebApp_GozenBv.Models;
using System.Linq.Expressions;

namespace WebApp_GozenBv.DataHandlers
{
    public class CarParkDataHandler : ICarParkDataHandler
    {
        private readonly DataDbContext _context;

        public CarParkDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarMaintenance>> GetCarMaintenances(Expression<Func<CarMaintenance, bool>> filterExpression)
        {
            return filterExpression != null ?
                await _context.CarMaintenances.Where(filterExpression).ToListAsync()
                : await _context.CarMaintenances.Select(x => x).ToListAsync();
        }

        public async Task<IEnumerable<CarPark>> GetCars() => await _context.CarPark.Select(x => x).ToListAsync();
    }
}

