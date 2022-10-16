using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Controllers
{
    public class ProductBrandController : Controller
    {
        private readonly DataDbContext _context;

        public ProductBrandController(DataDbContext context)
        {
            _context = context;
        }

        // GET: ProductBrand
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductBrands.ToListAsync());
        }

        // GET: ProductBrand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrand = await _context.ProductBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productBrand == null)
            {
                return NotFound();
            }

            return View(productBrand);
        }

        // GET: ProductBrand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductBrand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ProductBrand productBrand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productBrand);
        }

        // GET: ProductBrand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrand = await _context.ProductBrands.FindAsync(id);
            if (productBrand == null)
            {
                return NotFound();
            }
            return View(productBrand);
        }

        // POST: ProductBrand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ProductBrand productBrand)
        {
            if (id != productBrand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productBrand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductBrandExists(productBrand.Id))
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
            return View(productBrand);
        }

        // GET: ProductBrand/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrand = await _context.ProductBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productBrand == null)
            {
                return NotFound();
            }

            return View(productBrand);
        }

        // POST: ProductBrand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productBrand = await _context.ProductBrands.FindAsync(id);
            _context.ProductBrands.Remove(productBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductBrandExists(int id)
        {
            return _context.ProductBrands.Any(e => e.Id == id);
        }
    }
}
