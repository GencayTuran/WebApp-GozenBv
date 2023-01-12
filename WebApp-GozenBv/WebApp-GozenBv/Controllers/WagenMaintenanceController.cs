using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class WagenMaintenanceController : Controller
    {
        private readonly DataDbContext _context;
        private readonly IUserLogService _userLogService;

        public WagenMaintenanceController(DataDbContext context, IUserLogService userLogService)
        {
            _context = context;
            _userLogService = userLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.WagenMaintenances.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wagenMaintenance = await _context.WagenMaintenances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wagenMaintenance == null)
            {
                return NotFound();
            }

            return View(wagenMaintenance);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaintenanceDate,MaintenanceNotes,WagenId")] WagenMaintenance wagenMaintenance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wagenMaintenance);
                await _context.SaveChangesAsync();

                await _userLogService.CreateAsync(ControllerConst.WagenMaintenance, ActionConst.Create, wagenMaintenance.Id.ToString());

                return RedirectToAction(nameof(Index));
            }
            return View(wagenMaintenance);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wagenMaintenance = await _context.WagenMaintenances.FindAsync(id);
            if (wagenMaintenance == null)
            {
                return NotFound();
            }
            return View(wagenMaintenance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaintenanceDate,MaintenanceNotes,WagenId")] WagenMaintenance wagenMaintenance)
        {
            if (id != wagenMaintenance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wagenMaintenance);
                    await _context.SaveChangesAsync();
                    await _userLogService.CreateAsync(ControllerConst.WagenMaintenance, ActionConst.Edit, wagenMaintenance.Id.ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WagenMaintenanceExists(wagenMaintenance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(wagenMaintenance);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wagenMaintenance = await _context.WagenMaintenances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wagenMaintenance == null)
            {
                return NotFound();
            }

            return View(wagenMaintenance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wagenMaintenance = await _context.WagenMaintenances.FindAsync(id);
            _context.WagenMaintenances.Remove(wagenMaintenance);
            await _context.SaveChangesAsync();

            await _userLogService.CreateAsync(ControllerConst.WagenMaintenance, ActionConst.Create, wagenMaintenance.Id.ToString());

            return RedirectToAction(nameof(Index));
        }

        private bool WagenMaintenanceExists(int id)
        {
            return _context.WagenMaintenances.Any(e => e.Id == id);
        }
    }
}
