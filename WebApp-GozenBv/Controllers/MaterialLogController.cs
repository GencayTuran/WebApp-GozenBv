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
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.ViewModels;
using WebApp_GozenBv.ViewData;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class MaterialLogController : Controller
    {
        private readonly IMaterialLogManager _logManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IMaterialManager _materialManager;

        private readonly ILogSearchHelper _searchHelper;

        private readonly IMaterialLogService _logService;
        private readonly IUserLogService _userLogService;
        public MaterialLogController(
            IMaterialLogManager logManager,
            IEmployeeManager employeeManager,
            IMaterialManager materialManager,
            ILogSearchHelper searchHelper,
            IUserLogService userLogService,
            IMaterialLogService logService)
        {
            _logManager = logManager;
            _employeeManager = employeeManager;
            _materialManager = materialManager;
            _searchHelper = searchHelper;
            _userLogService = userLogService;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int sortStatus, int sortOrder)
        {
            var logs = await _logManager.GetMaterialLogs();

            var lstStatusSort = _searchHelper.GetStatusSortList();
            var lstOrderSort = _searchHelper.GetSortOrderList();

            ViewBag.StatusSortList = new SelectList(lstStatusSort, "Id", "Name");
            ViewBag.SortOrderList = new SelectList(lstOrderSort, "Id", "Name");

            //TODO: after finishing restructure, check what a not filtered ints return so the check is right.
            logs = CheckFilters(logs, lstStatusSort, lstOrderSort, searchString, sortStatus, sortOrder);

            return View(logs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            string logCode = id;

            if (logCode.IsNullOrEmpty())
            {
                return PartialView("_EntityNotFound");
            }

            return View(await _logManager.GetMaterialLogDetails(logCode));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["DateToday"] = DateTime.Today.ToString("yyyy-MM-dd");
            ViewData["Employees"] = new SelectList(await GetEmployeesViewData(), "EmployeeId", "EmployeeFullName");
            ViewData["Materials"] = new SelectList(await GetMaterialsViewData(), "MaterialId", "ProductNameCode");

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialLogCreateViewModel viewModel)
        {
            //TODO: check modelstate validation proper way
            if (ModelState.IsValid && viewModel.SelectedProducts != null)
            {
                var logId = await _logService.HandleCreate(viewModel);
                await _userLogService.StoreLogAsync(ControllerNames.MaterialLog, ActionConst.Create, logId);

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
            var logId = id;
            var log = await _logManager.GetMaterialLogAsync(logId);

            if (log == null)
            {
                return NotFound();
            }

            ViewData["Employees"] = new SelectList(await GetEmployeesViewData(), "EmployeeId", "EmployeeFullName");
            ViewData["Materials"] = new SelectList(await GetMaterialsViewData(), "MaterialId", "ProductNameCode");
            return View(new MaterialLogEditViewModel()
            {
                LogId = logId,
                Status = log.Status
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaterialLogDetailViewModel incomingEdit)
        { 
            if (ModelState.IsValid)
            {
                try
                {
                    await _logService.HandleEdit(incomingEdit);
                    await _userLogService.StoreLogAsync(ControllerNames.MaterialLog, ActionConst.Edit, incomingEdit.MaterialLog.LogId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    //TODO: handle exception
                    
                }
            }
            ViewData["Employees"] = new SelectList(await _employeeManager.GetEmployeesAsync(), "Id", "Id", incomingEdit.MaterialLog.EmployeeId);
            return View(incomingEdit);
        }

        [HttpGet]
        public async Task<IActionResult> ReturnItems(string id)
        {
            string logCode = id;

            if (logCode == null)
            {
                return NotFound();
            }

            var logDetails = await _logManager.GetMaterialLogDetails(logCode);

            if (logDetails == null)
            {
                return NotFound();
            }

            return View(logDetails);
        }

        [HttpPost]
        public IActionResult ReturnItems(MaterialLogDetailViewModel incomingReturn)
        {
            var logId = incomingReturn.MaterialLog.LogId;
            try
            {
                //TODO: try to return the full model with entities from view back to here. (employee gives null)
                _logService.HandleReturn(incomingReturn);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }

            _userLogService.StoreLogAsync(ControllerNames.MaterialLog, ActionConst.ReturnItems, logId);

            //TODO: more readable routevalue?
            return RedirectToAction("Details", new RouteValueDictionary(
                    new { ControllerContext = "MaterialLog", Action = "Details", Id = logId }));
        }

        [HttpGet]
        public async Task<IActionResult> CompleteDamaged(string id)
        {
            string logId = id;

            if (logId.IsNullOrEmpty())
            {
                return NotFound();
            }

            return View(await _logManager.GetMaterialLogDetails(logId));
        }

        [HttpPost]
        public async Task<IActionResult> CompleteDamaged(MaterialLogDetailViewModel incomingCompleteDamaged)
        {
            string logId = incomingCompleteDamaged.MaterialLog.LogId;

            await _logService.HandleDamaged(incomingCompleteDamaged);
            
            await _userLogService.StoreLogAsync(ControllerNames.MaterialLog, ActionConst.CompleteDamaged, logId);

            //TODO: routeValue could be more readable
            return RedirectToAction("Details", new RouteValueDictionary(
                new { ControllerContext = "MaterialLog", Action = "Details", Id = logId }));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var logId = id;
            if (logId == null)
            {
                return NotFound();
            }

            return View(await _logManager.GetMaterialLogDetails(logId));
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var logId = id;

            await _logService.HandleDelete(logId);

            await _userLogService.StoreLogAsync(ControllerNames.MaterialLog, ActionConst.Delete, logId);

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<EmployeeViewData>> GetEmployeesViewData()
        {
            var employees = await _employeeManager.GetEmployeesAsync();
            var viewData = new List<EmployeeViewData>();

            foreach (var employee in employees)
            {
                viewData.Add(new EmployeeViewData
                {
                    EmployeeId = employee.Id,
                    EmployeeFullName = employee.Name + " " + employee.Surname,
                });
            }

            return viewData;
        }

        private async Task<List<MaterialViewData>> GetMaterialsViewData()
        {
            var materials = await _materialManager.GetMaterialsAsync();
            var viewData = new List<MaterialViewData>();

            foreach (var material in materials)
            {
                viewData.Add(new MaterialViewData
                {
                    MaterialId = material.Id,
                    QuantityNew = material.NewQty,
                    QuantityUsed = material.UsedQty,
                    ProductNameCode = material.Name + " - " + material.Brand
                });
            }

            return viewData;
        }

        private List<MaterialLog> CheckFilters(List<MaterialLog> logs, List<SortViewModel> lstStatus,
            List<SortViewModel> lstOrder, string searchString, int sortStatus, int sortOrder)
        {
            if (IsNotFiltered(searchString, sortStatus, sortOrder))
            {
                return _searchHelper.SortListByDefault(logs);
            }

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

                logs = _searchHelper.FilterListByString(logs, trimmedString);
            }

            return logs;
        }
        private bool IsStringFiltered(string searchString) => !searchString.IsNullOrEmpty();
        private bool IsStatusFiltered(int sortStatus) => sortStatus != 0;
        private bool IsOrderFiltered(int sortOrder) => sortOrder != 0;
        private bool IsNotFiltered(string searchString, int sortStatus, int sortOrder)
            => !IsOrderFiltered(sortOrder) && !IsStatusFiltered(sortStatus) && !IsStringFiltered(searchString);
    }
}
