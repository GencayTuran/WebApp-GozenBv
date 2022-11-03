using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Data
{
    public class SeedData
    {
        public static DataDbContext context;
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DataDbContext>();

            if (!context.Stock.Any())
            {
                context.ProductBrands.AddRange(GetBrands());
                context.SaveChanges();
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
        private static Stock[] GetStock()
        {
            var stock = new Stock[5];
            stock[0] = new Stock
            {
                ProductName = "Kniptang",
                Quantity = 3,
                ProductBrandId = 1,
                Cost = 20
            };

            stock[1] = new Stock
            {
                ProductName = "Boormachine",
                Quantity = 5,
                ProductBrandId = 2,
                Cost = 50
            };
            stock[2] = new Stock
            {
                ProductName = "Slijper",
                Quantity = 3,
                ProductBrandId = 3,
                Cost = 80
            };
            stock[3] = new Stock
            {
                ProductName = "Hamer",
                Quantity = 4,
                ProductBrandId = 4,
                Cost = 20
            };
            stock[4] = new Stock
            {
                ProductName = "Kniptang",
                Quantity = 3,
                ProductBrandId = 5,
                Cost = 20
            };

            return stock;
        }
        private static ProductBrand[] GetBrands()
        {
            var brands = new ProductBrand[5];
            brands[0] = new ProductBrand
            {
                Name = "Knipex"
            };
            brands[1] = new ProductBrand
            {
                Name = "Makita"
            };
            brands[2] = new ProductBrand
            {
                Name = "Bosch"
            };
            brands[3] = new ProductBrand
            {
                Name = "Hitachi"
            };
            brands[4] = new ProductBrand
            {
                Name = "Andere"
            };

            return brands;
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
                KeuringDate = DateTime.Parse("05/02/2022"),
                DeadlineKeuring = DateTime.Parse("04/02/2023")
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
                DeadlineKeuring = DateTime.Parse("14/05/2023")
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
