using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Azure;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class MolecularMosaicsController : Controller
    {
        private readonly DigitalThesisDbContext _context;
        private readonly GeoTagRepository _geoTagRepo;
        private readonly ConnectorTagRepository _connectorTagRepo;
        private readonly MolarMosaicRepository _molarMosaicRepo;
        private readonly MolecularMosaicRepository _molecularMosaicRepo;
        private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;

        public MolecularMosaicsController(DigitalThesisDbContext context, GeoTagRepository geoTagRepo,
            MolarMosaicRepository molarMosaicRepo,
            MolecularMosaicRepository molecularMosaicRepo,
            ConnectorTagRepository connectorTagRepo,
            KaleidoscopeTagRepository kaleidoscopeTagRepo)
        {
            _context = context;
            _geoTagRepo = geoTagRepo;
            _molarMosaicRepo = molarMosaicRepo;
            _molecularMosaicRepo = molecularMosaicRepo;
            _connectorTagRepo = connectorTagRepo;
            _kaleidoscopeTagRepo = kaleidoscopeTagRepo;
        }

        // GET: MolecularMosaics
        public async Task<IActionResult> Index()
        {
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

                    if (ConnectorTags != null)
                    {
                        existingMolecularMosaic.ConnectorTags.Clear();
                        foreach (var tagId in ConnectorTags)
                        {
                            var connectorTag = await _context.ConnectorTags.FindAsync(Guid.Parse(tagId));
                            if (connectorTag != null)
                            {
                                existingMolecularMosaic.ConnectorTags.Add(connectorTag);
                            }
                        }
                    }
                    if (KaleidoscopeTags != null)
                    {
                        existingMolecularMosaic.KaleidoscopeTags.Clear();
                        foreach (var tagId in KaleidoscopeTags)
                        {
                            var kaleidoscopeTags = await _context.KaleidoscopeTags.FindAsync(Guid.Parse(tagId));
                            if (kaleidoscopeTags != null)
                            {
                                existingMolecularMosaic.KaleidoscopeTags.Add(kaleidoscopeTags);
                            }
                        }
                    }

                    // Update other properties if necessary
                    existingMolecularMosaic.Title = molecularMosaic.Title;
                    existingMolecularMosaic.Content = molecularMosaic.Content;
                    existingMolecularMosaic.PdfFilePath = molecularMosaic.PdfFilePath;
                    existingMolecularMosaic.HasAudio = molecularMosaic.HasAudio;
                    existingMolecularMosaic.AudioFilePath = molecularMosaic.AudioFilePath;
                    existingMolecularMosaic.Becomings = molecularMosaic.Becomings;

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
        public class ValueContainer
        {
            public string value { get; set; }
        }
    }
}
