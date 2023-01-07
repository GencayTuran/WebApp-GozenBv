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
    [Authorize]
    public class StockLogItemController : Controller
    {
        private readonly DataDbContext _context;

        public StockLogItemController(DataDbContext context)
        {
            _context = context;
        }

        // GET: StockLogItem
        public async Task<IActionResult> Index()
        {
            return View(await _context.StockLogItems.ToListAsync());
        }

        // GET: StockLogItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var StockLogItem = await _context.StockLogItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (StockLogItem == null)
            {
                return NotFound();
            }

            return View(StockLogItem);
        }

        // GET: StockLogItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StockLogItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderCode,Amount,StockId")] StockLogItem StockLogItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(StockLogItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(StockLogItem);
        }

        // GET: StockLogItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var StockLogItem = await _context.StockLogItems.FindAsync(id);
            if (StockLogItem == null)
            {
                return NotFound();
            }
            return View(StockLogItem);
        }

        // POST: StockLogItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderCode,Amount,StockId")] StockLogItem StockLogItem)
        {
            if (id != StockLogItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(StockLogItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockLogItemExists(StockLogItem.Id))
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
            return View(StockLogItem);
        }

        // GET: StockLogItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var StockLogItem = await _context.StockLogItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (StockLogItem == null)
            {
                return NotFound();
            }

            return View(StockLogItem);
        }

        // POST: StockLogItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var StockLogItem = await _context.StockLogItems.FindAsync(id);
            _context.StockLogItems.Remove(StockLogItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockLogItemExists(int id)
        {
            return _context.StockLogItems.Any(e => e.Id == id);
        }
    }
}
