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
using WebApp_GozenBv.ViewModels;

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
            return View(await GetCarsAndMaintenances());
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = GetCarDetails(id);

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
                _context.Add(carCreate.Car);
                _context.SaveChanges();

                if (carCreate.CarMaintenance.MaintenanceDate != null && !String.IsNullOrEmpty(carCreate.CarMaintenance.MaintenanceInfo))
                {
                    var maintenance = new CarMaintenance
                    {
                        CarId = carCreate.Car.Id,
                        MaintenanceDate = carCreate.CarMaintenance.MaintenanceDate,
                        MaintenanceInfo = carCreate.CarMaintenance.MaintenanceInfo
                    };
                    _context.Add(maintenance);
                }

                if (carCreate.CarMaintenance.MaintenanceKm != null)
                {
                    var maintenance = new CarMaintenance
                    {
                        CarId = carCreate.Car.Id,
                        MaintenanceKm = carCreate.CarMaintenance.MaintenanceKm,
                    };
                    _context.Add(maintenance);
                }
                await _context.SaveChangesAsync();
                await _userLogService.CreateAsync(ControllerConst.CarPark, ActionConst.Create, carCreate.Car.Id.ToString());

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

        private async Task<List<CarIndexViewModel>> GetCarsAndMaintenances()
        {
            var cars = _context.CarPark.Select(x => x);

            List<CarIndexViewModel> carIndexViewModel = new();

            foreach (var car in cars)
            {
                var maintenances = await _context.CarMaintenances
                        .Where(c => c.CarId == car.Id && c.Done == false)
                        .ToListAsync();

                carIndexViewModel.Add(new CarIndexViewModel
                {
                    Car = car,
                    CarMaintenances = maintenances
                });
            }

            return carIndexViewModel;
        }

        private CarDetailsViewModel GetCarDetails(int? id)
        {
            var car = _context.CarPark
                .Where(c => c.Id == id)
                .FirstOrDefault();
            IEnumerable<CarMaintenance> maintenances = _context.CarMaintenances
                .Where(c => c.CarId == id);

            CarDetailsViewModel carDetailsViewModel = new CarDetailsViewModel
            {
                Car = car,
                CarMaintenances = maintenances
            };

            return carDetailsViewModel;
        }

    }
}
