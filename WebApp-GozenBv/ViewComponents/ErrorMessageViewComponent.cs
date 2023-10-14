using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp_GozenBv.Data;

namespace WebApp_GozenBv.ViewComponents
{
    public class ErrorMessageViewComponent : ViewComponent
	{
        private readonly DataDbContext _context;

        public ErrorMessageViewComponent(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get the exception message from TempData or ViewBag
            string errorMessage = TempData["ErrorMessage"] as string;

            // Pass the error message to the view
            ViewBag.ErrorMessage = errorMessage;

            return View();
        }
	}
}

