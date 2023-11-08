using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.DataHandlers.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Managers
{
    public class RepairTicketManager : IRepairTicketManager
    {
        private readonly IRepairTicketDataHandler _ticketData;

        public RepairTicketManager(IRepairTicketDataHandler ticketData)
        {
            _ticketData = ticketData;
        }

        public async Task ManageTicketAsync(RepairTicket ticket, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _ticketData.CreateTicketAsync(ticket);
                    break;
                case EntityOperation.Update:
                    await _ticketData.UpdateTicketAsync(ticket);
                    break;
                case EntityOperation.Delete:
                    await _ticketData.DeleteTicketAsync(ticket);
                    break;
            }
        }

        public async Task ManageTicketsAsync(List<RepairTicket> tickets, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _ticketData.CreateTicketsAsync(tickets);
                    break;
                case EntityOperation.Update:
                    await _ticketData.UpdateTicketsAsync(tickets);
                    break;
                case EntityOperation.Delete:
                    await _ticketData.DeleteTicketsAsync(tickets);
                    break;
            }
        }

        public async Task<RepairTicket> GetTicketAsync(int ticketId)
        {
            return await _ticketData.GetTicketAsync(ticketId);
        }

        public async Task<List<RepairTicket>> GetTicketsByLogIdAsync(string logId)
        {
            return await _ticketData.GetTicketsByLogIdAsync(logId);
        }
    }
}
