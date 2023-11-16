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
using WebApp_GozenBv.Mappers.Interfaces;
using WebApp_GozenBv.Mappers;
using System.Security.Cryptography;
using WebApp_GozenBv.DTOs;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class MaterialLogController : Controller
    {
        private readonly IMaterialLogManager _logManager;
        private readonly IMaterialLogMapper _logMapper;
        private readonly IEmployeeManager _employeeManager;
        private readonly IMaterialManager _materialManager;

        private readonly ILogSearchHelper _searchHelper;

        private readonly IMaterialLogService _logService;
        private readonly IUserLogService _userLogService;

        private readonly IRepairTicketService _repairService;
        private readonly IRepairTicketMapper _repairMapper;
        public MaterialLogController(
            IMaterialLogManager logManager,
            IEmployeeManager employeeManager,
            IMaterialManager materialManager,
            ILogSearchHelper searchHelper,
            IUserLogService userLogService,
            IMaterialLogService logService,
            IMaterialLogMapper logMapper,
            IRepairTicketService repairService,
            IRepairTicketMapper repairMapper)
        {
            _logManager = logManager;
            _employeeManager = employeeManager;
            _materialManager = materialManager;
            _searchHelper = searchHelper;
            _userLogService = userLogService;
            _logService = logService;
            _logMapper = logMapper;
            _repairService = repairService;
            _repairMapper = repairMapper;
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
            string logId = id;

            if (logId.IsNullOrEmpty())
            {
                return PartialView("_EntityNotFound");
            }

            return View(await _logMapper.MapMaterialLogDetailViewModel(logId));
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
            var logDTO = await _logManager.GetMaterialLogDTO(logId);
            
            if (logDTO == null)
            {
                return NotFound();
            }

            if (logDTO.MaterialLog.Approved)
            {
                throw new Exception("Log cannot be edited after it is approved.");
            }

            var viewModel = _logMapper.MapLogAndItemsToViewModel(logDTO);

            ViewData["Employees"] = new SelectList(await GetEmployeesViewData(), "EmployeeId", "EmployeeFullName");
            ViewData["Materials"] = new SelectList(await GetMaterialsViewData(), "MaterialId", "ProductNameCode");
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string logId, MaterialLogAndItemsViewModel incomingEdit)
        { 
            if (ModelState.IsValid)
            {
                try
                {
                    await _logService.HandleEdit(logId, incomingEdit);
                    await _userLogService.StoreLogAsync(ControllerNames.MaterialLog, ActionConst.Edit, logId);
                    return RedirectToDetails(logId);
                }
                catch (Exception e)
                {
                    //TODO: handle exception
                    
                }
            }
            ViewData["Employees"] = new SelectList(await _employeeManager.GetEmployeesAsync(), "Id", "Id", incomingEdit.MaterialLog.EmployeeId);
            return View(incomingEdit);
        }

        public async Task<IActionResult> ApproveLog(string id)
        {
            var logId = id;
            await _logService.HandleApprove(logId);

            return RedirectToAction("Details", new RouteValueDictionary(
                    new { ControllerContext = "MaterialLog", Action = "Details", Id = logId }));
        }

        [HttpGet]
        public async Task<IActionResult> ReturnItems(string id)
        {
            string logId = id;

            if (logId == null)
            {
                return NotFound();
            }
            var logDTO = await _logManager.GetMaterialLogDTO(logId);
            if (logDTO == null)
            {
                return NotFound();
            }

            var mappedLog = _logMapper.MapLogAndItemsToViewModel(logDTO);

            return View(mappedLog);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnItems(MaterialLogAndItemsViewModel incomingReturn)
        {
            var logId = incomingReturn.MaterialLog.LogId;
            try
            {
                //TODO: try to return the full model with entities from view back to here. (employee gives null)
                await _logService.HandleReturn(incomingReturn);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }

            await _userLogService.StoreLogAsync(ControllerNames.MaterialLog, ActionConst.ReturnItems, logId);

            //TODO: more readable routevalue?
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

            return View(await _logMapper.MapMaterialLogDetailViewModel(logId));
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var logId = id;

            await _logService.HandleDelete(logId);

            await _userLogService.StoreLogAsync(ControllerNames.MaterialLog, ActionConst.Delete, logId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RepairTicket(int? id)
        {
            var updatedTicket = await _repairService.HandleTicket(id, RepairTicketAction.Repair);
            return RedirectToAction("Details", new RouteValueDictionary(
                new { ControllerContext = "MaterialLog", Action = "Details", Id = updatedTicket.LogId }));
        }

        public async Task<IActionResult> DeleteTicket(int? id)
        {
            var updatedTicket = await _repairService.HandleTicket(id, RepairTicketAction.Delete);
            return RedirectToAction("Details", new RouteValueDictionary(
                new { ControllerContext = "MaterialLog", Action = "Details", Id = updatedTicket.LogId }));
        }

        public async Task<IActionResult> HistoryIndex(string id)
        {
            var logId = id;
            var logHistory = await _logManager.GetLogHistoryByLogId(logId);
            
            //TODO: exception Handling

            var mappedHistory = _logMapper.MapHistoryToIndexViewModel(logHistory);

            return View(mappedHistory);
        }

        public async Task<IActionResult> HistoryDetails(string id, int version)
        {
            var logId = id;
            var logHistoryDTO = await _logManager.GetHistoryDetails(logId, version);
            
            //TODO: exception Handling

            var mappedDetail = _logMapper.MapHistoryToDetailViewModel(logHistoryDTO);

            return View(mappedDetail);
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

        private RedirectToActionResult RedirectToDetails(string logId)
        {
            return RedirectToAction("Details", new RouteValueDictionary(
            new { ControllerContext = "MaterialLog", Action = "Details", Id = logId }));
        }
    }
}
