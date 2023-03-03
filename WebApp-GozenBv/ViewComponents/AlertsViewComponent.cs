using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using System.Linq;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.ViewModels;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewComponents
{
    public class AlertsViewComponent : ViewComponent
    {
        private readonly DataDbContext _context;

        public AlertsViewComponent(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CarAlertViewModel> carAlerts = new();
            List<StockAlertViewModel> stockAlerts = new();

            var cars = await _context.CarPark.Select(x => x).ToListAsync();
            var stock = await _context.Stock.Select(x => x).ToListAsync();
            var maintenances = await _context.CarMaintenances.Where(x => !x.Done).ToListAsync();

            foreach (var car in cars)
            {
                if (DateTime.Now >= car.DeadlineKeuringDate.AddMonths(-1))
                {
                    if (DateTime.Now >= car.DeadlineKeuringDate)
                    {
                        carAlerts.Add(new CarAlertViewModel()
                        {
                            Status = CarAlertsConst.KeuringOutdated,
                            CarPark = car
                        });
                    }
                    else
                    {
                        int daysLeft = (car.DeadlineKeuringDate - DateTime.Now).Days + 1;

                        carAlerts.Add(new CarAlertViewModel()
                        {
                            Status = CarAlertsConst.KeuringOneMonth,
                            CarPark = car
                        });
                    }
                }
            }

            foreach (var maintenance in maintenances)
            {
                var car = _context.CarPark.Where(c => c.Id == maintenance.CarId)
                    .FirstOrDefault();

                if (maintenance.MaintenanceDate != null)
                {
                    if (DateTime.Now >= maintenance.MaintenanceDate.Value.AddMonths(-1))
                    {
                        carAlerts.Add(new CarAlertViewModel()
                        {
                            Status = CarAlertsConst.MaintenanceOneMonth,
                            CarPark = car,
                            CarMaintenance = maintenance
                        });
                    }
                }

                if (maintenance.MaintenanceKm != null)
                { 
                    if (car.Km >= (maintenance.MaintenanceKm - 5000))
                    {
                        carAlerts.Add(new CarAlertViewModel()
                        {
                            Status = CarAlertsConst.MaintenanceKm,
                            CarPark = car,
                            CarMaintenance = maintenance
                        });
                    }
                }
            }

            foreach (var item in stock)
            {
                if (item.Quantity < item.MinQuantity)
                {
                    if (item.Quantity != 0)
                    {
                        stockAlerts.Add(new StockAlertViewModel()
                        {
                            Status = StockAlertsConst.LessThanMinimum,
                            Stock = item
                        });
                    }
                    else
                    {
                        stockAlerts.Add(new StockAlertViewModel()
                        {
                            Status = StockAlertsConst.Empty,
                            Stock = item
                        });
                    }
                }
            }

            AlertsViewModel alerts = new AlertsViewModel()
            {
                CarAlerts = carAlerts,
                StockAlerts = stockAlerts
            };

            return View(alerts);
        }
    }
}
