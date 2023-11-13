using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.ViewComponents
{
    public class LogReturnedEditViewComponent : ViewComponent
    {
        private readonly IMaterialLogManager _logManager;
        public LogReturnedEditViewComponent(IMaterialLogManager logManager)
        {
            _logManager = logManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(MaterialLogViewModel log, List<MaterialLogItemViewModel> items)
        {
            var viewModel = new LogItemsReturnedEditViewModel()
            {
                MaterialLog = log,
                Items = items
            };

            return View(viewModel);
        }
    }
}
