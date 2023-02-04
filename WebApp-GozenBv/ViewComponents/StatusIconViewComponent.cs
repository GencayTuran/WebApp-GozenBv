﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp_GozenBv.Services;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.ViewComponents
{
    public class StatusIconViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int status, bool isDamaged, int col)
        {
            var statusIconViewModel = new StatusIconViewModel();
            statusIconViewModel.Status = status;
            statusIconViewModel.IsDamaged = isDamaged;
            statusIconViewModel.Col = col;

            return View(statusIconViewModel);
        }
    }
}
