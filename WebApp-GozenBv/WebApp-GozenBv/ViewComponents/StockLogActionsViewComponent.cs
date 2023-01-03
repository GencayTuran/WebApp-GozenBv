using Microsoft.AspNetCore.Mvc;

namespace WebApp_GozenBv.ViewComponents
{
    public class StockLogActionsViewComponent : ViewComponent
    {
        public int Status { get; set; }

        public IViewComponentResult Invoke()
        {


            return View();
        }
    }
}
