using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class KaleidoscopeTagsController : Controller
{
    private readonly DigitalThesisDbContext _context;

    public KaleidoscopeTagsController(DigitalThesisDbContext context)
    {
        _context = context;
    }

    // GET: KaleidoscopeTags
    public async Task<IActionResult> Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        return View(await _context.KaleidoscopeTags.ToListAsync());
    }

    // GET: KaleidoscopeTags/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kaleidoscopeTag = await _context.KaleidoscopeTags
            .FirstOrDefaultAsync(m => m.Id == id);
        if (kaleidoscopeTag == null)
        {
            return NotFound();
        }

        return View(kaleidoscopeTag);
    }

    // GET: KaleidoscopeTags/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: KaleidoscopeTags/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] KaleidoscopeTag kaleidoscopeTag)
    {
        if (ModelState.IsValid)
        {
            kaleidoscopeTag.Id = Guid.NewGuid();
            _context.Add(kaleidoscopeTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(kaleidoscopeTag);
    }

    // GET: KaleidoscopeTags/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kaleidoscopeTag = await _context.KaleidoscopeTags.FindAsync(id);
        if (kaleidoscopeTag == null)
        {
            return NotFound();
        }
        return View(kaleidoscopeTag);
    }

    // POST: KaleidoscopeTags/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] KaleidoscopeTag kaleidoscopeTag)
    {
        if (id != kaleidoscopeTag.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(kaleidoscopeTag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KaleidoscopeTagExists(kaleidoscopeTag.Id))
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
        return View(kaleidoscopeTag);
    }

    // GET: KaleidoscopeTags/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kaleidoscopeTag = await _context.KaleidoscopeTags
            .FirstOrDefaultAsync(m => m.Id == id);
        if (kaleidoscopeTag == null)
        {
            return NotFound();
        }

        return View(kaleidoscopeTag);
    }

    // POST: KaleidoscopeTags/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var kaleidoscopeTag = await _context.KaleidoscopeTags.FindAsync(id);
        if (kaleidoscopeTag != null)
        {
            _context.KaleidoscopeTags.Remove(kaleidoscopeTag);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool KaleidoscopeTagExists(Guid id)
    {
        return _context.KaleidoscopeTags.Any(e => e.Id == id);
    }

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }

}
