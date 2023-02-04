﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<StockLog> StockLogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<WagenPark> WagenPark { get; set; }
        public DbSet<WagenMaintenance> WagenMaintenances { get; set; }
        public DbSet<Firma> Firmas { get; set; }
        public DbSet<StockLogItem> StockLogItems { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<User> Users { get; set; }

        
    }
}