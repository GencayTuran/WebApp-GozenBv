using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services
{
    public class ActionService : IActionService
    {
        public List<ActionViewModel> actions = new();
        public async Task<List<ActionViewModel>> GetActionsAsync(int status, int id, string logCode)
        {
            switch (status)
            {
                case StockLogStatusConst.Returned:
                    actions.Add(new ActionViewModel
                    {
                        Action = "Details",
                        RouteId = logCode,
                    });
                    break;

                case StockLogStatusConst.Created:
                    actions.Add(new ActionViewModel
                    {
                        Action = "Edit",
                        RouteId = id.ToString(),
                    });
                    actions.Add(new ActionViewModel
                    {
                        Action = "Details",
                        RouteId = logCode,
                    });
                    actions.Add(new ActionViewModel
                    {
                        Action = "CompleteReturn",
                        RouteId = logCode,
                    });
                    actions.Add(new ActionViewModel
                    {
                        Action = "Delete",
                        RouteId = id.ToString(),
                    });
                    break;

                case StockLogStatusConst.DamagedAwaitingAction:
                    actions.Add(new ActionViewModel
                    {
                        Action = "Details",
                        RouteId = logCode
                    });
                    actions.Add(new ActionViewModel
                    {
                        Action = "CompleteDamaged",
                        RouteId = logCode
                    });

                    break;

                default:
                    //notfound
                    break;
            }
            return actions;
        }
    }
}
