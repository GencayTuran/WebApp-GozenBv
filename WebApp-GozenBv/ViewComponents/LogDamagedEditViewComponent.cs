using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp_GozenBv.Managers.Interfaces;
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

        public async Task<IViewComponentResult> InvokeAsync(string logId)
        {
            var log = await _logManager.MapMaterialLogDetailViewModel(logId);
            var viewModel = new LogItemsReturnedEditViewModel()
            {
                
            };

            return View(viewModel);
        }
    }
}
