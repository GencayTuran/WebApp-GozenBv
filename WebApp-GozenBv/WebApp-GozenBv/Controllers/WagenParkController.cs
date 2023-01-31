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
    public class WagenParkController : Controller
    {
        private readonly DataDbContext _context;
        private readonly IUserLogService _userLogService;

        public WagenParkController(DataDbContext context, IUserLogService userLogService)
        {
            _context = context;
            _userLogService = userLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dataDbContext = _context.WagenPark.Include(w => w.Firma);
            return View(await dataDbContext.ToListAsync());
        }

        [HttpGet]
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
                return PartialView("_EntityNotFound");
            }

            return View(wagenPark);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LicencePlate,ChassisNumber,Brand,Model,Km,KeuringDate,DeadlineKeuring,FirmaId")] WagenPark wagenPark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wagenPark);
                await _context.SaveChangesAsync();
                
                await _userLogService.CreateAsync(ControllerConst.WagenPark, ActionConst.Create, wagenPark.Id.ToString());

                return RedirectToAction(nameof(Index));
            }
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id", wagenPark.FirmaId);
            return View(wagenPark);
        }

        [HttpGet]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LicencePlate,ChassisNumber,Brand,Model,Km,KeuringDate,DeadlineKeuring,FirmaId")] WagenPark wagenPark)
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
                    await _userLogService.CreateAsync(ControllerConst.WagenPark, ActionConst.Edit, wagenPark.Id.ToString());
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

            await _userLogService.CreateAsync(ControllerConst.WagenPark, ActionConst.Delete, wagenPark.Id.ToString());

            return RedirectToAction(nameof(Index));
        }

        private bool WagenParkExists(int id)
        {
            return _context.WagenPark.Any(e => e.Id == id);
        }
    }
}
