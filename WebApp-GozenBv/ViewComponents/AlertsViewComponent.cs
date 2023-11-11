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
        private readonly IMaterialManager _materialManager;

        public AlertsViewComponent(ICarParkManager carParkManager, IMaterialManager materialManager)
        {
            _carParkManager = carParkManager;
            _materialManager = materialManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new AlertsViewModel()
            {
                CarAlerts = await _carParkManager.GetCarAlerts(),
                MaterialAlerts = await _materialManager.GetMaterialAlerts(),
            });
        }
    }
}
