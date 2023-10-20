﻿using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Services;
using System.Linq;
using WebApp_GozenBv.Models;
using System.Linq.Expressions;
using WebApp_GozenBv.DataHandlers.Interfaces;

namespace WebApp_GozenBv.DataHandlers
{
    public class CarParkDataHandler : ICarParkDataHandler
    {
        private readonly DataDbContext _context;

        public CarParkDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarPark>> GetCars() => await _context.CarPark.Select(x => x).ToListAsync();

        public async Task<CarPark> GetCarById(int? id) => await _context.CarPark.FindAsync(id);

        public async Task CreateCar(CarPark car)
        {
            await _context.AddAsync(car);
            await _context.SaveChangesAsync();
        }
    }
}

