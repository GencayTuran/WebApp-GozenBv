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
    public class EmployeeController : Controller
    {
        private readonly DataDbContext _context;
        private readonly IUserLogService _userLogService;

        public EmployeeController(DataDbContext context, IUserLogService userLogService)
        {
            _context = context;
            _userLogService = userLogService;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var dataDbContext = _context.Employees.Include(e => e.Firma);
            return View(await dataDbContext.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,FirmaId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();

                await _userLogService.CreateAsync(ControllerConst.Employee, ActionConst.Create, employee.Id.ToString());

                return RedirectToAction(nameof(Index));
            }
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id", employee.FirmaId);
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id", employee.FirmaId);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,FirmaId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    await _userLogService.CreateAsync(ControllerConst.Employee, ActionConst.Edit, employee.Id.ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["FirmaId"] = new SelectList(_context.Firmas, "Id", "Id", employee.FirmaId);
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            await _userLogService.CreateAsync(ControllerConst.Employee, ActionConst.Delete, employee.Id.ToString());
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
