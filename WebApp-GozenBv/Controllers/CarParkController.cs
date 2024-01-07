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
using WebApp_GozenBv.Mappers;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.ViewData;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class CarParkController : Controller
    {
        private readonly ICarParkManager _manager;
        private readonly ICarParkMapper _mapper;
        private readonly ICarParkService _carService;
        private readonly IUserLogService _userLogService;

        public CarParkController(
            ICarParkManager manager,
            ICarParkMapper mapper,
            ICarParkService carService,
            IUserLogService userLogService)
        {
            _manager = manager;
            _mapper = mapper;
            _carService = carService;
            _userLogService = userLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _manager.GetCarsAndFutureMaintenances());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _manager.GetCarAndAllMaintenances(id);

            if (car == null)
            {
                return PartialView("_EntityNotFound");
            }

            return View(car);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["MaintenanceTypes"] = new SelectList(SetCreateViewData(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCreateViewModel carCreate)
        {
            if (ModelState.IsValid)
            {
                await _carService.HandleCreate(carCreate);
                await _userLogService.StoreLogAsync(ControllerNames.CarPark, ActionConst.Create, carCreate.Car.Id.ToString());

                var carId = await _manager.GetLastCreatedCarId();
                return RedirectToDetails(carId);
            }
            return View(carCreate);
        }

        [HttpGet]
        public async Task<IActionResult> EditCar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPark = await _manager.GetCar(id);

            if (carPark == null)
            {
                return NotFound();
            }

            return View(carPark);
        }

        [HttpPost]
        public async Task<IActionResult> EditCar(CarPark car)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _carService.HandleEdit(car);
                }
                catch (Exception)
                {
                    //TODO: handle exception
                    //add viewBag?
                    return View(car);
                }

                await _userLogService.StoreLogAsync(ControllerNames.CarPark, ActionConst.Edit, car.Id.ToString());
                return RedirectToDetails(car.Id);
            }
            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> EditMaintenances(int id)
        {
            var carParkDto = await _manager.GetCarParkDTO(id);
            var viewModel = _mapper.MapEditMaintenances(carParkDto);

            if (carParkDto == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditMaintenances(CarMaintenancesEditViewModel viewModel)
        {
            try
            {
                await _carService.HandleEdit(viewModel);
            }
            catch (Exception)
            {
                //TODO: handle exception
                throw;
            }
            return RedirectToDetails(viewModel.Car.Id);
        }

        public async Task<IActionResult> CompleteAlert(int id)
        {
            var maintenance = await _manager.GetCarMaintenance(id);
            maintenance.IsFinished = true;

            await _manager.ManageCarMaintenance(maintenance, EntityOperation.Update);

            return RedirectToDetails(id);
        }

        // GET: CarPark/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _manager.GetCar(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: CarPark/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _manager.GetCar(id);
            if (car == null)
            {
                return NotFound();
            }

            try
            {
                await _carService.HandleDelete(id);
            }
            catch (Exception)
            {
                //TODO: handle exception
                throw;
            }

            await _userLogService.StoreLogAsync(ControllerNames.CarPark, ActionConst.Delete, car.Id.ToString());
            return RedirectToAction(nameof(Index));
        }

        private List<MaintenanceTypeViewData> SetCreateViewData()
        {
            return new List<MaintenanceTypeViewData>()
            {
                new MaintenanceTypeViewData()
                {
                    Id = MaintenanceTypes.Date,
                    Name = MaintenanceTypes.DateName
                },
                new MaintenanceTypeViewData()
                {
                    Id = MaintenanceTypes.Km,
                    Name = MaintenanceTypes.KmName
                },
                new MaintenanceTypeViewData()
                {
                    Id = MaintenanceTypes.Other,
                    Name = MaintenanceTypes.OtherName
                },
            };
        }

        private bool CarParkExists(int id)
        {
            return _manager.GetCar(id) != null;
        }

        private RedirectToActionResult RedirectToDetails(int id)
        {
            return RedirectToAction("Details", new RouteValueDictionary(
                new { ControllerContext = "CarPark", Action = "Details", Id = id }));
        }
    }
}
