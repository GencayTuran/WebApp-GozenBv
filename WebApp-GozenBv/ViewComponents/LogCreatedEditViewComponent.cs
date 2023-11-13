using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp_GozenBv.Managers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.ViewModels;
using WebApp_GozenBv.Models;
using Microsoft.Graph;
using System.Collections.Generic;

namespace WebApp_GozenBv.ViewComponents
{
    public class LogCreatedEditViewComponent : ViewComponent
    {
        private readonly IMaterialLogManager _logManager;
        public LogCreatedEditViewComponent(IMaterialLogManager logManager)
        {
            _logManager = logManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(MaterialLogViewModel log, List<MaterialLogItemViewModel> items)
        {
            var viewModel = new LogItemsCreatedEditViewModel()
            {
                MaterialLog = log,
                Items = items
            };

            return View(viewModel);
        }
    }
}
