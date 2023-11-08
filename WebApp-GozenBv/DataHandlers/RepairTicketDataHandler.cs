using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.DataHandlers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public class RepairTicketDataHandler : IRepairTicketDataHandler
    {
        private readonly DataDbContext _context;

        public RepairTicketDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task<List<RepairTicket>> GetTicketsByLogIdAsync(string logId)
        {
            return await _context.RepairTickets.Where(x => x.LogId == logId).ToListAsync();
        }

        public async Task CreateTicketAsync(RepairTicket ticket)
        {
            await _context.RepairTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateTicketAsync(RepairTicket ticket)
        {
            _context.RepairTickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(RepairTicket ticket)
        {
            _context.RepairTickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<RepairTicket> GetTicketAsync(int ticketId)
        {
            return await _context.RepairTickets.FindAsync(ticketId);
        }

        public async Task CreateTicketsAsync(List<RepairTicket> tickets)
        {
            await _context.RepairTickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketsAsync(List<RepairTicket> tickets)
        {
            _context.RepairTickets.UpdateRange(tickets);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketsAsync(List<RepairTicket> tickets)
        {
            _context.RepairTickets.RemoveRange(tickets);
            await _context.SaveChangesAsync();
        }
    }
}
