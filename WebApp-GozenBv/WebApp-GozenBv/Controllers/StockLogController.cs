using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Controllers
{
    public class StockLogController : Controller
    {
        private readonly DataDbContext _context;

        public StockLogController(DataDbContext context)
        {
            _context = context;
        }

        // GET: StockLog
        public async Task<IActionResult> Index()
        {
            var dataDbContext = _context.StockLogs.Include(s => s.Employee);
            return View(await dataDbContext.ToListAsync());
        }

        // GET: StockLog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLog = await _context.StockLogs
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockLog == null)
            {
                return NotFound();
            }

            return View(stockLog);
        }

        // GET: StockLog/Create
        public IActionResult Create()
        {
            DateTime dateToday = DateTime.Now;
            List<string> lstActions = new List<string>();

            Employee emp = new Employee();
            List<StockLogCreationVM> lstEmp = new List<StockLogCreationVM>();

            var queryEmp = from e in _context.Employees
                         join f in _context.Firmas
                         on e.FirmaId equals f.Id
                         select new { e.Id, e.Name, e.Surname, f.FirmaName };

            foreach (var employee in queryEmp)
            {
                lstEmp.Add(new StockLogCreationVM
                {
                    EmployeeId = employee.Id,
                    EmployeeFullNameFirma = employee.Id + " " + employee.Name + " " + employee.Surname + " (" + employee.FirmaName + ")",
                });
            }

            //

            Stock stock = new Stock();
            List<StockLogCreationVM> lstStock = new List<StockLogCreationVM>();

            var queryStock = from s in _context.Stock
                           join p in _context.ProductBrands
                           on s.ProductBrandId equals p.Id
                           select new { s.Id, s.ProductName, p.Name };

            foreach (var product in queryStock)
            {
                lstStock.Add(new StockLogCreationVM
                {
                    ProductId = product.Id,
                    ProductNameBrand = product.ProductName + " - " + product.Name
                });
            }

            lstActions.Add("Ophalen");
            lstActions.Add("Terugbrengen");

            ViewData["employees"] = new SelectList(lstEmp, "EmployeeId", "EmployeeFullNameFirma");
            ViewData["actions"] = new SelectList(lstActions);
            ViewData["dateToday"] = dateToday;
            ViewData["stock"] = new SelectList(lstStock, "ProductId", "ProductNameBrand");
            ViewData["stockQuantity"] = new SelectList(_context.Stock, "Id", "Quantity");

            return View();
        }

        // POST: StockLog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Action,EmployeeId,OrderId")] StockLog stockLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", stockLog.EmployeeId);
            return View(stockLog);
        }

        // GET: StockLog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLog = await _context.StockLogs.FindAsync(id);
            if (stockLog == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", stockLog.EmployeeId);
            return View(stockLog);
        }

        // POST: StockLog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Action,EmployeeId,OrderId")] StockLog stockLog)
        {
            if (id != stockLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockLogExists(stockLog.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", stockLog.EmployeeId);
            return View(stockLog);
        }

        // GET: StockLog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLog = await _context.StockLogs
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockLog == null)
            {
                return NotFound();
            }

            return View(stockLog);
        }

        // POST: StockLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockLog = await _context.StockLogs.FindAsync(id);
            _context.StockLogs.Remove(stockLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockLogExists(int id)
        {
            return _context.StockLogs.Any(e => e.Id == id);
        }
    }
}
