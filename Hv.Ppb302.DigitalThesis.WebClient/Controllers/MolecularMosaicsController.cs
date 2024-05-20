using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class MolecularMosaicsController : Controller
{
    private readonly DigitalThesisDbContext _context;
    private readonly GeoTagRepository _geoTagRepo;
    private readonly ConnectorTagRepository _connectorTagRepo;
    private readonly MolarMosaicRepository _molarMosaicRepo;
    private readonly MolecularMosaicRepository _molecularMosaicRepo;
    private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;
    private readonly AssemblageTagRepository _assemblageTagRepository;


    public MolecularMosaicsController(DigitalThesisDbContext context, GeoTagRepository geoTagRepo,
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

    // GET: MolecularMosaics
    public async Task<IActionResult> Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        return View(await _context.MolecularMosaics.ToListAsync());
    }

    // GET: MolecularMosaics/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var molecularMosaic = await _context.MolecularMosaics
            .FirstOrDefaultAsync(m => m.Id == id);
        if (molecularMosaic == null)
        {
            return NotFound();
        }

        return View(molecularMosaic);
    }

    // GET: MolecularMosaics/Create
    public IActionResult Create()
    {
        List<MolecularMosaic> molecularMosaic = _molecularMosaicRepo.GetAll();
        var becomingsList = new List<string>(); // Replace this with the actual list of strings you want to display

        becomingsList = molecularMosaic.SelectMany(m => m.Becomings).Distinct().ToList();
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

    // POST: MolecularMosaics/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create( MolecularMosaic molecularMosaic, string[] ConnectorTags, string[] KaleidoscopeTags, string Becomings)
    {
        if (ModelState.IsValid)
        {

            molecularMosaic.ConnectorTags ??= [];
            molecularMosaic.KaleidoscopeTags ??= [];

            if (!System.String.IsNullOrEmpty(Becomings))
            {
                molecularMosaic.Becomings ??= [];

                List<ValueContainer>? data = JsonSerializer.Deserialize<List<ValueContainer>>(Becomings);

                // Extract the "value" field from each object and collect into a list
                List<string> valuesList = [];
                valuesList.AddRange(from item in data
                                    select item.value);

                molecularMosaic.Becomings = valuesList;


            }

            foreach (var connectorTagId in ConnectorTags)
            {
                var connectorTag = new ConnectorTag { Id = Guid.Parse(connectorTagId) };
                _context.Attach(connectorTag);
                molecularMosaic.ConnectorTags.Add(connectorTag);
            }

            foreach (var kaleidoscopeId in KaleidoscopeTags)
            {
                var kaleidoscopeTag = new KaleidoscopeTag { Id = Guid.Parse(kaleidoscopeId) };
                _context.Attach(kaleidoscopeTag);
                molecularMosaic.KaleidoscopeTags.Add(kaleidoscopeTag);
            }
            molecularMosaic.Id = Guid.NewGuid();
            _context.Add(molecularMosaic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(molecularMosaic);
    }

    // GET: MolecularMosaics/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var molecularMosaic = await _context.MolecularMosaics.FindAsync(id);
        if (molecularMosaic == null)
        {
            return NotFound();
        }

        List<MolecularMosaic> molecularMosaics = _molecularMosaicRepo.GetAll();
        var becomingsList = new List<string>(); // Replace this with the actual list of strings you want to display

        becomingsList = molecularMosaics.SelectMany(m => m.Becomings).Distinct().ToList();
        var connectors = _connectorTagRepo.GetAll();
        var Kaleidoscope = _kaleidoscopeTagRepo.GetAll();

        var becomingsSelectListItems = becomingsList.Select(b => new SelectListItem { Value = b, Text = b }).ToList();
        var connectorsSelectList = connectors
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = molecularMosaic.ConnectorTags.Any(ct => ct.Id == c.Id) })
            .ToList();
        var kaleidoscopeSelectList = Kaleidoscope
            .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Name, Selected = molecularMosaic.KaleidoscopeTags.Any(ct => ct.Id == k.Id) })
            .ToList();

        var s = _assemblageTagRepository.GetAll();
        ViewData["AssemblageTags"] = new SelectList(s, "Id", "Name", molecularMosaic.AssemblageTagId);
        ViewData["Becomings"] = becomingsSelectListItems;
        ViewData["Connectors"] = connectorsSelectList;
        ViewData["Kaleidoscope"] = kaleidoscopeSelectList;


        return View(molecularMosaic);
    }

    // POST: MolecularMosaics/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MolecularMosaic molecularMosaic, string[] ConnectorTags, string[] KaleidoscopeTags)
    {
        if (id != molecularMosaic.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Update ConnectorTags collection
                var existingMolecularMosaic = await _context.MolecularMosaics
                    .Include(m => m.ConnectorTags)
                    .Include(m => m.KaleidoscopeTags)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (existingMolecularMosaic == null)
                {
                    return NotFound();
                }

                if (molecularMosaic.Becomings != null && molecularMosaic.Becomings.Count > 0 && !string.IsNullOrEmpty(molecularMosaic.Becomings[0]?.Trim()))
                {
                    molecularMosaic.Becomings ??= [];
                    
                    List<ValueContainer>? data = JsonSerializer.Deserialize<List<ValueContainer>>(molecularMosaic.Becomings[0]);

                    // Extract the "value" field from each object and collect into a list
                    List<string> valuesList = [];
                    valuesList.AddRange(from item in data
                                        select item.value);

                    molecularMosaic.Becomings = valuesList;


                }

                if (KaleidoscopeTags != null)
                {
                    var newKaleidoscopeTagIds = KaleidoscopeTags.Distinct().Select(Guid.Parse).ToList();
                    var existingKaleidoscopeTagIds = existingMolecularMosaic.KaleidoscopeTags.Select(t => t.Id).ToList();

                    var kaleidoscopeTagsToAdd = newKaleidoscopeTagIds.Except(existingKaleidoscopeTagIds).ToList();
                    var kaleidoscopeTagsToRemove = existingKaleidoscopeTagIds.Except(newKaleidoscopeTagIds).ToList();

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // Remove old Kaleidoscope tags
                            foreach (var tagId in kaleidoscopeTagsToRemove)
                            {
                                var tagToRemove = existingMolecularMosaic.KaleidoscopeTags.FirstOrDefault(t => t.Id == tagId);
                                if (tagToRemove != null)
                                {
                                    existingMolecularMosaic.KaleidoscopeTags.Remove(tagToRemove);
                                }
                            }

                            // Add new Kaleidoscope tags
                            foreach (var tagId in kaleidoscopeTagsToAdd)
                            {
                                var tagToAdd = await _context.KaleidoscopeTags.FindAsync(tagId);
                                if (tagToAdd != null)
                                {
                                    existingMolecularMosaic.KaleidoscopeTags.Add(tagToAdd);
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
                    var existingConnectorTagIds = existingMolecularMosaic.ConnectorTags.Select(t => t.Id).ToList();

                    var connectorTagsToAdd = newConnectorTagIds.Except(existingConnectorTagIds).ToList();
                    var connectorTagsToRemove = existingConnectorTagIds.Except(newConnectorTagIds).ToList();

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // Remove old Connector tags
                            foreach (var tagId in connectorTagsToRemove)
                            {
                                var tagToRemove = existingMolecularMosaic.ConnectorTags.FirstOrDefault(t => t.Id == tagId);
                                if (tagToRemove != null)
                                {
                                    existingMolecularMosaic.ConnectorTags.Remove(tagToRemove);
                                }
                            }

                            // Add new Connector tags
                            foreach (var tagId in connectorTagsToAdd)
                            {
                                var tagToAdd = await _context.ConnectorTags.FindAsync(tagId);
                                if (tagToAdd != null)
                                {
                                    existingMolecularMosaic.ConnectorTags.Add(tagToAdd);
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
                existingMolecularMosaic.Title = molecularMosaic.Title;
                existingMolecularMosaic.Content = molecularMosaic.Content;
                existingMolecularMosaic.PdfFilePath = molecularMosaic.PdfFilePath;
                existingMolecularMosaic.AudioFilePath = molecularMosaic.AudioFilePath;
                existingMolecularMosaic.Becomings = molecularMosaic.Becomings;
                existingMolecularMosaic.AssemblageTagId = molecularMosaic.AssemblageTagId;

                _context.Update(existingMolecularMosaic);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MolecularMosaicExists(molecularMosaic.Id))
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
        return View(molecularMosaic);
    }

    // GET: MolecularMosaics/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var molecularMosaic = await _context.MolecularMosaics
            .FirstOrDefaultAsync(m => m.Id == id);
        if (molecularMosaic == null)
        {
            return NotFound();
        }

        return View(molecularMosaic);
    }

    // POST: MolecularMosaics/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var molecularMosaic = await _context.MolecularMosaics.FindAsync(id);
        if (molecularMosaic != null)
        {
            _context.MolecularMosaics.Remove(molecularMosaic);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MolecularMosaicExists(Guid id)
    {
        return _context.MolecularMosaics.Any(e => e.Id == id);
    }
    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }

    public class ValueContainer
    {
        public string value { get; set; }
    }
}
