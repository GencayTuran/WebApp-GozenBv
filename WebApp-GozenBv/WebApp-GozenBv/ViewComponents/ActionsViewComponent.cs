using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp_GozenBv.Services;

namespace WebApp_GozenBv.ViewComponents
{
    public class ActionsViewComponent : ViewComponent
    {
        public IActionService _actionService;
        public ActionsViewComponent(IActionService actionService)
        {
            _actionService = actionService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int status, int id, string logCode)
        {
            var actions = await _actionService.GetActionsAsync(status, id, logCode);

            return View(actions);
        }
    }
}
