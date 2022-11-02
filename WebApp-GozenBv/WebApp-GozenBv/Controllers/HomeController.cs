using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataDbContext _context;

        public HomeController(ILogger<HomeController> logger, DataDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<string> lstAlerts = new List<string>();
            var cars = _context.WagenPark.Include(w => w.Firma);
            foreach (var car in cars)
            {
                if ( DateTime.Now >= car.KeuringDate.AddMonths(11))
                {
                    int daysLeft = (car.KeuringDate - DateTime.Now).Days;
                    lstAlerts.Add( car.Id + ". " + car.LicencePlate + " (" + car.Brand + " - " + car.Model + ") KEURING BINNEN " + daysLeft + " DAGEN!");
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
