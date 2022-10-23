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
    public class FirmaController : Controller
    {
        private readonly DataDbContext _context;

        public FirmaController(DataDbContext context)
        {
            _context = context;
        }

        // GET: Firma
        public async Task<IActionResult> Index()
        {
            return View(await _context.Firmas.ToListAsync());
        }

        // GET: Firma/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firma = await _context.Firmas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (firma == null)
            {
                return NotFound();
            }

            return View(firma);
        }

        // GET: Firma/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Firma/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirmaName")] Firma firma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(firma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(firma);
        }

        // GET: Firma/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firma = await _context.Firmas.FindAsync(id);
            if (firma == null)
            {
                return NotFound();
            }
            return View(firma);
        }

        // POST: Firma/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirmaName")] Firma firma)
        {
            if (id != firma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(firma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirmaExists(firma.Id))
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
            return View(firma);
        }

        // GET: Firma/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firma = await _context.Firmas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (firma == null)
            {
                return NotFound();
            }

            return View(firma);
        }

        // POST: Firma/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var firma = await _context.Firmas.FindAsync(id);
            _context.Firmas.Remove(firma);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FirmaExists(int id)
        {
            return _context.Firmas.Any(e => e.Id == id);
        }
    }
}
