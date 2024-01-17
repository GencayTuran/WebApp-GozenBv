using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Mappers;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services.Interfaces;

namespace WebApp_GozenBv.Controllers
{
    public class RepairTicketController : Controller
    {
        private readonly IRepairTicketManager _manager;
        private readonly IRepairTicketService _service;
        private readonly IRepairTicketMapper _mapper;

        public RepairTicketController(
            IRepairTicketManager manager,
            IRepairTicketService service,
            IRepairTicketMapper mapper)
        {
            _manager = manager;
            _service = service;
            _mapper = mapper;
        }

        // GET: RepairTicket
        public async Task<IActionResult> Index()
        {
            //viewModel here?
            return View(await _manager.GetTicketsAsync());
        }

        // GET: RepairTicket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _manager.GetTicketAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.MapTicketToViewModel(ticket);

            return View(viewModel);
        }

        public async Task<IActionResult> RepairMaterial(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "id cannot be null");
            }

            await _service.HandleTicket(id, RepairTicketAction.Repair);

            return RedirectToAction("Details", new RouteValueDictionary(
                    new { ControllerContext = "RepairTicket", Action = "Details", Id = id }));
        }

        public async Task<IActionResult> DeleteMaterial(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "id cannot be null");
            }

            await _service.HandleTicket(id, RepairTicketAction.Delete);

            return RedirectToAction("Details", new RouteValueDictionary(
                    new { ControllerContext = "RepairTicket", Action = "Details", Id = id }));
        }
    }
}
