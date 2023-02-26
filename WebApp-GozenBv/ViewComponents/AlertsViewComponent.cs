using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using System.Linq;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.ViewModels;

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

            var cars = _context.CarPark.Select(x => x);
            var stock = _context.Stock.Select(x => x);

            foreach (var car in cars)
            {
                if (DateTime.Now >= car.DeadlineKeuringDate.AddMonths(-1))
                {
                    if (DateTime.Now >= car.DeadlineKeuringDate)
                    {
                        carAlerts.Add(new CarAlertViewModel()
                        {
                            Status = CarAlertsConst.Outdated,
                            CarPark = car
                        });
                    }
                    else
                    {
                        int daysLeft = (car.DeadlineKeuringDate - DateTime.Now).Days + 1;

                        carAlerts.Add(new CarAlertViewModel()
                        {
                            Status = CarAlertsConst.LessThanOneMonth,
                            CarPark = car
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
