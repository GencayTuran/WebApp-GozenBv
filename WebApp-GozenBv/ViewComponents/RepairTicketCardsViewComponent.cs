using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Mappers;
using System.Collections.Generic;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.ViewComponents
{
    public class RepairTicketCardsViewComponent : ViewComponent
    {
        
        private readonly IRepairTicketManager _manager;
        private readonly IRepairTicketMapper _mapper;
        public RepairTicketCardsViewComponent(IRepairTicketMapper mapper, IRepairTicketManager manager)
        {
            _manager = manager;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(string logId)
        {
            var ticketsViewModel = _mapper.MapTicketsToCardViewModel(await _manager.GetTicketsByLogIdAsync(logId));
            return View(ticketsViewModel);
        }
    }
}
