using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class CarParkController : Controller
    {
        private readonly IUserLogService _userLogService;
        private readonly ICarParkManager _manager;

        public CarParkController(ICarParkManager manager, IUserLogService userLogService)
        {
            _manager = manager;
            _userLogService = userLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _manager.MapCarsAndFutureMaintenances());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _manager.MapCarDetails(id);

            if (car == null)
            {
                return PartialView("_EntityNotFound");
            }

            return View(car);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCreateViewModel carCreate)
        {
            if (ModelState.IsValid)
            {
                //maps car and possible maintenances
                await _manager.MapNewCar(carCreate);

                await _userLogService.CreateAsync(ControllerConst.CarPark, ActionConst.Create, carCreate.Car.Id.ToString());\

                return RedirectToAction(nameof(Index));
            }
            return View(carCreate);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //TODO: this could be mapped to CarEditViewModel or is CarDetailsViewModel the same?
            //TODO: edit view for car & maintenance unfinished.
            var carPark = await _manager.MapCarDetails(id);

            if (carPark == null)
            {
                return NotFound();
            }

            //TODO: currently it should return car only.
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

        public async Task<IActionResult> CompleteAlert(int id)
        {
            var maintenance = await _context.CarMaintenances
                .Where(m => m.Id == id).FirstOrDefaultAsync();

            maintenance.Done = true;

            _context.Update(maintenance);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new RouteValueDictionary(
                new { ControllerContext = "CarPark", Action = "Details", Id = maintenance.CarId }));
        }

        private bool CarParkExists(int id)
        {
            return _context.CarPark.Any(e => e.Id == id);
        }

        

        

    }
}
