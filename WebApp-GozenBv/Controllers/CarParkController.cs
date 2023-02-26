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
    public class CarParkController : Controller
    {
        private readonly DataDbContext _context;
        private readonly IUserLogService _userLogService;

        public CarParkController(DataDbContext context, IUserLogService userLogService)
        {
            _context = context;
            _userLogService = userLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dataDbContext = _context.CarPark.Select(x => x);
            return View(await dataDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPark = await _context.CarPark
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carPark == null)
            {
                return PartialView("_EntityNotFound");
            }

            return View(carPark);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarPark carPark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carPark);
                await _context.SaveChangesAsync();
                
                await _userLogService.CreateAsync(ControllerConst.CarPark, ActionConst.Create, carPark.Id.ToString());

                return RedirectToAction(nameof(Index));
            }
            return View(carPark);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPark = await _context.CarPark.FindAsync(id);
            if (carPark == null)
            {
                return NotFound();
            }
            return View(carPark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarPark carPark)
        {
            if (id != carPark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carPark);
                    await _context.SaveChangesAsync();
                    await _userLogService.CreateAsync(ControllerConst.CarPark, ActionConst.Edit, carPark.Id.ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarParkExists(carPark.Id))
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
            return View(carPark);
        }

        // GET: CarPark/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPark = await _context.CarPark
                .FirstOrDefaultAsync(m => m.Id == id);

            if (carPark == null)
            {
                return NotFound();
            }

            return View(carPark);
        }

        // POST: CarPark/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carPark = await _context.CarPark.FindAsync(id);
            _context.CarPark.Remove(carPark);
            await _context.SaveChangesAsync();

            await _userLogService.CreateAsync(ControllerConst.CarPark, ActionConst.Delete, carPark.Id.ToString());

            return RedirectToAction(nameof(Index));
        }

        private bool CarParkExists(int id)
        {
            return _context.CarPark.Any(e => e.Id == id);
        }
    }
}
