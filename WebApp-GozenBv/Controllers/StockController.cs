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
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class StockController : Controller
    {
        private readonly IStockManager _manager;
        private readonly IUserLogService _userLogService;
        public StockController(IStockManager manager, IUserLogService userLogService)
        {
            _manager = manager;
            _userLogService = userLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var materials = await _manager.MapMaterials();
            List<StockViewModel> lstStockViewModel = new();

            foreach (var material in materials)
            {
                lstStockViewModel.Add(new StockViewModel
                {
                    Id = material.Id,
                    ProductName = material.ProductName,
                    ProductCode = material.ProductCode,
                    QuantityNew = material.QuantityNew,
                    MinQuantity = material.MinQuantity,
                    QuantityUsed = material.QuantityUsed,
                    NoReturn = material.NoReturn,
                    Cost = material.Cost,
                    TotalQty = material.QuantityNew + material.QuantityUsed
                });
            }

            return View(lstStockViewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _manager.MapMaterial(id);

            StockViewModel stockViewModel = new()
            {
                Id = stock.Id,
                ProductName = stock.ProductName,
                ProductCode = stock.ProductCode,
                QuantityNew = stock.QuantityNew,
                MinQuantity = stock.MinQuantity,
                QuantityUsed = stock.QuantityUsed,
                NoReturn = stock.NoReturn,
                Cost = stock.Cost,
                TotalQty = stock.QuantityNew + stock.QuantityUsed
            };

            if (stock == null)
            {
                return PartialView("_EntityNotFound");
            }

            return View(stockViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //var productBrands = _manager.MapProductBrands();

            //ViewData["ProductBrands"] = productBrands;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stock stock)
        {
            if (ModelState.IsValid)
            {
                if (stock.NoReturn)
                {
                    stock.Cost = null;
                }

                await _manager.ManageMaterial(stock, EntityOperation.Create);

                await _userLogService.CreateAsync(ControllerConst.Stock, ActionConst.Create, stock.Id.ToString());

                return RedirectToAction(nameof(Index));
            }
            return View(stock);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _manager.MapMaterial(id);
            if (stock == null)
            {
                return NotFound();
            }
            return View(stock);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _manager.ManageMaterial(stock, EntityOperation.Update);

                await _userLogService.CreateAsync(ControllerConst.Stock, ActionConst.Edit, stock.Id.ToString());
                
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ProductBrandId"] = new SelectList(_context.Set<ProductBrand>(), "Id", "Id", stock.ProductBrandId);
            return View(stock);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _manager.MapMaterial(id);

            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _manager.MapMaterial(id);
            await _manager.ManageMaterial(material, EntityOperation.Delete);

            await _userLogService.CreateAsync(ControllerConst.Stock, ActionConst.Delete, material.Id.ToString());

            return RedirectToAction(nameof(Index));
        }
    }
}
