using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabiWei.Data;
using HabiWei.Models;

namespace HabiWei.Controllers
{
    public class WeightEntriesController : Controller
    {
        private readonly HabiWeiContext _context;

        public WeightEntriesController(HabiWeiContext context)
        {
            _context = context;
        }

        // GET: WeightEntries
        public async Task<IActionResult> Index()
        {
            var weightEntries = await _context.WeightEntries.ToListAsync();
            var groupedEntries = weightEntries
                .GroupBy(e => (e.Date - weightEntries.Min(w => w.Date)).Days / 7)
                .Select(g => new
                {
                    Week = g.Key + 1,
                    AverageWeight = g.Average(e => e.Weight)
                })
                .ToList();

            ViewBag.WeeklyAverages = groupedEntries;
            return View(weightEntries);
        }
            

        // GET: WeightEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weightEntry = await _context.WeightEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weightEntry == null)
            {
                return NotFound();
            }

            return View(weightEntry);
        }

        // GET: WeightEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WeightEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Weight")] WeightEntry weightEntry)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Received Weight: {weightEntry.Weight}");
                _context.Add(weightEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weightEntry);
        }

        // GET: WeightEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weightEntry = await _context.WeightEntries.FindAsync(id);
            if (weightEntry == null)
            {
                return NotFound();
            }
            return View(weightEntry);
        }

        // POST: WeightEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Weight")] WeightEntry weightEntry)
        {
            if (id != weightEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weightEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeightEntryExists(weightEntry.Id))
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
            return View(weightEntry);
        }

        // GET: WeightEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weightEntry = await _context.WeightEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weightEntry == null)
            {
                return NotFound();
            }

            return View(weightEntry);
        }

        // POST: WeightEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weightEntry = await _context.WeightEntries.FindAsync(id);
            if (weightEntry != null)
            {
                _context.WeightEntries.Remove(weightEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeightEntryExists(int id)
        {
            return _context.WeightEntries.Any(e => e.Id == id);
        }
    }
}
