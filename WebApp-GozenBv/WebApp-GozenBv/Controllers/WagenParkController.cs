using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Controllers
{
    public class WagenParkController : Controller
    {
        private readonly DataDbContext _context;

        public WagenParkController(DataDbContext context)
        {
            _context = context;
        }

        // GET: WagenPark
        public async Task<IActionResult> Index()
        {
            var dataDbContext = _context.WagenPark.Include(w => w.Firma);
            return View(await dataDbContext.ToListAsync());
        }

        // GET: WagenPark/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wagenPark = await _context.WagenPark
                .Include(w => w.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wagenPark == null)
            {
                return NotFound();
            }

            return View(wagenPark);
        }

        // GET: WagenPark/Create
        public IActionResult Create()
        {
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id");
            return View();
        }

        // POST: WagenPark/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LicencePlate,ChassisNumber,Brand,Km,KeuringDate,FirmaId")] WagenPark wagenPark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wagenPark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id", wagenPark.FirmaId);
            return View(wagenPark);
        }

        // GET: WagenPark/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wagenPark = await _context.WagenPark.FindAsync(id);
            if (wagenPark == null)
            {
                return NotFound();
            }
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id", wagenPark.FirmaId);
            return View(wagenPark);
        }

        // POST: WagenPark/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LicencePlate,ChassisNumber,Brand,Km,KeuringDate,FirmaId")] WagenPark wagenPark)
        {
            if (id != wagenPark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wagenPark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WagenParkExists(wagenPark.Id))
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
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id", wagenPark.FirmaId);
            return View(wagenPark);
        }

        // GET: WagenPark/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wagenPark = await _context.WagenPark
                .Include(w => w.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wagenPark == null)
            {
                return NotFound();
            }

            return View(wagenPark);
        }

        // POST: WagenPark/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wagenPark = await _context.WagenPark.FindAsync(id);
            _context.WagenPark.Remove(wagenPark);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WagenParkExists(int id)
        {
            return _context.WagenPark.Any(e => e.Id == id);
        }
    }
}
