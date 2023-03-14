﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Helpers;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class StockLogController : Controller
    {
        private readonly DataDbContext _context;
        private readonly IUserLogService _userLogService;
        public StockLogController(DataDbContext context, IUserLogService userLogService)
        {
            _context = context;
            _userLogService = userLogService;
        }

        // GET: StockLog
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int sortStatus, int sortOrder)
        {

            var stockLogsAll = _context.StockLogs
                .Include(s => s.Employee);

            List<StockLog> stockLogs = new();
            List<SortViewModel> lstStatus = new()
            {
                new SortViewModel
                {
                    Id = StockLogStatusConst.Created,
                    Name = StockLogStatusConst.CreatedName
                },

                new SortViewModel
                {
                    Id = StockLogStatusConst.Returned,
                    Name = StockLogStatusConst.ReturnedName
                },

                new SortViewModel
                {
                    Id = StockLogStatusConst.DamagedAwaitingAction,
                    Name = StockLogStatusConst.DamagedAwaitingActionName
                }

            };
            List<SortViewModel> lstSortOrder = new()
            {
                new SortViewModel
                {
                    Id = SortOrderConst.DateDescendingId,
                    Name = SortOrderConst.DateDescendingName
                },

                new SortViewModel
                {
                    Id = SortOrderConst.DateAscendingId,
                    Name = SortOrderConst.DateAscendingName
                },

                new SortViewModel
                {
                    Id = SortOrderConst.EmpAzId,
                    Name = SortOrderConst.EmpAzName
                },

                new SortViewModel
                {
                    Id = SortOrderConst.EmpZaId,
                    Name = SortOrderConst.EmpZaName
                }
            };

            ViewBag.StatusSortList = new SelectList(lstStatus, "Id", "Name");
            ViewBag.SortOrderList = new SelectList(lstSortOrder, "Id", "Name");

            foreach (var item in lstSortOrder)
            {
                if (item.Id == sortOrder)
                {
                    ViewBag.SortOrderIdParam = item.Id;
                    ViewBag.SortOrderNameParam = item.Name;
                }
            }

            foreach (var item in lstStatus)
            {
                if (item.Id == sortStatus)
                {
                    ViewBag.SortStatusIdParam = item.Id;
                    ViewBag.SortStatusNameParam = item.Name;
                }
            }

            switch (sortOrder)
            {
                case SortOrderConst.DateDescendingId:
                    stockLogs = stockLogsAll.OrderByDescending(s => s.StockLogDate).ToList();
                    break;
                case SortOrderConst.DateAscendingId:
                    stockLogs = stockLogsAll.OrderBy(s => s.StockLogDate).ToList();
                    break;
                case SortOrderConst.EmpAzId:
                    stockLogs = stockLogsAll.OrderBy(s => s.Employee.Name).ToList();
                    break;
                case SortOrderConst.EmpZaId:
                    stockLogs = stockLogsAll.OrderByDescending(s => s.Employee.Name).ToList();
                    break;
                default:
                    stockLogs = stockLogsAll.OrderByDescending(s => s.StockLogDate).ToList();
                    break;
            }

            if (sortStatus != 0)
            {
                switch (sortStatus)
                {
                    case StockLogStatusConst.Created:
                        stockLogs = stockLogs.Where(s => s.Status == StockLogStatusConst.Created).ToList();
                        break;
                    case StockLogStatusConst.Returned:
                        stockLogs = stockLogs.Where(s => s.Status == StockLogStatusConst.Returned).ToList();
                        break;
                    case StockLogStatusConst.DamagedAwaitingAction:
                        stockLogs = stockLogs.Where(s => s.Status == StockLogStatusConst.DamagedAwaitingAction).ToList();
                        break;
                }
            }

            stockLogs = CheckSearchString(stockLogs, searchString);

            return View(stockLogs);
        }

        private List<StockLog> CheckSearchString(List<StockLog> stockLogs, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                var capitalizedString = (char.ToUpper(searchString[0]) + searchString.Substring(1).ToLower());
                var lowerString = searchString.ToLower();

                stockLogs = stockLogs
                        .Where(s => s.Employee.Name.Contains(searchString)
                            || s.Employee.Surname.Contains(searchString)
                            || s.Employee.Name.Contains(capitalizedString)
                            || s.Employee.Surname.Contains(capitalizedString)
                            || s.Employee.Name.Contains(lowerString)
                            || s.Employee.Surname.Contains(lowerString))
                        .ToList();

                ViewBag.SearchString = searchString;
            }

            return stockLogs;
        }

        // GET: StockLog/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            string logCode = id;

            var stockLog = await _context.StockLogs
                .Where(s => s.LogCode == logCode)
                .FirstOrDefaultAsync();

            if (stockLog == null)
            {
                return PartialView("_EntityNotFound");
            }

            var stockLogDetailVM = GetStockLogDetails(logCode);
            return View(stockLogDetailVM);
        }

        private readonly List<EmployeeVM> lstEmp = new List<EmployeeVM>();
        private readonly List<StockQuantityVM> lstStock = new();
        [HttpGet]
        public IActionResult Create()
        {
            GetCreateViewData();

            ViewData["dateToday"] = DateTime.Today.ToString("yyyy-MM-dd");
            //ViewData["employees"] = lstEmp;
            ViewData["employees"] = new SelectList(lstEmp, "EmployeeId", "EmployeeFullName"); ;
            //ViewData["stock"] = lstStock;
            ViewData["stock"] = new SelectList(lstStock, "StockId", "ProductNameCode");

            return View();
        }

        private void GetCreateViewData()
        {
            var employees = _context.Employees.Select(x => x);

            foreach (var emp in employees)
            {
                var empFullName = String.Format("{0,0} {1,0}", emp.Name, emp.Surname);

                lstEmp.Add(new EmployeeVM
                {
                    EmployeeId = emp.Id,
                    EmployeeFullName = empFullName,
                });
            }

            //stock
            var queryStock = from s in _context.Stock
                             select new { s.Id, s.ProductName, s.ProductCode, s.Quantity, s.QuantityUsed };

            foreach (var product in queryStock)
            {
                lstStock.Add(new StockQuantityVM
                {
                    StockId = product.Id,
                    Quantity = product.Quantity,
                    QuantityUsed = product.QuantityUsed,
                    ProductNameCode = product.ProductName + " - " + product.ProductCode
                });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockLogCreateVM stockLogCreateVM)
        {

            if (ModelState.IsValid && stockLogCreateVM.SelectedProducts != null)
            {
                var selectedItems = JsonSerializer.Deserialize<List<StockLogSelectedItemViewModel>>(stockLogCreateVM.SelectedProducts,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                Guid guid = Guid.NewGuid();
                string logCode = guid.ToString();

                //new stocklog
                StockLog stockLog = new()
                {
                    LogCode = logCode,
                    Status = StockLogStatusConst.Created,
                    StockLogDate = stockLogCreateVM.StockLogDate,
                    EmployeeId = stockLogCreateVM.EmployeeId
                };
                
                _context.Add(stockLog);

                foreach (var item in selectedItems)
                {
                    //new StockLogItem
                    StockLogItem stockLogItem = new();
                    var stock = _context.Stock.Where(s => s.Id == item.StockId).FirstOrDefault();

                    if (stock.NoReturn)
                    {
                        stockLogItem.DamagedAmount = null;
                        stockLogItem.RepairedAmount = null;
                        stockLogItem.DeletedAmount = null;
                    }

                    stockLogItem.StockId = item.StockId;
                    stockLogItem.StockAmount = item.Amount;
                    stockLogItem.Used = item.Used;

                    stockLogItem.Cost = stock.Cost;
                    stockLogItem.LogCode = logCode;
                    stockLogItem.ProductNameCode = (stock.ProductName + " " + stock.ProductCode).ToUpper();

                    _context.Add(stockLogItem);

                    //update stock amount
                    _context.Update(await StockHelper.UpdateStockQty(item.StockId, item.Amount, _context));
                }

                await _context.SaveChangesAsync();

                await _userLogService.CreateAsync(ControllerConst.StockLog, ActionConst.Create, logCode);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                //GetCreateViewData();

                //ViewData["dateToday"] = DateTime.Today.ToString("yyyy-MM-dd");
                //ViewData["employees"] = new SelectList(lstEmp, "EmployeeId", "EmployeeFullName");
                //ViewData["stock"] = new SelectList(lstStock, "StockId", "ProductNameCode");
                //ViewData["stockQuantity"] = lstStock;

                return RedirectToAction(nameof(Create));
            }
        }

        [HttpGet]
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
                    var stockLogItems = GetStockLogItems(stockLog);

                    _context.Update(stockLog);
                    await _context.SaveChangesAsync();
                    await _userLogService.CreateAsync(ControllerConst.StockLog, ActionConst.Edit, stockLog.LogCode);

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
        public async Task<IActionResult> ReturnItems(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            var stockLog = await _context.StockLogs
                .FirstOrDefaultAsync(m => m.LogCode == logCode);

            if (stockLog == null)
            {
                return NotFound();
            }

            var stockLogDetailVM = GetStockLogDetails(logCode);

            return View(stockLogDetailVM);
        }

        // POST: StockLog/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnItems(StockLogDetailVM stockLogDetailVM)
        {
            //TODO: alternative modelstate valid check needed
            string logCode = stockLogDetailVM.LogCode;

            StockLog stockLog = _context.StockLogs
                .FirstOrDefault(s => s.LogCode == logCode);

            if (stockLog == null)
            {
                return NotFound();
            }

            var stockLogItems = GetStockLogItems(stockLog);

            //Status COMPLETE
            if (stockLogDetailVM.DamagedStock != null)
            {

                var damagedStock = ConvertStringToIntArray(stockLogDetailVM.DamagedStock);

                //update StockLogItems
                int x = 0;
                foreach (var item in stockLogItems)
                {
                    if (x < damagedStock.Length && item.StockId == damagedStock[x])
                    {
                        x++;
                        item.DamagedAmount = damagedStock[x];
                        var notDamagedAmount = item.StockAmount - damagedStock[x];
                        await StockHelper.UpdateStockQty(item.StockId, notDamagedAmount, _context);
                        item.IsDamaged = true;
                        _context.Update(item);
                        x++;
                    }
                    else
                    {
                        await StockHelper.UpdateStockQty(item.StockId, item.StockAmount, _context);
                    }
                }
                stockLog.Status = StockLogStatusConst.DamagedAwaitingAction;
                stockLog.Damaged = true;
                _context.Update(stockLog);
                await _context.SaveChangesAsync();

                await _userLogService.CreateAsync(ControllerConst.StockLog, ActionConst.ReturnItems, logCode);

                //TODO: routeValue could be more readable
                return RedirectToAction("Details", new RouteValueDictionary(
                    new { ControllerContext = "StockLog", Action = "Details", Id = logCode }));
            }

            //update stock amount for each stocklogitems
            foreach (var item in stockLogItems)
            {
                var stock = await StockHelper.UpdateStockQty(item.StockId, item.StockAmount, _context);
                _context.Update(stock);
            }

            stockLog.ReturnDate = DateTime.Now;
            stockLog.Status = StockLogStatusConst.Returned;
            _context.Update(stockLog);
            _context.SaveChanges();

            await _userLogService.CreateAsync(ControllerConst.StockLog, ActionConst.ReturnItems, logCode);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CompleteDamaged(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            var stockLogDetailVM = GetStockLogDetails(logCode);

            return View(stockLogDetailVM);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteDamaged(StockLogDetailVM stockLogDetail)
        {
            string logCode = stockLogDetail.LogCode;

            StockLog stockLog = _context.StockLogs
                        .FirstOrDefault(s => s.LogCode == stockLogDetail.LogCode);

            if (stockLog == null)
            {
                return NotFound();
            }

            var damagedStock = ConvertStringToIntArray(stockLogDetail.DamagedStock);

            var stockLogItems = _context.StockLogItems.Where(s => s.LogCode == logCode);

            //update StockLogItems
            for (int x = 0; x < damagedStock.Length; x++)
            {
                foreach (var item in stockLogItems)
                {
                    if (item.StockId == damagedStock[x])
                    {
                        x++;
                        item.RepairedAmount = damagedStock[x];
                        await StockHelper.UpdateStockQty(item.StockId, damagedStock[x], _context);
                        x++;
                        item.DeletedAmount = damagedStock[x];
                        _context.Update(item);
                    }
                }

            }
            stockLog.Status = StockLogStatusConst.Returned;
            _context.Update(stockLog);
            await _context.SaveChangesAsync();

            await _userLogService.CreateAsync(ControllerConst.StockLog, ActionConst.CompleteDamaged, logCode);

            //TODO: routeValue could be more readable
            return RedirectToAction("Details", new RouteValueDictionary(
                new { ControllerContext = "StockLog", Action = "Details", Id = logCode }));
        }

        public async Task<IActionResult> Undo(string id)
        {
            var logCode = id;
            var stockLog = _context.StockLogs
                .Where(s => s.LogCode == logCode)
                .FirstOrDefault();

            var stockLogItems = _context.StockLogItems
                .Where(s => s.LogCode == logCode);

            switch (stockLog.Status)
            {
                case StockLogStatusConst.DamagedAwaitingAction:

                    foreach (var item in stockLogItems)
                    {
                        var notDamagedItems = item.StockAmount - item.DamagedAmount;
                        await StockHelper.UpdateStockQty(item.StockId, -(int)notDamagedItems, _context);
                        item.DamagedAmount = 0;

                        _context.Update(item);
                    }

                    stockLog.Status = StockLogStatusConst.Created;
                    stockLog.Damaged = false;
                    break;

                case StockLogStatusConst.Returned:
                    if (stockLog.Damaged)
                    {
                        foreach (var item in stockLogItems)
                        {
                            if (item.RepairedAmount > 0)
                            {
                                await StockHelper.UpdateStockQty(item.StockId, -(int)item.RepairedAmount, _context);
                            }

                            if (item.DeletedAmount > 0)
                            {
                                await StockHelper.UpdateStockQty(item.StockId, (int)item.DeletedAmount, _context);
                            }

                            var notDamagedItems = item.StockAmount - item.DamagedAmount;
                            await StockHelper.UpdateStockQty(item.StockId, -(int)notDamagedItems, _context);

                            item.RepairedAmount = 0;
                            item.DeletedAmount = 0;

                            _context.Update(item);

                        }
                        stockLog.Status = StockLogStatusConst.DamagedAwaitingAction;
                    }
                    else
                    {
                        foreach (var item in stockLogItems)
                        {
                            await StockHelper.UpdateStockQty(item.StockId, -item.StockAmount, _context);

                            _context.Update(item);
                        }
                        stockLog.Status = StockLogStatusConst.Created;
                    }
                    break;

                default:
                    break;
            }
            _context.Update(stockLog);
            _context.SaveChanges();

            return RedirectToAction("Details", new RouteValueDictionary(
             new { ControllerContext = "StockLog", Action = "Details", Id = logCode }));
        }

        private bool StockLogExists(int id)
        {
            return _context.StockLogs.Any(e => e.Id == id);
        }

        private StockLogDetailVM GetStockLogDetails(string logCode)
        {
            StockLog stockLog = _context.StockLogs
                .Include(s => s.Employee)
                .FirstOrDefault(s => s.LogCode == logCode);

            List<StockLogItem> stockLogItems = new();
                stockLogItems = _context.StockLogItems
                    .Where(s => s.LogCode == logCode)
                    .Where(s => s.IsDamaged == false || s.NoReturn == true).ToList();

            List<StockLogItem> stockLogItemsDamaged = new();
            if (stockLog.Damaged)
            {
                stockLogItemsDamaged = _context.StockLogItems
                    .Where(s => s.LogCode == logCode)
                    .Where(s => s.IsDamaged == true)
                    .ToList();
            }

            StockLogDetailVM stockLogDetailVM = new()
            {
                StockLogId = stockLog.Id,
                StockLogDate = stockLog.StockLogDate,
                EmployeeFullName = (stockLog.Employee.Name + " " + stockLog.Employee.Surname).ToUpper(),
                LogCode = stockLog.LogCode,
                StockLogItems = stockLogItems,
                StockLogItemsDamaged = stockLogItemsDamaged,
                ReturnDate = stockLog.ReturnDate,
                Status = stockLog.Status,
                IsDamaged = stockLog.Damaged,
            };

            return stockLogDetailVM;
        }

        // GET: StockLog/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logCode = _context.StockLogs
                .Where(s => s.Id == id)
                .Select(s => s.LogCode)
                .FirstOrDefault();

            var stockLogDetailVM = GetStockLogDetails(logCode);

            return View(stockLogDetailVM);
        }

        // POST: StockLog/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var logCode = id;

            var stockLog = await _context.StockLogs
                .Where(s => s.LogCode == logCode)
                .FirstOrDefaultAsync();

            var stockLogItems = GetStockLogItems(stockLog);

            //update stock amount for each stocklogitems
            foreach (var item in stockLogItems)
            {
                var stock = await StockHelper.UpdateStockQty(item.StockId, item.StockAmount, _context);
                _context.StockLogItems.Remove(item);
            }

            _context.StockLogs.Remove(stockLog);
            _context.SaveChanges();

            await _userLogService.CreateAsync(ControllerConst.StockLog, ActionConst.Delete, stockLog.LogCode);

            return RedirectToAction(nameof(Index));
        }

        private IQueryable<StockLogItem> GetStockLogItems(StockLog stockLog)
        {
            //get stocklogitems that link with current stocklog
            var stockLogItems = _context.StockLogItems
                .Where(s => s.LogCode == stockLog.LogCode);

            return stockLogItems;
        }

        private int[] ConvertStringToIntArray(string selectedProducts)
        {
            string[] data = selectedProducts.Split(","); //id, amount
            int[] products = Array.ConvertAll(data, d => int.Parse(d));
            return products;
        }
    }
}
