using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Controllers
{
    //[Authorize]
    public class MaterialLogItemController : Controller
    {
        private readonly DataDbContext _context;

        public MaterialLogItemController(DataDbContext context)
        {
            _context = context;
        }

        // GET: MaterialLogItem
        public async Task<IActionResult> Index()
        {
            return View(await _context.MaterialLogItems.ToListAsync());
        }

        // GET: MaterialLogItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var MaterialLogItem = await _context.MaterialLogItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (MaterialLogItem == null)
            {
                return NotFound();
            }

            return View(MaterialLogItem);
        }

        // GET: MaterialLogItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MaterialLogItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderCode,Amount,MaterialId")] MaterialLogItem MaterialLogItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(MaterialLogItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(MaterialLogItem);
        }

        // GET: MaterialLogItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var MaterialLogItem = await _context.MaterialLogItems.FindAsync(id);
            if (MaterialLogItem == null)
            {
                return NotFound();
            }
            return View(MaterialLogItem);
        }

        // POST: MaterialLogItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderCode,Amount,MaterialId")] MaterialLogItem MaterialLogItem)
        {
            if (id != MaterialLogItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(MaterialLogItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialLogItemExists(MaterialLogItem.Id))
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
            return View(MaterialLogItem);
        }

        // GET: MaterialLogItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var MaterialLogItem = await _context.MaterialLogItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (MaterialLogItem == null)
            {
                return NotFound();
            }

            return View(MaterialLogItem);
        }

        // POST: MaterialLogItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var MaterialLogItem = await _context.MaterialLogItems.FindAsync(id);
            _context.MaterialLogItems.Remove(MaterialLogItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialLogItemExists(int id)
        {
            return _context.MaterialLogItems.Any(e => e.Id == id);
        }
    }
}
