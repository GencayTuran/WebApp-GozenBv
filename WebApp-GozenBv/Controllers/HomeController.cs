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
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly DataDbContext _context;

        public HomeController(
            DataDbContext context)
        {
            _context = context;
        }

        //[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> Index()
        {

            //var user = await _graphServiceClient.Me.Request().GetAsync();
            //ViewData["ApiResult"] = user.DisplayName;

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

        public IActionResult EntityNotFound()
        {
            return PartialView("_EntityNotFound");
        }
    }
}
