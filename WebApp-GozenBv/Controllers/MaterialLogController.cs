using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Graph.SecurityNamespace;
using Microsoft.IdentityModel.Tokens;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Helpers;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class MaterialLogController : Controller
    {
        private readonly IMaterialLogManager _manager;
        private readonly ILogSearchHelper _searchHelper;
        private readonly IUserLogService _userLogService;
        public MaterialLogController(
            IMaterialLogManager manager,
            IUserLogService userLogService,
            ILogSearchHelper searchHelper)
        {
            _manager = manager;
            _userLogService = userLogService;
            _searchHelper = searchHelper;
        }

        // GET: MaterialLog
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int sortStatus, int sortOrder)
        {
            var logs = await _manager.MapMaterialLogs();

            var lstStatusSort = _searchHelper.GetStatusSortList();
            var lstOrderSort = _searchHelper.GetSortOrderList();

            ViewBag.StatusSortList = new SelectList(lstStatusSort, "Id", "Name");
            ViewBag.SortOrderList = new SelectList(lstOrderSort, "Id", "Name");

            //TODO: after finishing restructure, check what a not filtered ints return so the check is right.
            logs = CheckFilters(logs, lstStatusSort, lstOrderSort, searchString, sortStatus, sortOrder);

            return View(logs);
        }


        private List<MaterialLog> CheckFilters(List<MaterialLog> logs, List<SortViewModel> lstStatus,
            List<SortViewModel> lstOrder, string searchString, int sortStatus, int sortOrder)
        {

            if (IsOrderFiltered(sortOrder))
            {
                foreach (var item in lstOrder)
                {
                    if (item.Id == sortOrder)
                    {
                        ViewBag.SortOrderIdParam = item.Id;
                        ViewBag.SortOrderNameParam = item.Name;
                    }
                }

                logs = _searchHelper.SortListByOrder(logs, sortOrder);
            }

            if (IsStatusFiltered(sortStatus))
            {
                foreach (var item in lstStatus)
                {
                    if (item.Id == sortStatus)
                    {
                        ViewBag.SortStatusIdParam = item.Id;
                        ViewBag.SortStatusNameParam = item.Name;
                    }
                }

                logs = _searchHelper.SortListByStatus(logs, sortStatus); 
            }

            if (IsStringFiltered(searchString))
            {
                var trimmedString = searchString.Trim();
                ViewBag.SearchString = trimmedString;

                logs = _searchHelper.FilterList(logs, trimmedString);
            }

            return logs;
        }
        private bool IsStringFiltered(string searchString) => !searchString.IsNullOrEmpty();
        private bool IsStatusFiltered(int sortStatus) => sortStatus != 0;
        private bool IsOrderFiltered(int sortOrder) => sortOrder != 0;



        // GET: MaterialLog/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            string logCode = id;

            var materialLog = await _context.MaterialLogs
                .Where(s => s.LogCode == logCode)
                .FirstOrDefaultAsync();

            if (materialLog == null)
            {
                return PartialView("_EntityNotFound");
            }

            return View(await GetMaterialLogDetails(logCode));
        }

        private readonly List<EmployeeVM> lstEmp = new List<EmployeeVM>();
        private readonly List<MaterialQuantityVM> lstMaterial = new();
        [HttpGet]
        public IActionResult Create()
        {
            GetCreateViewData();

            ViewData["dateToday"] = DateTime.Today.ToString("yyyy-MM-dd");
            //ViewData["employees"] = lstEmp;
            ViewData["employees"] = new SelectList(lstEmp, "EmployeeId", "EmployeeFullName"); ;
            //ViewData["material"] = lstMaterial;
            ViewData["material"] = new SelectList(lstMaterial, "MaterialId", "ProductNameCode");

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

            var materialVM = _context.Material.Select(s => new { s.Id, s.ProductName, s.ProductCode, s.QuantityNew, s.QuantityUsed });

            foreach (var material in materialVM)
            {
                lstMaterial.Add(new MaterialQuantityVM
                {
                    MaterialId = material.Id,
                    QuantityNew = material.QuantityNew,
                    QuantityUsed = material.QuantityUsed,
                    ProductNameCode = material.ProductName + " - " + material.ProductCode
                });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialLogCreateVM materialLogCreateVM)
        {
            //TODO: check modelstate validation proper way
            if (ModelState.IsValid && materialLogCreateVM.SelectedProducts != null)
            {
                var selectedItems = JsonSerializer.Deserialize<List<MaterialLogSelectedItemViewModel>>(materialLogCreateVM.SelectedProducts,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                Guid guid = Guid.NewGuid();
                string logCode = guid.ToString();

                //new materiallog
                MaterialLog materialLog = new()
                {
                    LogCode = logCode,
                    Status = MaterialLogStatusConst.Created,
                    MaterialLogDate = materialLogCreateVM.MaterialLogDate,
                    EmployeeId = materialLogCreateVM.EmployeeId
                };
                _context.Add(materialLog);


                foreach (var item in selectedItems)
                {
                    //new MaterialLogItem
                    MaterialLogItem materialLogItem = new();
                    var material = await GetMaterialAsync(item.MaterialId);

                    if (material.NoReturn)
                    {
                        materialLogItem.DamagedAmount = null;
                        materialLogItem.RepairedAmount = null;
                        materialLogItem.DeletedAmount = null;
                    }

                    materialLogItem.MaterialId = item.MaterialId;
                    materialLogItem.MaterialAmount = item.Amount;
                    materialLogItem.Used = item.Used;

                    materialLogItem.Cost = material.Cost;
                    materialLogItem.LogCode = logCode;
                    materialLogItem.ProductNameCode = (material.ProductName + " " + material.ProductCode).ToUpper();

                    _context.Add(materialLogItem);

                    //update material amount
                    _context.Update(MaterialHelper.TakeMaterial(material, item.Amount, item.Used));
                }

                await _context.SaveChangesAsync();
                await _userLogService.CreateAsync(ControllerConst.MaterialLog, ActionConst.Create, logCode);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                //GetCreateViewData();

                //ViewData["dateToday"] = DateTime.Today.ToString("yyyy-MM-dd");
                //ViewData["employees"] = new SelectList(lstEmp, "EmployeeId", "EmployeeFullName");
                //ViewData["material"] = new SelectList(lstMaterial, "MaterialId", "ProductNameCode");
                //ViewData["materialQuantity"] = lstMaterial;

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

            var materialLog = await _context.MaterialLogs.FindAsync(id);
            if (materialLog == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", materialLog.EmployeeId);
            return View(materialLog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaterialLogDate,Action,EmployeeId,LogCode")] MaterialLog materialLog)
        {
            if (id != materialLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var materialLogItems = await GetMaterialLogItems(materialLog);

                    _context.Update(materialLog);
                    await _context.SaveChangesAsync();
                    await _userLogService.CreateAsync(ControllerConst.MaterialLog, ActionConst.Edit, materialLog.LogCode);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialLogExists(materialLog.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", materialLog.EmployeeId);
            return View(materialLog);
        }

        // GET: MaterialLog/Delete/5
        [HttpGet]
        public async Task<IActionResult> ReturnItems(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            var materialLog = await _context.MaterialLogs
                .FirstOrDefaultAsync(m => m.LogCode == logCode);

            if (materialLog == null)
            {
                return NotFound();
            }

            return View(await GetMaterialLogDetails(logCode));
        }

        [HttpPost]
        public async Task<IActionResult> ReturnItems(MaterialLogDetailVM materialLogDetailVM)
        {
            string logCode = materialLogDetailVM.LogCode;

            MaterialLog materialLog = _context.MaterialLogs
                .FirstOrDefault(s => s.LogCode == logCode);

            if (materialLog == null)
            {
                return NotFound();
            }

            var materialLogItems = await GetMaterialLogItems(materialLog);
            materialLog.ReturnDate = DateTime.Now;

            if (materialLogDetailVM.IsDamaged)
            {
                var damagedMaterials = JsonSerializer.Deserialize<List<ReturnItemsDamagedViewModel>>(materialLogDetailVM.DamagedMaterial,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                //update MaterialLog
                materialLog.Status = MaterialLogStatusConst.DamagedAwaitingAction;
                materialLog.Damaged = true;
                _context.Update(materialLog);

                //update MaterialLogItems
                foreach (var materiallogItem in materialLogItems)
                {
                    foreach (var damagedItem in damagedMaterials)
                    {
                        if (materiallogItem.MaterialId == damagedItem.MaterialId)
                        {
                            var material = await GetMaterialAsync(damagedItem.MaterialId);
                            materiallogItem.DamagedAmount = damagedItem.DamagedAmount;
                            materiallogItem.IsDamaged = true;
                            _context.Update(materiallogItem);

                            var notDamagedAmount = materiallogItem.MaterialAmount - damagedItem.DamagedAmount;
                            if (notDamagedAmount != 0)
                            {
                                _context.Update(MaterialHelper.AddToUsed(material, notDamagedAmount));
                            }
                        }
                        else
                        {
                            var material = await GetMaterialAsync(materiallogItem.MaterialId);
                            _context.Update(MaterialHelper.AddToUsed(material, materiallogItem.MaterialAmount));
                        }
                    }
                }
                await _context.SaveChangesAsync();
                await _userLogService.CreateAsync(ControllerConst.MaterialLog, ActionConst.ReturnItems, logCode);

                //TODO: routeValue could be more readable
                return RedirectToAction("Details", new RouteValueDictionary(
                    new { ControllerContext = "MaterialLog", Action = "Details", Id = logCode }));
            }

            materialLog.Status = MaterialLogStatusConst.Returned;
            _context.Update(materialLog);

            //update material amount for each materiallogitems
            foreach (var item in materialLogItems)
            {
                var material = await GetMaterialAsync(item.MaterialId);
                _context.Update(MaterialHelper.AddToUsed(material, item.MaterialAmount));
            }

            _context.SaveChanges();

            await _userLogService.CreateAsync(ControllerConst.MaterialLog, ActionConst.ReturnItems, logCode);

            return RedirectToAction("Details", new RouteValueDictionary(
                    new { ControllerContext = "MaterialLog", Action = "Details", Id = logCode }));
        }

        [HttpGet]
        public async Task<IActionResult> CompleteDamaged(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            return View(await GetMaterialLogDetails(logCode));
        }

        [HttpPost]
        public async Task<IActionResult> CompleteDamaged(MaterialLogDetailVM materialLogDetail)
        {
            string logCode = materialLogDetail.LogCode;

            var materialLog = await GetMaterialLog(logCode);
            var materialLogItems = await GetMaterialLogItems(materialLog);

            if (materialLog == null)
            {
                return NotFound();
            }
            materialLog.Status = MaterialLogStatusConst.Returned;
            _context.Update(materialLog);

            var completeDamagedMaterial = JsonSerializer.Deserialize<List<CompleteDamagedMaterialViewModel>>(materialLogDetail.DamagedMaterial,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            //update MaterialLogItems
            foreach (var damagedItem in completeDamagedMaterial)
            {
                foreach (var materialLogItem in materialLogItems)
                {
                    if (materialLogItem.MaterialId == damagedItem.MaterialId)
                    {
                        var material = await GetMaterialAsync(damagedItem.MaterialId);
                        materialLogItem.RepairedAmount = damagedItem.RepairedAmount;
                        materialLogItem.DeletedAmount = damagedItem.DeletedAmount;
                        _context.Update(MaterialHelper.AddToUsed(material, damagedItem.RepairedAmount));
                    }
                }
            }
            await _context.SaveChangesAsync();
            await _userLogService.CreateAsync(ControllerConst.MaterialLog, ActionConst.CompleteDamaged, logCode);

            //TODO: routeValue could be more readable
            return RedirectToAction("Details", new RouteValueDictionary(
                new { ControllerContext = "MaterialLog", Action = "Details", Id = logCode }));
        }

        public async Task<IActionResult> Undo(string id)
        {
            var logCode = id;

            var materialLog = await GetMaterialLog(logCode);
            var materialLogItems = await GetMaterialLogItems(materialLog);

            switch (materialLog.Status)
            {
                case MaterialLogStatusConst.DamagedAwaitingAction:

                    foreach (var item in materialLogItems)
                    {
                        item.DamagedAmount = item.DamagedAmount == null ? 0 : item.DamagedAmount;
                        var notDamagedItems = item.MaterialAmount - item.DamagedAmount;
                        item.DamagedAmount = 0;
                        _context.Update(item);

                        var material = await GetMaterialAsync(item.MaterialId);
                        _context.Update(MaterialHelper.UndoAddToUsed(material, (int)notDamagedItems));
                    }
                    materialLog.Status = MaterialLogStatusConst.Created;
                    materialLog.Damaged = false;
                    break;

                case MaterialLogStatusConst.Returned:

                    if (materialLog.Damaged)
                    {
                        foreach (var item in materialLogItems)
                        {
                            var material = await GetMaterialAsync(item.MaterialId);
                            if (item.RepairedAmount > 0)
                            {
                                _context.Update(MaterialHelper.UndoAddToUsed(material, (int)item.RepairedAmount));
                                item.RepairedAmount = 0;
                            }
                            item.DeletedAmount = 0;
                            _context.Update(item);

                            var notDamagedItems = item.MaterialAmount - item.DamagedAmount;
                            _context.Update(MaterialHelper.UndoAddToUsed(material, (int)notDamagedItems));
                        }
                        materialLog.Status = MaterialLogStatusConst.DamagedAwaitingAction;
                    }
                    else
                    {
                        foreach (var item in materialLogItems)
                        {
                            var material = await GetMaterialAsync(item.MaterialId);
                            _context.Update(MaterialHelper.UndoAddToUsed(material, item.MaterialAmount));
                        }
                        materialLog.Status = MaterialLogStatusConst.Created;
                    }
                    break;

                default:
                    break;
            }
            _context.Update(materialLog);
            _context.SaveChanges();

            return RedirectToAction("Details", new RouteValueDictionary(
             new { ControllerContext = "MaterialLog", Action = "Details", Id = logCode }));
        }

        private bool MaterialLogExists(int id)
        {
            return _context.MaterialLogs.Any(e => e.Id == id);
        }

        private async Task<MaterialLogDetailVM> GetMaterialLogDetails(string logCode)
        {
            var materialLog = await _context.MaterialLogs
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.LogCode == logCode);

            var materialLogItems = await _context.MaterialLogItems
                .Where(s => s.LogCode == logCode)
                .Where(s => s.IsDamaged == false || s.NoReturn == true).ToListAsync();

            List<MaterialLogItem> materialLogItemsDamaged = new();
            if (materialLog.Damaged)
            {
                materialLogItemsDamaged = await _context.MaterialLogItems
                    .Where(s => s.LogCode == logCode)
                    .Where(s => s.IsDamaged == true)
                    .ToListAsync();
            }

            return new MaterialLogDetailVM
            {
                MaterialLogId = materialLog.Id,
                MaterialLogDate = materialLog.MaterialLogDate,
                EmployeeFullName = (materialLog.Employee.Name + " " + materialLog.Employee.Surname).ToUpper(),
                LogCode = materialLog.LogCode,
                MaterialLogItems = materialLogItems,
                MaterialLogItemsDamaged = materialLogItemsDamaged,
                ReturnDate = materialLog.ReturnDate,
                Status = materialLog.Status,
                IsDamaged = materialLog.Damaged,
            };
        }

        // GET: MaterialLog/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await GetMaterialLogDetails(id));
        }

        // POST: MaterialLog/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var logCode = id;

            var materialLog = await _context.MaterialLogs
                .Where(s => s.LogCode == logCode)
                .FirstOrDefaultAsync();

            var materialLogItems = await GetMaterialLogItems(materialLog);

            //update material & remove its materiallogitems
            foreach (var item in materialLogItems)
            {
                var material = await GetMaterialAsync(item.MaterialId);
                _context.Update(MaterialHelper.UpdateMaterialQty(material, item.MaterialAmount, item.Used));
                _context.MaterialLogItems.Remove(item);
            }

            _context.MaterialLogs.Remove(materialLog);
            _context.SaveChanges();

            await _userLogService.CreateAsync(ControllerConst.MaterialLog, ActionConst.Delete, materialLog.LogCode);

            return RedirectToAction(nameof(Index));
        }

        private async Task<MaterialLog> GetMaterialLog(string logCode)
        {
            return await _context.MaterialLogs.Where(s => s.LogCode == logCode).FirstOrDefaultAsync();
        }

        private async Task<List<MaterialLogItem>> GetMaterialLogItems(MaterialLog materialLog)
        {
            return await _context.MaterialLogItems.Where(s => s.LogCode == materialLog.LogCode).ToListAsync();
        }

        private async Task<Material> GetMaterialAsync(int materialId)
        {
            return await _context.Material.Where(s => s.Id == materialId).FirstOrDefaultAsync();
        }
    }
}
