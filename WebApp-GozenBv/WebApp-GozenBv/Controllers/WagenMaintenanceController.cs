using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Controllers
{
    [Authorize]
    public class WagenMaintenanceController : Controller
    {
        private readonly DataDbContext _context;

        public WagenMaintenanceController(DataDbContext context)
        {
            _context = context;
        }

        // GET: WagenMaintenance
        public async Task<IActionResult> Index()
        {
            return View(await _context.WagenMaintenances.ToListAsync());
        }

        // GET: WagenMaintenance/Details/5
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

        // GET: WagenMaintenance/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WagenMaintenance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaintenanceDate,MaintenanceNotes,WagenId")] WagenMaintenance wagenMaintenance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wagenMaintenance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wagenMaintenance);
        }

        // GET: WagenMaintenance/Edit/5
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

        // POST: WagenMaintenance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: WagenMaintenance/Delete/5
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

        // POST: WagenMaintenance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wagenMaintenance = await _context.WagenMaintenances.FindAsync(id);
            _context.WagenMaintenances.Remove(wagenMaintenance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WagenMaintenanceExists(int id)
        {
            return _context.WagenMaintenances.Any(e => e.Id == id);
        }
    }
}
