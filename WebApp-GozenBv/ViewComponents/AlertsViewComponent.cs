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
using WebApp_GozenBv.Managers.Interfaces;

namespace WebApp_GozenBv.ViewComponents
{
    public class AlertsViewComponent : ViewComponent
    {
        private readonly ICarParkManager _carParkManager;
        private readonly IStockManager _stockManager;

        public AlertsViewComponent(ICarParkManager carParkManager, IStockManager stockManager)
        {
            _carParkManager = carParkManager;
            _stockManager = stockManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new AlertsViewModel()
            {
                CarAlerts = await _carParkManager.MapCarAlerts(),
                StockAlerts = await _stockManager.MapMaterialAlerts(),
            });
        }
    }
}
