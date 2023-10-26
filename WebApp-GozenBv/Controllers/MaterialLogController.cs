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
        private readonly IMaterialLogManager _logManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IMaterialLogHelper _logHelper;
        private readonly ILogSearchHelper _searchHelper;
        private readonly IUserLogService _userLogService;
        public MaterialLogController(
            IMaterialLogManager logManager,
            IUserLogService userLogService,
            ILogSearchHelper searchHelper,
            IEmployeeManager employeeManager,
            IMaterialLogHelper logHelper)
        {
            _logManager = logManager;
            _userLogService = userLogService;
            _searchHelper = searchHelper;
            _employeeManager = employeeManager;
            _logHelper = logHelper;
        }

        // GET: MaterialLog
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int sortStatus, int sortOrder)
        {
            var logs = await _logManager.MapMaterialLogs();

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

            if (logCode.IsNullOrEmpty())
            {
                return PartialView("_EntityNotFound");
            }

            return View(await _logManager.MapMaterialLogDetails(logCode));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var createViewModel = await MapLogCreateViewData();

            ViewData["dateToday"] = DateTime.Today.ToString("yyyy-MM-dd");

            //TODO: change the viewbags to returning viewModel as you did with carpark?
            ViewData["employees"] = new SelectList(createViewModel.EmployeesList, "EmployeeId", "EmployeeFullName"); ;
            ViewData["material"] = new SelectList(createViewModel.MaterialsList, "MaterialId", "ProductNameCode");

            return View();
        }

        private async Task<MaterialLogCreateViewModel> MapLogCreateViewData()
        {
            var employees = await _employeeManager.MapEmployees();
            MaterialLogCreateViewModel viewModel = new();

            foreach (var employee in employees)
            {
                viewModel.EmployeesList.Add(new LogCreateEmployeeViewModel
                {
                    EmployeeId = employee.Id,
                    EmployeeFullName = employee.Name + " " + employee.Surname,
                });
            }

            var materials = await _materialManager.MapMaterials();

            foreach (var material in materials)
            {
                viewModel.MaterialsList.Add(new LogCreateMaterialViewModel
                {
                    MaterialId = material.Id,
                    QuantityNew = material.QuantityNew,
                    QuantityUsed = material.QuantityUsed,
                    ProductNameCode = material.ProductName + " - " + material.ProductCode
                });
            }

            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialLogCreateViewModel viewModel)
        {
            //TODO: check modelstate validation proper way
            if (ModelState.IsValid && viewModel.SelectedProducts != null)
            {
                var logCode = await _logManager.MapIncomingLog(viewModel);
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
        public async Task<IActionResult> Edit(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return NotFound();
            }
            var logCode = id;
            
            var logDetails = await _logManager.MapMaterialLogDetails(logCode);

            if (logDetails == null)
            {
                return NotFound();
            }

            ViewData["Employees"] = new SelectList(await _employeeManager.MapEmployees(), "Id", "Id", logDetails.MaterialLog.EmployeeId);
            return View(logDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaterialLogDetailViewModel incomingEdit)
        { 
            if (ModelState.IsValid)
            {
                await _logHelper.HandleEdit(incomingEdit);

                await _userLogService.CreateAsync(ControllerConst.MaterialLog, ActionConst.Edit, incomingEdit.MaterialLog.LogCode);

                return RedirectToAction(nameof(Index));
            }
            ViewData["Employees"] = new SelectList(await _employeeManager.MapEmployees(), "Id", "Id", incomingEdit.MaterialLog.EmployeeId);
            return View(incomingEdit);
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
        public async Task<IActionResult> ReturnItems(MaterialLogDetailViewModel materialLogDetailVM)
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

            if (logCode.IsNullOrEmpty())
            {
                return NotFound();
            }

            return View(await _manager.MapMaterialLogDetails(logCode));
        }

        [HttpPost]
        public async Task<IActionResult> CompleteDamaged(MaterialLogDetailViewModel materialLogDetail)
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
