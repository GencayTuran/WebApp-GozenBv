using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Services.Interfaces
{
    public interface IRepairTicketService
    {
        Task<RepairTicket> HandleTicket(int? id, RepairTicketAction action);
    }
}