using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Helpers;
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
            var stockLogs = await _context.StockLogs
                .Include(s => s.Employee)
                .Where(s => s.CompletionDate == null)
                .ToListAsync();

            return View(stockLogs);
        }

        // GET: StockLog/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            var stockLogDetailVM = GetDetails(logCode, null);

            return View(stockLogDetailVM);
        }

        // GET: StockLog/Create
        public IActionResult Create()
        {
            DateTime dateToday = DateTime.Now;

            Employee emp = new Employee();
            List<EmployeeVM> lstEmp = new List<EmployeeVM>();

            var queryEmp = from e in _context.Employees
                           join f in _context.Firmas
                           on e.FirmaId equals f.Id
                           select new { e.Id, e.Name, e.Surname, f.FirmaName };

            foreach (var employee in queryEmp)
            {
                lstEmp.Add(new EmployeeVM
                {
                    EmployeeId = employee.Id,
                    EmployeeFullNameFirma = employee.Id + " " + employee.Name + " " + employee.Surname + " (" + employee.FirmaName + ")",
                });
            }


            Stock stock = new Stock();
            List<ProductVM> lstStock = new List<ProductVM>();

            var queryStock = from s in _context.Stock
                             select new { s.Id, s.ProductName, s.ProductBrand };

            foreach (var product in queryStock)
            {
                lstStock.Add(new ProductVM
                {
                    StockId = product.Id,
                    ProductNameBrand = product.ProductName + " - " + product.ProductBrand
                });
            }

            ViewData["employees"] = new SelectList(lstEmp, "EmployeeId", "EmployeeFullNameFirma");
            ViewData["dateToday"] = dateToday;
            ViewData["stock"] = new SelectList(lstStock, "StockId", "ProductNameBrand");
            ViewData["stockx"] = lstStock;
            ViewData["stockQuantity"] = new SelectList(_context.Stock, "Id", "Quantity");

            return View();
        }

        // POST: StockLog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockLogCreateVM stockLogCreateVM)
        {

            if (ModelState.IsValid)
            {
                string[] data = stockLogCreateVM.SelectedProducts.Split(","); //id, amount 
                int[] products = Array.ConvertAll(data, d => int.Parse(d));
                Guid guid = Guid.NewGuid();
                string logCode = guid.ToString();
                StockLog stockLog = new StockLog();

                //TODO: check here if stock/amount exists ==> else return view

                //new StockLogItem
                for (int x = 0; x < data.Length; x++)
                {
                    StockLogItem stockLogItem = new StockLogItem();
                    stockLogItem.LogCode = logCode;
                    stockLogItem.StockId = products[x];
                    x++;
                    stockLogItem.Amount = products[x];

                    _context.Add(stockLogItem);
                }

                //update stock amount
                for (int s = 0; s < data.Length; s++)
                {
                    var stock = await StockHelper.UpdateStockQty(products[s], -products[s + 1], _context);
                    _context.Update(stock);
                    s++;
                }

                //new stocklog
                stockLogCreateVM.LogCode = logCode;
                stockLog = stockLogCreateVM;
                _context.Add(stockLog);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", stockLog.EmployeeId);
            return View(stockLogCreateVM);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,StockLogDate,Action,EmployeeId,LogCode")] StockLog stockLog)
        {
            if (id != stockLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var stockLogItems = GetItemsForStockLog(stockLog);

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
        [HttpGet]
        public async Task<IActionResult> ToComplete(int? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToComplete(int id)
        {
            var stockLog = await _context.StockLogs.FindAsync(id);
            
            //TODO: group this into a method 
            var stockLogItems = GetItemsForStockLog(stockLog);

            //update stock amount for each stocklogitems
            foreach (var item in stockLogItems)
            {
                var stock = await StockHelper.UpdateStockQty(item.StockId, item.Amount, _context);
            }

            stockLog.CompletionDate = DateTime.Now;
            _context.Update(stockLog);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool StockLogExists(int id)
        {
            return _context.StockLogs.Any(e => e.Id == id);
        }

        private StockLogDetailVM GetDetails(string logCode, DateTime? completionDate)
        {
            StockLog stockLog = _context.StockLogs
                .Include(s => s.Employee)
                .Include(s => s.Employee.Firma)
                .FirstOrDefault(s => s.LogCode == logCode);

            List<StockLogItem> stockLogItems = _context.StockLogItems
                .Include(s => s.Stock)
                .Where(s => s.LogCode == logCode).ToList();

            List<StockLogItemVM> stockLogItemsVM = new List<StockLogItemVM>();

            foreach (var item in stockLogItems)
            {
                stockLogItemsVM.Add(new StockLogItemVM
                {
                    Id = item.Id,
                    LogCode = item.LogCode,
                    Amount = item.Amount,
                    StockId = item.StockId,
                    Stock = item.Stock,
                    ProductNameBrand = (item.Stock.ProductName + " " + item.Stock.ProductBrand).ToUpper()
                });
            }

            StockLogDetailVM stockLogDetailVM = new StockLogDetailVM
            {
                StockLogDate = stockLog.StockLogDate,
                EmployeeFullNameFirma = (stockLog.Employee.Name + " " + stockLog.Employee.Surname + " - " + stockLog.Employee.Firma.FirmaName).ToUpper(),
                LogCode = stockLog.LogCode, //need to show?
                StockLogItems = stockLogItemsVM,
                CompletionDate = completionDate
            };

            return stockLogDetailVM;
        }

        [HttpGet]
        public async Task<IActionResult> CompletedList()
        {
            var confirmedStockLogs = await _context.StockLogs
                .Include(s => s.Employee)
                .Where(s => s.CompletionDate.HasValue)
                .ToListAsync();

            return View(confirmedStockLogs);
        }

        [HttpGet]
        public async Task<IActionResult> CompletedDetails(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            var stockLogDetailVM = GetDetails(logCode, DateTime.Now);

            return View(stockLogDetailVM);
        }

        // GET: StockLog/Delete/5
        [HttpGet]
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
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var stockLog = await _context.StockLogs.FindAsync(id);

            //TODO: group this into a method 
            var stockLogItems = GetItemsForStockLog(stockLog);

            //update stock amount for each stocklogitems
            foreach (var item in stockLogItems)
            {
                var stock = await StockHelper.UpdateStockQty(item.StockId, item.Amount, _context);
            }

            _context.StockLogs.Remove(stockLog);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ToDamaged(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            var stockLog = await _context.StockLogs
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.LogCode == logCode);

            if (stockLog == null)
            {
                return NotFound();
            }

            var stockLogDetails = GetDetails(logCode, null);

            StockLogDamagedVM stockLogDamagedVM = new StockLogDamagedVM
            {
                StockLogDate = stockLogDetails.StockLogDate,
                EmployeeFullNameFirma = stockLogDetails.EmployeeFullNameFirma,
                LogCode = stockLogDetails.LogCode, //TODO: need to show?
                StockLogItems = stockLogDetails.StockLogItems
            };


            return View(stockLogDamagedVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToDamaged(StockLogDamagedVM stockLogDamagedVM)
        {
            if (ModelState.IsValid)
            {
                StockLog stockLog = _context.StockLogs
                        .FirstOrDefault(s => s.LogCode == stockLogDamagedVM.LogCode);

                //TODO: is this check necessary?
                if (stockLogDamagedVM.DamagedStock != "")
                {
                    //complete by completionDate
                    stockLog.CompletionDate = DateTime.Now;
                    stockLog.Damaged = true;
                    _context.Update(stockLog);
                    _context.SaveChanges();

                    string[] data = stockLogDamagedVM.DamagedStock.Split(","); //id, amount 
                    int[] damagedStock = Array.ConvertAll(data, d => int.Parse(d));

                    //new StockDamaged
                    for (int x = 0; x < data.Length; x++)
                    {
                        StockDamaged stockDamaged = new StockDamaged();
                        stockDamaged.LogCode = stockLogDamagedVM.LogCode;
                        stockDamaged.StockId = damagedStock[x];
                        x++;
                        stockDamaged.StockAmount = damagedStock[x];

                        _context.Add(stockDamaged);
                    }
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(DamagedList));
                }
                return View(stockLogDamagedVM);
            }
            return View(stockLogDamagedVM);
        }

        [HttpGet]
        public async Task<IActionResult> DamagedList()
        {
            var stockLog = await _context.StockLogs
                .Include(s => s.Employee)
                .Where(s => s.Damaged == true)
                .ToListAsync();

            //TODO: change DamagedList viewpage to Model: DamagedListVM to add extra props to list (instead of using stocklog)
            //DamagedListVM damagedList = new DamagedListVM
            //{

            //};

            return View(stockLog);
        }

        [HttpGet]
        public async Task<IActionResult> DamagedDetails(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            var damagedDetails = GetDamagedDetails(logCode);

            return View(damagedDetails);
        }

        private DamagedDetailVM GetDamagedDetails(string logCode)
        {
            StockLog stockLog = _context.StockLogs
                        .Include(s => s.Employee)
                        .Include(s => s.Employee.Firma)
                        .FirstOrDefault(s => s.LogCode == logCode);

            //get list damageditems
            List<StockDamaged> damagedItems = new List<StockDamaged>();
            List<StockDamagedVM> damagedItemsVM = new List<StockDamagedVM>();

            damagedItems = _context.StockDamaged
                .Include(d => d.Stock)
                .Where(s => s.LogCode == logCode).ToList();

            foreach (var item in damagedItems)
            {
                damagedItemsVM.Add(new StockDamagedVM
                {
                    Id = item.Id,
                    LogCode = item.LogCode,
                    StockAmount = item.StockAmount,
                    StockId = item.StockId,
                    Stock = item.Stock,
                    ProductNameBrand = (item.Stock.ProductName + " " + item.Stock.ProductBrand).ToUpper()
                });
            }

            DamagedDetailVM damagedDetailVM = new DamagedDetailVM
            {
                StockLog = stockLog,
                EmployeeFullNameFirma = (stockLog.Employee.Name + " " + stockLog.Employee.Surname + " - " + stockLog.Employee.Firma.FirmaName).ToUpper(),
                DamagedItemsVM = damagedItemsVM
            };

            return damagedDetailVM;
        }

        public async Task<IActionResult> RepairStock(int stockId, int stockAmount, int damagedStockId)
        {
            //remove damagedstock record when repaired
            //update stock qty

            var damagedStock = await _context.StockDamaged.FindAsync(damagedStockId);
            var stock = await StockHelper.UpdateStockQty(stockId, stockAmount, _context);

            //check stock?

            _context.Update(stock);
            _context.StockDamaged.Remove(damagedStock);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(DamagedList));
        }

        private IQueryable<StockLogItem> GetItemsForStockLog(StockLog stockLog)
        {
            //get stocklogitems that link with current stocklog
            var stockLogItems = _context.StockLogItems
                .Where(s => s.LogCode == stockLog.LogCode);

            return stockLogItems;            
        }
    }
}
