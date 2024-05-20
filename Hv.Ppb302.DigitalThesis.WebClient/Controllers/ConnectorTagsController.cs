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
    public class ConnectorTagsController : Controller
    {
        private readonly DigitalThesisDbContext _context;

        public ConnectorTagsController(DigitalThesisDbContext context)
        {
            _context = context;
        }

        // GET: ConnectorTags
        public async Task<IActionResult> Index()
        {
            if (!CheckAuthentication())
            {
                return RedirectToAction("Login", "Admin");
            }

            return View(await _context.ConnectorTags.ToListAsync());
        }

        // GET: ConnectorTags/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectorTag = await _context.ConnectorTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connectorTag == null)
            {
                return NotFound();
            }

            return View(connectorTag);
        }

        // GET: ConnectorTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConnectorTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ConnectorTag connectorTag)
        {
            if (ModelState.IsValid)
            {
                connectorTag.Id = Guid.NewGuid();
                _context.Add(connectorTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(connectorTag);
        }

        // GET: ConnectorTags/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectorTag = await _context.ConnectorTags.FindAsync(id);
            if (connectorTag == null)
            {
                return NotFound();
            }
            return View(connectorTag);
        }

        // POST: ConnectorTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] ConnectorTag connectorTag)
        {
            if (id != connectorTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(connectorTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConnectorTagExists(connectorTag.Id))
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
            return View(connectorTag);
        }

        // GET: ConnectorTags/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectorTag = await _context.ConnectorTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connectorTag == null)
            {
                return NotFound();
            }

            return View(connectorTag);
        }

        // POST: ConnectorTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var connectorTag = await _context.ConnectorTags.FindAsync(id);
            if (connectorTag != null)
            {
                _context.ConnectorTags.Remove(connectorTag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConnectorTagExists(Guid id)
        {
            return _context.ConnectorTags.Any(e => e.Id == id);
        }
        public bool CheckAuthentication()
        {
            return HttpContext.Session.GetString("Username") != null;
        }

    }
}
