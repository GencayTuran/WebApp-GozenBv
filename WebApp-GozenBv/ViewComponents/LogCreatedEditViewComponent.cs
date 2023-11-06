using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp_GozenBv.Managers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.ViewComponents
{
    public class LogCreatedEditViewComponent : ViewComponent
    {
        private readonly IMaterialLogManager _logManager;
        public LogCreatedEditViewComponent(IMaterialLogManager logManager)
        {
            _logManager = logManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string logId)
        {
            var log = await _logManager.GetMaterialLogDetails(logId);
            var viewModel = new LogCreatedEditViewModel()
            {
                LogId = logId,
                EmployeeId = log.MaterialLog.EmployeeId,
                LogDate = log.MaterialLog.LogDate,
                Items = log.Items,
            };

            return View(viewModel);
        }
    }
}
