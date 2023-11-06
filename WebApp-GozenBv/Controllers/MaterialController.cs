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
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class MaterialController : Controller
    {
        private readonly IMaterialManager _manager;
        private readonly IUserLogService _userLogService;
        public MaterialController(IMaterialManager manager, IUserLogService userLogService)
        {
            _manager = manager;
            _userLogService = userLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var materials = await _manager.GetMaterialsAsync();
            List<MaterialViewModel> lstMaterialViewModel = new();

            foreach (var material in materials)
            {
                lstMaterialViewModel.Add(new MaterialViewModel
                {
                    Id = material.Id,
                    Name = material.Name,
                    Brand = material.Brand,
                    NewQty = material.NewQty,
                    MinQty = material.MinQty,
                    UsedQty = material.UsedQty,
                    NoReturn = material.NoReturn,
                    Cost = material.Cost,
                    TotalQty = material.NewQty + material.UsedQty
                });
            }

            return View(lstMaterialViewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _manager.GetMaterialAsync(id);

            MaterialViewModel materialViewModel = new()
            {
                Id = material.Id,
                Name = material.Name,
                Brand = material.Brand,
                NewQty = material.NewQty,
                MinQty = material.MinQty,
                UsedQty = material.UsedQty,
                NoReturn = material.NoReturn,
                Cost = material.Cost,
                TotalQty = material.NewQty + material.UsedQty
            };

            if (material == null)
            {
                return PartialView("_EntityNotFound");
            }

            return View(materialViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //var productBrands = _manager.GetProductBrands();

            //ViewData["ProductBrands"] = productBrands;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material)
        {
            if (ModelState.IsValid)
            {
                if (material.NoReturn)
                {
                    material.Cost = null;
                }

                await _manager.ManageMaterial(material, EntityOperation.Create);

                await _userLogService.StoreLogAsync(ControllerNames.Material, ActionConst.Create, material.Id.ToString());

                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _manager.GetMaterialAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _manager.ManageMaterial(material, EntityOperation.Update);

                await _userLogService.StoreLogAsync(ControllerNames.Material, ActionConst.Edit, material.Id.ToString());
                
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ProductBrandId"] = new SelectList(_context.Set<ProductBrand>(), "Id", "Id", material.ProductBrandId);
            return View(material);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _manager.GetMaterialAsync(id);

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
            var material = await _manager.GetMaterialAsync(id);
            await _manager.ManageMaterial(material, EntityOperation.Delete);

            await _userLogService.StoreLogAsync(ControllerNames.Material, ActionConst.Delete, material.Id.ToString());

            return RedirectToAction(nameof(Index));
        }
    }
}
