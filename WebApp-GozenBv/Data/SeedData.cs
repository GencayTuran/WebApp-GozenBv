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

            if (!_context.Stock.Any())
            {
                _context.Stock.AddRange(GetStock());
                _context.SaveChanges();
                _context.Firmas.AddRange(GetFirmas());
                _context.SaveChanges();
                _context.Employees.AddRange(GetEmployees());
                _context.SaveChanges();
                _context.WagenPark.AddRange(GetWagenPark());
                _context.SaveChanges();

                _context.StockLogs.AddRange(GetStockLogs());
                _context.SaveChanges();
                _context.StockLogItems.AddRange(GetStockLogItems());
                _context.SaveChanges();

                _context.Users.AddRange(GetUsers());
                _context.SaveChanges();
            }

        }

        private static StockLog[] GetStockLogs()
        {
            var stockLogs = new List<StockLog>
            {
                new StockLog
                {
                    StockLogDate = new DateTime(2022, 1, 1),
                    EmployeeId = 1,
                    ReturnDate = new DateTime(2022, 2, 1),
                    Damaged = false,
                    Status = 1,
                    LogCode = Guid.NewGuid().ToString(),
                },
                new StockLog
                {
                    StockLogDate = new DateTime(2022, 2, 1),
                    EmployeeId = 2,
                    ReturnDate = new DateTime(2022, 3, 1),
                    Damaged = false,
                    Status = 2,
                    LogCode = Guid.NewGuid().ToString(),
                },
                new StockLog
                {
                    StockLogDate = new DateTime(2022, 3, 1),
                    EmployeeId = 3,
                    ReturnDate = new DateTime(2022, 4, 1),
                    Damaged = true,
                    Status = 3,
                    LogCode = Guid.NewGuid().ToString(),
                },
                new StockLog
                {
                    StockLogDate = new DateTime(2022, 4, 1),
                    EmployeeId = 1,
                    ReturnDate = new DateTime(2022, 5, 1),
                    Damaged = false,
                    Status = 1,
                    LogCode = Guid.NewGuid().ToString(),
                },
                new StockLog
                {
                    StockLogDate = new DateTime(2022, 5, 1),
                    EmployeeId = 2,
                    ReturnDate = new DateTime(2022, 6, 1),
                    Damaged = true,
                    Status = 2,
                    LogCode = Guid.NewGuid().ToString(),
                },
                new StockLog
                {
                    StockLogDate = new DateTime(2022, 6, 1),
                    EmployeeId = 3,
                    ReturnDate = new DateTime(2022, 7, 1),
                    Damaged = true,
                    Status = 3,
                    LogCode = Guid.NewGuid().ToString(),
        }
            };
            return stockLogs.ToArray();
        }


        private static StockLogItem[] GetStockLogItems()
        {
            var stockLogItems = new List<StockLogItem>();
            var stockLogs = _context.StockLogs.Select(s => s).ToList();

            bool[] arrDamaged = new bool[4];
            arrDamaged[0] = true;
            arrDamaged[1] = true;
            arrDamaged[2] = true;
            arrDamaged[3] = false;

            var rnd = new Random();
            for (int i = 0; i < stockLogs.Count; i++)
            {
                int counter = 1;
                do
                {
                    var rndStockId = rnd.Next(1, 6);
                    var stock = _context.Stock.Where(s => s.Id == rndStockId).FirstOrDefault();
                    string productNameBrand = (stock.ProductName + " " + stock.ProductBrand).ToUpper();
                    int rndStockAmount = rnd.Next(1, 4);
                    bool rndDamaged = arrDamaged[rnd.Next(0, 4)];
                    bool damaged = stockLogs[i].Damaged ? rndDamaged : false;
                    int rndDamagedAmount = stockLogs[i].Damaged == false ? 0 : rndDamaged ? rnd.Next(1, rndStockAmount + 1) : 0;
                    int rndRepairedAmount = rndDamagedAmount != 0 ? rnd.Next(0, rndDamagedAmount + 1) : 0;
                    int rndDeletedAmount = rndDamagedAmount - rndRepairedAmount;

                    stockLogItems.Add(new StockLogItem
                    {
                        LogCode = stockLogs[i].LogCode,
                        StockId = stock.Id,
                        ProductNameBrand = productNameBrand,
                        StockAmount = rndStockAmount,
                        IsDamaged = damaged,
                        Cost = stock.Cost,
                        DamagedAmount = rndDamagedAmount,
                        RepairedAmount = rndRepairedAmount,
                        DeletedAmount = rndDeletedAmount,
                    });

                    counter++;
                } while (counter <= 10);
            }

            return stockLogItems.ToArray();
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

        private static Stock[] GetStock()
        {
            var stock = new Stock[5];
            stock[0] = new Stock
            {
                ProductName = "Kniptang",
                Quantity = 75,
                MinQuantity = 50,
                ProductBrand = "Knipex",
                Cost = 20
            };

            stock[1] = new Stock
            {
                ProductName = "Boormachine",
                Quantity = 80,
                MinQuantity = 15,
                ProductBrand = "Makita",
                Cost = 50
            };
            stock[2] = new Stock
            {
                ProductName = "Slijper",
                Quantity = 50,
                MinQuantity = 20,
                ProductBrand = "Bosch",
                Cost = 80
            };
            stock[3] = new Stock
            {
                ProductName = "Hamer",
                Quantity = 50,
                MinQuantity = 50,
                ProductBrand = "Hitachi",
                Cost = 20
            };
            stock[4] = new Stock
            {
                ProductName = "Kniptang",
                Quantity = 50,
                MinQuantity = 25,
                ProductBrand = "Andere",
                Cost = 20
            };

            return stock;
        }

        private static Firma[] GetFirmas()
        {
            var firmas = new Firma[4];
            firmas[0] = new Firma
            {
                FirmaName = "Elektrobel"
            };
            firmas[1] = new Firma
            {
                FirmaName = "Infrux"
            };
            firmas[2] = new Firma
            {
                FirmaName = "FirmaX"
            };
            firmas[3] = new Firma
            {
                FirmaName = "FirmaY"
            };
            return firmas;
        }

        private static WagenPark[] GetWagenPark()
        {
            var wagenPark = new WagenPark[3];
            wagenPark[0] = new WagenPark
            {
                LicencePlate = "1-ABC-123",
                ChassisNumber = "ABCDEFGHIJ1234567",
                Brand = "Volkswagen",
                Model = "Polo",
                Km = 90000,
                FirmaId = 3,
                KeuringDate = DateTime.Parse("01/01/2022"),
                DeadlineKeuring = DateTime.Parse("01/01/2023")
            };
            wagenPark[1] = new WagenPark
            {
                LicencePlate = "2-CBA-321",
                ChassisNumber = "KLMNOPQRST7654321",
                Brand = "Ford",
                Model = "Transit",
                Km = 30000,
                FirmaId = 3,
                KeuringDate = DateTime.Parse("05/02/2020"),
                DeadlineKeuring = DateTime.Parse("04/02/2021")
            };
            wagenPark[2] = new WagenPark
            {
                LicencePlate = "1-XYZ-987",
                ChassisNumber = "QRSTUVXYZA1234567",
                Brand = "Volkswagen",
                Model = "Transporter",
                Km = 110000,
                FirmaId = 4,
                KeuringDate = DateTime.Parse("14/05/2022"),
                DeadlineKeuring = DateTime.Parse("30/11/2022")
            };
            return wagenPark;
        }

        private static Employee[] GetEmployees()
        {
            var employees = new Employee[3];
            employees[0] = new Employee
            {
                Name = "Aydin",
                Surname = "Ahmet",
                FirmaId = 1
            };
            employees[1] = new Employee
            {
                Name = "Turan",
                Surname = "Gencay",
                FirmaId = 1
            };
            employees[2] = new Employee
            {
                Name = "Janssens",
                Surname = "Jan",
                FirmaId = 2
            };
            return employees;
        }

    }
}
