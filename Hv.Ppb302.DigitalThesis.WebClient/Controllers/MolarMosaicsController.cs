using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class MolarMosaicsController : Controller
{
    private readonly DigitalThesisDbContext _context;
    private readonly GeoTagRepository _geoTagRepo;
    private readonly ConnectorTagRepository _connectorTagRepo;
    private readonly MolarMosaicRepository _molarMosaicRepo;
    private readonly MolecularMosaicRepository _molecularMosaicRepo;
    private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;
    private readonly AssemblageTagRepository _assemblageTagRepository;


    public MolarMosaicsController(DigitalThesisDbContext context, GeoTagRepository geoTagRepo,
        MolarMosaicRepository molarMosaicRepo,
        MolecularMosaicRepository molecularMosaicRepo,
        ConnectorTagRepository connectorTagRepo,
        KaleidoscopeTagRepository kaleidoscopeTagRepo,
        AssemblageTagRepository assemblageTagRepository)
    {
        _context = context;
        _geoTagRepo = geoTagRepo;
        _molarMosaicRepo = molarMosaicRepo;
        _molecularMosaicRepo = molecularMosaicRepo;
        _connectorTagRepo = connectorTagRepo;
        _kaleidoscopeTagRepo = kaleidoscopeTagRepo;
        _assemblageTagRepository = assemblageTagRepository;
    }

    // GET: MolarMosaics
    public async Task<IActionResult> Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        return View(await _context.MolarMosaics.ToListAsync());
    }

    // GET: MolarMosaics/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var molarMosaic = await _context.MolarMosaics
            .FirstOrDefaultAsync(m => m.Id == id);
        if (molarMosaic == null)
        {
            return NotFound();
        }

        return View(molarMosaic);
    }

    // GET: MolarMosaics/Create
    public IActionResult Create()
    {
        List<MolarMosaic> molarMosaic = _molarMosaicRepo.GetAll();
        var becomingsList = new List<string>(); // Replace this with the actual list of strings you want to display

        becomingsList = molarMosaic.SelectMany(m => m.Becomings).Distinct().ToList();
        var connectors = _connectorTagRepo.GetAll();
        var Kaleidoscope = _kaleidoscopeTagRepo.GetAll();

        var becomingsSelectListItems = becomingsList.Select(b => new SelectListItem { Value = b, Text = b }).ToList();
        var connectorsSelectList = connectors
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();
        var kaleidoscopeSelectList = Kaleidoscope
            .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Name })
            .ToList();
        var s = _assemblageTagRepository.GetAll();


        ViewData["AssemblageTags"] = new SelectList(s, "Id", "Name");
        ViewData["Becomings"] = becomingsSelectListItems;
        ViewData["Connectors"] = connectorsSelectList;
        ViewData["Kaleidoscope"] = kaleidoscopeSelectList;

        return View();
    }

    // POST: MolarMosaics/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create( MolarMosaic molarMosaic, string[] ConnectorTags, string[] KaleidoscopeTags, string Becomings)
    {
        if (ModelState.IsValid)
        {
            molarMosaic.ConnectorTags ??= [];
            molarMosaic.KaleidoscopeTags ??= [];

            if (!System.String.IsNullOrEmpty(Becomings))
            {
                molarMosaic.Becomings ??= [];

                List<ValueContainer>? data = JsonSerializer.Deserialize<List<ValueContainer>>(Becomings);

                // Extract the "value" field from each object and collect into a list
                List<string> valuesList = [];
                valuesList.AddRange(from item in data
                                    select item.Value);

                molarMosaic.Becomings = valuesList;
            }

            foreach (var connectorTagId in ConnectorTags)
            {
                var connectorTag = new ConnectorTag { Id = Guid.Parse(connectorTagId) };
                _context.Attach(connectorTag);
                molarMosaic.ConnectorTags.Add(connectorTag);
            }

            foreach (var kaleidoscopeId in KaleidoscopeTags)
            {
                var kaleidoscopeTag = new KaleidoscopeTag { Id = Guid.Parse(kaleidoscopeId) };
                _context.Attach(kaleidoscopeTag);
                molarMosaic.KaleidoscopeTags.Add(kaleidoscopeTag);
            }

            molarMosaic.Id = Guid.NewGuid();
            _context.Add(molarMosaic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(molarMosaic);
    }

    // GET: MolarMosaics/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var molarMosaic = await _context.MolarMosaics.FindAsync(id);
        if (molarMosaic == null)
        {
            return NotFound();
        }

        List<MolarMosaic> molarMosaics = _molarMosaicRepo.GetAll();
        var becomingsList = new List<string>(); // Replace this with the actual list of strings you want to display

        becomingsList = molarMosaics.SelectMany(m => m.Becomings).Distinct().ToList();
        var connectors = _connectorTagRepo.GetAll();
        var Kaleidoscope = _kaleidoscopeTagRepo.GetAll();

        var becomingsSelectListItems = becomingsList.Select(b => new SelectListItem { Value = b, Text = b }).ToList();
        var connectorsSelectList = connectors
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = molarMosaic.ConnectorTags.Any(ct => ct.Id == c.Id) })
            .ToList();
        var kaleidoscopeSelectList = Kaleidoscope
            .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Name, Selected = molarMosaic.KaleidoscopeTags.Any(ct => ct.Id == k.Id) })
            .ToList();

        var s = _assemblageTagRepository.GetAll();
        ViewData["AssemblageTags"] = new SelectList(s, "Id", "Name", molarMosaic.AssemblageTagId);
        ViewData["Becomings"] = becomingsSelectListItems;
        ViewData["Connectors"] = connectorsSelectList;
        ViewData["Kaleidoscope"] = kaleidoscopeSelectList;

        return View(molarMosaic);
    }

    // POST: MolarMosaics/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MolarMosaic molarMosaic, string[] ConnectorTags, string[] KaleidoscopeTags)
    {
        if (id != molarMosaic.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Update ConnectorTags collection
                var existingmolarMosaic = await _context.MolarMosaics
                    .Include(m => m.ConnectorTags)
                    .Include(m => m.KaleidoscopeTags)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (existingmolarMosaic == null)
                {
                    return NotFound();
                }

                if (molarMosaic.Becomings != null && molarMosaic.Becomings.Count > 0 && !string.IsNullOrEmpty(molarMosaic.Becomings[0]?.Trim()))
                {
                    molarMosaic.Becomings ??= [];

                    List<ValueContainer>? data = JsonSerializer.Deserialize<List<ValueContainer>>(molarMosaic.Becomings[0]);

                    // Extract the "value" field from each object and collect into a list
                    List<string> valuesList = [];
                    valuesList.AddRange(from item in data
                                        select item.Value);

                    molarMosaic.Becomings = valuesList;


                }

                if (KaleidoscopeTags != null)
                {
                    var newKaleidoscopeTagIds = KaleidoscopeTags.Distinct().Select(Guid.Parse).ToList();
                    var existingKaleidoscopeTagIds = existingmolarMosaic.KaleidoscopeTags.Select(t => t.Id).ToList();

                    var kaleidoscopeTagsToAdd = newKaleidoscopeTagIds.Except(existingKaleidoscopeTagIds).ToList();
                    var kaleidoscopeTagsToRemove = existingKaleidoscopeTagIds.Except(newKaleidoscopeTagIds).ToList();

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // Remove old Kaleidoscope tags
                            foreach (var tagId in kaleidoscopeTagsToRemove)
                            {
                                var tagToRemove = existingmolarMosaic.KaleidoscopeTags.FirstOrDefault(t => t.Id == tagId);
                                if (tagToRemove != null)
                                {
                                    existingmolarMosaic.KaleidoscopeTags.Remove(tagToRemove);
                                }
                            }

                            // Add new Kaleidoscope tags
                            foreach (var tagId in kaleidoscopeTagsToAdd)
                            {
                                var tagToAdd = await _context.KaleidoscopeTags.FindAsync(tagId);
                                if (tagToAdd != null)
                                {
                                    existingmolarMosaic.KaleidoscopeTags.Add(tagToAdd);
                                }
                            }

                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }

                if (ConnectorTags != null)
                {
                    var newConnectorTagIds = ConnectorTags.Distinct().Select(Guid.Parse).ToList();
                    var existingConnectorTagIds = existingmolarMosaic.ConnectorTags.Select(t => t.Id).ToList();

                    var connectorTagsToAdd = newConnectorTagIds.Except(existingConnectorTagIds).ToList();
                    var connectorTagsToRemove = existingConnectorTagIds.Except(newConnectorTagIds).ToList();

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // Remove old Connector tags
                            foreach (var tagId in connectorTagsToRemove)
                            {
                                var tagToRemove = existingmolarMosaic.ConnectorTags.FirstOrDefault(t => t.Id == tagId);
                                if (tagToRemove != null)
                                {
                                    existingmolarMosaic.ConnectorTags.Remove(tagToRemove);
                                }
                            }

                            // Add new Connector tags
                            foreach (var tagId in connectorTagsToAdd)
                            {
                                var tagToAdd = await _context.ConnectorTags.FindAsync(tagId);
                                if (tagToAdd != null)
                                {
                                    existingmolarMosaic.ConnectorTags.Add(tagToAdd);
                                }
                            }

                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }


                // Update other properties if necessary
                existingmolarMosaic.Title = molarMosaic.Title;
                existingmolarMosaic.Content = molarMosaic.Content;
                existingmolarMosaic.PdfFilePath = molarMosaic.PdfFilePath;
                existingmolarMosaic.AudioFilePath = molarMosaic.AudioFilePath;
                existingmolarMosaic.Becomings = molarMosaic.Becomings;
                existingmolarMosaic.AssemblageTagId = molarMosaic.AssemblageTagId;

                _context.Update(existingmolarMosaic);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MolarMosaicExists(molarMosaic.Id))
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
        return View(molarMosaic);
    }

    // GET: MolarMosaics/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var molarMosaic = await _context.MolarMosaics
            .FirstOrDefaultAsync(m => m.Id == id);
        if (molarMosaic == null)
        {
            return NotFound();
        }

        return View(molarMosaic);
    }

    // POST: MolarMosaics/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var molarMosaic = await _context.MolarMosaics.FindAsync(id);
        if (molarMosaic != null)
        {
            _context.MolarMosaics.Remove(molarMosaic);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MolarMosaicExists(Guid id)
    {
        return _context.MolarMosaics.Any(e => e.Id == id);
    }
    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }

}
