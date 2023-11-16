using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Mappers
{
    public class RepairTicketMapper : IRepairTicketMapper
    {
        private readonly IRepairTicketManager _manager;
        public RepairTicketMapper(IRepairTicketManager manager) 
        {
            _manager = manager;
        }

        public List<RepairTicketCardViewModel> MapTicketsToCardViewModel(List<RepairTicket> tickets)
        {
            var ticketsViewModel = new List<RepairTicketCardViewModel>();
            //map each ticket to new viewmodel
            foreach (var ticket in tickets)
            {
                ticketsViewModel.Add(new RepairTicketCardViewModel()
                {
                    Id = ticket.Id,
                    Status = ticket.Status,
                    MaterialName = ticket.Material.Name + " " + ticket.Material.Brand,
                    HasInfo = !(ticket.RepairInfo).IsNullOrEmpty()
                });
            }
            return ticketsViewModel;
        }

        public RepairTicketCardViewModel MapTicketToCardViewModel(RepairTicket ticket)
        {
                return new RepairTicketCardViewModel()
                {
                    Id = ticket.Id,
                    Status = ticket.Status,
                    MaterialName = ticket.Material.Name + " " + ticket.Material.Brand,
                    HasInfo = !(ticket.RepairInfo).IsNullOrEmpty()
                };
        }

        public RepairTicketViewModel MapTicketToViewModel(RepairTicket ticket)
        {
            return new RepairTicketViewModel()
            {
                Id = ticket.Id,
                Status = ticket.Status,
                LogId = ticket.LogId,
                MaterialName = ticket.Material.Name + " " + ticket.Material.Brand,
                RepairInfo = ticket.RepairInfo,
            };
        }
    }
}
