using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Data
{
    public static class SeedData
    {
        private static DataDbContext context;
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            context = app
                .ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<DataDbContext>();

            UserManager<IdentityUser> userManager = app
               .ApplicationServices.CreateScope()
               .ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await CreateIdentityRecordAsync(userManager);

            if (!context.Stock.Any())
            {
                context.Stock.AddRange(GetStock());
                context.SaveChanges();
                context.Firmas.AddRange(GetFirmas());
                context.SaveChanges();
                context.Employees.AddRange(GetEmployees());
                context.SaveChanges();
                context.WagenPark.AddRange(GetWagenPark());
                context.SaveChanges();
            }

        }

        private static async Task CreateIdentityRecordAsync(UserManager<IdentityUser> userManager)
        {
            var email = "gencay.turan@test.be";
            var userName = "admin";
            if (await userManager.FindByEmailAsync(email) == null &&
                await userManager.FindByNameAsync(userName) == null)
            {
                var pwd = "test1234";
                var identityUser = new IdentityUser() { Email = email, UserName = userName };
                var result = await userManager.CreateAsync(identityUser, pwd);

                if (!result.Succeeded)
                {
                    //
                }
            }
        }

        private static Stock[] GetStock()
        {
            var stock = new Stock[5];
            stock[0] = new Stock
            {
                ProductName = "Kniptang",
                Quantity = 3,
                MinQuantity = 5,
                ProductBrand = "Knipex",
                Cost = 20
            };

            stock[1] = new Stock
            {
                ProductName = "Boormachine",
                Quantity = 5,
                MinQuantity = 1,
                ProductBrand = "Makita",
                Cost = 50
            };
            stock[2] = new Stock
            {
                ProductName = "Slijper",
                Quantity = 3,
                MinQuantity = 1,
                ProductBrand = "Bosch",
                Cost = 80
            };
            stock[3] = new Stock
            {
                ProductName = "Hamer",
                Quantity = 4,
                MinQuantity = 1,
                ProductBrand = "Hitachi",
                Cost = 20
            };
            stock[4] = new Stock
            {
                ProductName = "Kniptang",
                Quantity = 3,
                MinQuantity = 1,
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
