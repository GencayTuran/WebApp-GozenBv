using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Material> Material { get; set; }
        public DbSet<MaterialLog> MaterialLogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<CarPark> CarPark { get; set; }
        public DbSet<CarMaintenance> CarMaintenances { get; set; }
        public DbSet<MaterialLogItem> MaterialLogItems { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<MaterialLogHistory> MaterialLogHistory { get; set; }
        public DbSet<MaterialLogItemHistory> MaterialLogItemsHistory { get; set; }
    }
}
