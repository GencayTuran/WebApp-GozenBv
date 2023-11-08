using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers.Interfaces
{
    public interface IRepairTicketDataHandler
    {
        Task CreateTicketAsync(RepairTicket ticket);
        Task UpdateTicketAsync(RepairTicket ticket);
        Task DeleteTicketAsync(RepairTicket ticket);
        Task CreateTicketsAsync(List<RepairTicket> tickets);
        Task UpdateTicketsAsync(List<RepairTicket> tickets);
        Task DeleteTicketsAsync(List<RepairTicket> tickets);

        Task<RepairTicket> GetTicketAsync(int ticketId);
        Task<List<RepairTicket>> GetTicketsByLogIdAsync(string logId);

    }
}