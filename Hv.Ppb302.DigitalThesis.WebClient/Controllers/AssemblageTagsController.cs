using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class AssemblageTagsController : Controller
    {
        private readonly DigitalThesisDbContext _context;

        public AssemblageTagsController(DigitalThesisDbContext context)
        {
            _context = context;
        }

        // GET: AssemblageTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.AssemblageTags.ToListAsync());
        }

        // GET: AssemblageTags/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assemblageTag = await _context.AssemblageTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assemblageTag == null)
            {
                return NotFound();
            }

            return View(assemblageTag);
        }

        // GET: AssemblageTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AssemblageTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] AssemblageTag assemblageTag)
        {
            if (ModelState.IsValid)
            {
                assemblageTag.Id = Guid.NewGuid();
                _context.Add(assemblageTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assemblageTag);
        }

        // GET: AssemblageTags/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assemblageTag = await _context.AssemblageTags.FindAsync(id);
            if (assemblageTag == null)
            {
                return NotFound();
            }
            return View(assemblageTag);
        }

        // POST: AssemblageTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] AssemblageTag assemblageTag)
        {
            if (id != assemblageTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assemblageTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssemblageTagExists(assemblageTag.Id))
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
            return View(assemblageTag);
        }

        // GET: AssemblageTags/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assemblageTag = await _context.AssemblageTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assemblageTag == null)
            {
                return NotFound();
            }

            return View(assemblageTag);
        }

        // POST: AssemblageTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var assemblageTag = await _context.AssemblageTags.FindAsync(id);
            if (assemblageTag != null)
            {
                _context.AssemblageTags.Remove(assemblageTag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssemblageTagExists(Guid id)
        {
            return _context.AssemblageTags.Any(e => e.Id == id);
        }
    }
}
