using System.Threading.Tasks;
using System;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Helpers;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Services.Interfaces;

namespace WebApp_GozenBv.Services
{
    public class RepairTicketService : IRepairTicketService
    {
        private readonly IRepairTicketManager _ticketManager;
        private readonly IMaterialManager _materialManager;
        private readonly IMaterialHelper _materialHelper;
        public RepairTicketService(
            IRepairTicketManager ticketManager,
            IMaterialManager materialManager,
            IMaterialHelper materialHelper)
        {
            _ticketManager = ticketManager;
            _materialManager = materialManager;
            _materialHelper = materialHelper;
        }

        public async Task<RepairTicket> HandleTicket(int? id, RepairTicketAction action)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Ticket id cannot be null.");
            }

            var ticket = await _ticketManager.GetTicketAsync(id);

            if (ticket == null)
            {
                throw new ArgumentNullException($"Ticket with id {id} does not exist.");
            }

            if (ticket.Status == RepairTicketStatus.Repaired)
            {
                throw new Exception("Ticket has already been repaired.");
            }
            if (ticket.Status == RepairTicketStatus.Removed)
            {
                throw new Exception("Ticket has already been deleted.");
            }

            var material = await _materialManager.GetMaterialAsync(ticket.MaterialId);
            if (material == null)
            {
                throw new ArgumentNullException("Material of ticket does not exist anymore.");
            }

            switch (action)
            {
                case RepairTicketAction.Repair:

                    ticket.Status = RepairTicketStatus.Repaired;
                    material = _materialHelper.FinishRepair(material, false);
                    break;

                case RepairTicketAction.Delete:

                    ticket.Status = RepairTicketStatus.Removed;
                    material = _materialHelper.FinishRepair(material, true);
                    break;

                default: throw new Exception($"Internal error: method {nameof(HandleTicket)}");
            }

            await _materialManager.ManageMaterialAsync(material, EntityOperation.Update);
            await _ticketManager.ManageTicketAsync(ticket, EntityOperation.Update);
            return ticket;
        }
    }
}
