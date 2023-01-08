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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace WebApp_GozenBv.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataDbContext _context;

        private readonly GraphServiceClient _graphServiceClient;


        public HomeController(ILogger<HomeController> logger,
            DataDbContext context, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _context = context;
            _graphServiceClient = graphServiceClient;

        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> Index()
        {
            List<string> lstAlertsCar = new List<string>();
            var cars = _context.WagenPark.Include(w => w.Firma);

            foreach (var car in cars)
            {
                if (DateTime.Now >= car.DeadlineKeuring.AddMonths(-1))
                {
                    if (DateTime.Now >= car.DeadlineKeuring)
                    {
                        lstAlertsCar.Add("(" + car.Id + ") "
                        + car.LicencePlate
                        + " (" + car.Brand
                        + " - " + car.Model
                        + ") KEURING VERLOPEN OP "
                        + car.DeadlineKeuring.ToShortDateString());
                    }
                    else
                    {
                        int daysLeft = (car.DeadlineKeuring - DateTime.Now).Days + 1;
                        lstAlertsCar.Add("(" + car.Id + ") "
                            + car.LicencePlate
                            + " (" + car.Brand
                            + " - " + car.Model
                            + ") KEURING VERLOOPT BINNEN "
                            + daysLeft
                            + " DAGEN! (" + car.DeadlineKeuring.ToShortDateString() + ")");
                    }
                }
            }

            List<string> lstAlertsStock = new List<string>();
            var stock = _context.Stock.Select(x => x);

            foreach (var product in stock)
            {
                if (product.Quantity < product.MinQuantity)
                {
                    lstAlertsStock.Add("PRODUCTNR "
                    + product.Id
                    + " " + product.ProductName
                    + " - " + product.ProductBrand
                    + " WEINIG IN STOCK! (" + product.Quantity + ")");
                }
            }

            ViewData["alertsCar"] = lstAlertsCar;
            ViewData["alertsStock"] = lstAlertsStock;

            var user = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["ApiResult"] = user.DisplayName;

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
