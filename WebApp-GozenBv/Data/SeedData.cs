using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Data
{
    public static class SeedData
    {
        private static DataDbContext _context;
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            _context = app
                .ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<DataDbContext>();

            if (!_context.Material.Any())
            {
                _context.Material.AddRange(GetMaterial());
                _context.SaveChanges();
                _context.Employees.AddRange(GetEmployees());
                _context.SaveChanges();
                _context.CarPark.AddRange(GetCarPark());
                _context.SaveChanges();
                _context.CarMaintenances.AddRange(GetCarMaintenances());
                _context.SaveChanges();

                _context.MaterialLogs.AddRange(GetMaterialLogs());
                _context.SaveChanges();
                _context.MaterialLogItems.AddRange(GetMaterialLogItems());
                _context.SaveChanges();

                _context.Users.AddRange(GetUsers());
                _context.SaveChanges();
            }

        }

        private static MaterialLog[] GetMaterialLogs()
        {
            var materialLogs = new List<MaterialLog>
            {
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 1, 1),
                    EmployeeId = 1,
                    ReturnDate = new DateTime(2022, 2, 1),
                    Damaged = false,
                    Status = 1,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 2, 1),
                    EmployeeId = 2,
                    ReturnDate = new DateTime(2022, 3, 1),
                    Damaged = false,
                    Status = 2,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 3, 1),
                    EmployeeId = 3,
                    ReturnDate = new DateTime(2022, 4, 1),
                    Damaged = true,
                    Status = 3,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 4, 1),
                    EmployeeId = 1,
                    ReturnDate = new DateTime(2022, 5, 1),
                    Damaged = false,
                    Status = 1,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 5, 1),
                    EmployeeId = 2,
                    ReturnDate = new DateTime(2022, 6, 1),
                    Damaged = true,
                    Status = 2,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 6, 1),
                    EmployeeId = 3,
                    ReturnDate = new DateTime(2022, 7, 1),
                    Damaged = true,
                    Status = 3,
                    LogId = Guid.NewGuid().ToString(),
        }
            };
            return materialLogs.ToArray();
        }


        private static MaterialLogItem[] GetMaterialLogItems()
        {
            var materialLogItems = new List<MaterialLogItem>();
            var materialLogs = _context.MaterialLogs.Select(s => s).ToList();

            bool[] arrDamaged = new bool[4];
            arrDamaged[0] = true;
            arrDamaged[1] = true;
            arrDamaged[2] = true;
            arrDamaged[3] = false;

            var rnd = new Random();
            for (int i = 0; i < materialLogs.Count; i++)
            {
                int counter = 1;
                do
                {
                    var rndMaterialId = rnd.Next(1, 6);
                    var material = _context.Material.Where(s => s.Id == rndMaterialId).FirstOrDefault();
                    string productNameCode = (material.Name + " " + material.Brand).ToUpper();
                    int rndMaterialAmount = rnd.Next(1, 4);
                    bool rndDamaged = arrDamaged[rnd.Next(0, 4)];
                    bool damaged = materialLogs[i].Damaged ? rndDamaged : false;
                    int rndDamagedAmount = materialLogs[i].Damaged == false ? 0 : rndDamaged ? rnd.Next(1, rndMaterialAmount + 1) : 0;
                    int rndRepairedAmount = rndDamagedAmount != 0 ? rnd.Next(0, rndDamagedAmount + 1) : 0;
                    int rndDeletedAmount = rndDamagedAmount - rndRepairedAmount;
                    var rndUsed = rnd.Next(0, 2);
                    bool used = rndUsed == 1 ? true : false;

                    materialLogItems.Add(new MaterialLogItem
                    {
                        LogId = materialLogs[i].LogId,
                        MaterialId = material.Id,
                        MaterialAmount = rndMaterialAmount,
                        IsDamaged = damaged,
                        DamagedAmount = rndDamagedAmount,
                        RepairAmount = rndRepairedAmount,
                        DeleteAmount = rndDeletedAmount,
                        Used = used
                    });

                    counter++;
                } while (counter <= 10);
            }

            return materialLogItems.ToArray();
        }

        private static User[] GetUsers()
        {
            var users = new User[1];

            users[0] = new User
            {
                Email = "gencay.turan@hotmail.com",
                Name = "Gencay Turan"
            };

            return users;
        }

        private static Material[] GetMaterial()
        {
            var material = new Material[5];
            material[0] = new Material
            {
                Name = "Kniptang",
                NewQty = 75,
                MinQty = 50,
                UsedQty = 20,
                Brand = "Knipex",
                Cost = 20
            };

            material[1] = new Material
            {
                Name = "Boormachine",
                NewQty = 80,
                MinQty = 15,
                UsedQty = 20,
                Brand = "Makita",
                Cost = 50
            };
            material[2] = new Material
            {
                Name = "Slijper",
                NewQty = 50,
                MinQty = 20,
                UsedQty = 20,
                Brand = "Bosch",
                Cost = 80
            };
            material[3] = new Material
            {
                Name = "Hamer",
                NewQty = 50,
                MinQty = 50,
                UsedQty = 20,
                Brand = "Hitachi",
            };
            material[4] = new Material
            {
                Name = "Kniptang",
                NewQty = 50,
                MinQty = 25,
                UsedQty = 20,
                Brand = "Andere",
                Cost = 20
            };

            return material;
        }

        private static CarPark[] GetCarPark()
        {
            var CarPark = new CarPark[4];
            CarPark[0] = new CarPark
            {
                LicencePlate = "1-ABC-123",
                ChassisNumber = "ABCDEFGHIJ1234567",
                Brand = "VolksCar",
                Model = "Polo",
                Km = 90000,
                KeuringDate = DateTime.Parse("01/01/2022"),
                DeadlineKeuringDate = DateTime.Parse("01/01/2023"),
                DriverName = "Cemal Ercan",

            };
            CarPark[1] = new CarPark
            {
                LicencePlate = "2-CBA-321",
                ChassisNumber = "KLMNOPQRST7654321",
                Brand = "Ford",
                Model = "Transit",
                Km = 30000,
                KeuringDate = DateTime.Parse("05/02/2020"),
                DeadlineKeuringDate = DateTime.Parse("04/02/2021"),
                DriverName = "Ahmed Ahmedov"
            };
            CarPark[2] = new CarPark
            {
                LicencePlate = "1-XYZ-987",
                ChassisNumber = "QRSTUVXYZA1234567",
                Brand = "VolksCar",
                Model = "Transporter",
                Km = 110000,
                KeuringDate = DateTime.Parse("14/05/2022"),
                DeadlineKeuringDate = DateTime.Parse("30/11/2022"),
            };
            CarPark[3] = new CarPark
            {
                LicencePlate = "1-XYZ-987",
                ChassisNumber = "QRSTUVXYZA1234567",
                Brand = "VolksCar",
                Model = "Transporter",
                Km = 110000,
                KeuringDate = DateTime.Parse("14/05/2022"),
                DeadlineKeuringDate = DateTime.Parse("30/11/2022"),
            };
            return CarPark;
        }

        private static CarMaintenance[] GetCarMaintenances()
        {
            var carMaintenances = new CarMaintenance[4];
            carMaintenances[0] = new CarMaintenance
            {
                CarId = 1,
                MaintenanceKm = 85000,
            };
            carMaintenances[1] = new CarMaintenance
            {
                CarId = 2,
                MaintenanceDate = DateTime.Parse("15/05/2023"),
                MaintenanceInfo = "Motorwissel"
            };
            carMaintenances[2] = new CarMaintenance
            {
                CarId = 3,
                MaintenanceDate = DateTime.Parse("01/01/2023"),
                MaintenanceInfo = "testDone",
                Done = true
            };
            carMaintenances[3] = new CarMaintenance
            {
                CarId = 4,
                MaintenanceDate = DateTime.Parse("01/01/2023"),
                MaintenanceInfo = "testpastdateinfo"
            };
            return carMaintenances;
        }

        private static Employee[] GetEmployees()
        {
            var employees = new Employee[3];
            employees[0] = new Employee
            {
                Name = "Aydin",
                Surname = "Ahmet",
            };
            employees[1] = new Employee
            {
                Name = "Turan",
                Surname = "Gencay",
            };
            employees[2] = new Employee
            {
                Name = "Janssens",
                Surname = "Jan",
            };
            return employees;
        }

    }
}
