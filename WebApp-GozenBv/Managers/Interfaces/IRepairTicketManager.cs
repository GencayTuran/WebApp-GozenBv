using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Managers.Interfaces
{
    public interface IRepairTicketManager
    {
        Task ManageTicketAsync(RepairTicket ticket, EntityOperation operation);
        Task ManageTicketsAsync(List<RepairTicket> tickets, EntityOperation operation);
        void ManageTickets(List<RepairTicket> tickets, EntityOperation operation);

        Task<List<RepairTicket>> GetTicketsAsync();
        Task<RepairTicket> GetTicketAsync(int? ticketId);
        Task<List<RepairTicket>> GetTicketsByLogIdAsync(string logId);
    }
}