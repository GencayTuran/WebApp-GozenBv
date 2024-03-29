﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Debugger.Contracts.HotReload;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services.Interfaces;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeManager _manager;
        private readonly IUserLogService _userLogService;

        public EmployeeController(IEmployeeManager manager, IUserLogService userLogService)
        {
            _manager = manager;
            _userLogService = userLogService;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _manager.GetEmployeesAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _manager.GetEmployeeAsync(id);
            if (employee == null)
            {
                return PartialView("_EntityNotFound");
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _manager.ManageEmployee(employee, EntityOperation.Create);
                await _userLogService.StoreLogAsync(ControllerNames.Employee, ActionConst.Create, employee.Id.ToString());
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _manager.GetEmployeeAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _manager.ManageEmployee(employee, EntityOperation.Update);
                await _userLogService.StoreLogAsync(ControllerNames.Employee, ActionConst.Edit, employee.Id.ToString());

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _manager.GetEmployeeAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _manager.GetEmployeeAsync(id);
            await _manager.ManageEmployee(employee, EntityOperation.Delete);

            await _userLogService.StoreLogAsync(ControllerNames.Employee, ActionConst.Delete, employee.Id.ToString());
            return RedirectToAction(nameof(Index));
        }
    }
}
