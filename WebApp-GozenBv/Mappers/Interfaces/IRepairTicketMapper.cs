using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Mappers
{
    public interface IRepairTicketMapper
    {
        List<RepairTicketCardViewModel> MapTicketsToCardViewModel(List<RepairTicket> tickets);
        RepairTicketViewModel MapTicketToViewModel(RepairTicket repairTicket);
        RepairTicketCardViewModel MapTicketToCardViewModel(RepairTicket ticket);
    }
}