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
using static Hv.Ppb302.DigitalThesis.WebClient.Controllers.MolecularMosaicsController;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class MolarMosaicsController : Controller
    {
        private readonly DigitalThesisDbContext _context;
        private readonly GeoTagRepository _geoTagRepo;
        private readonly ConnectorTagRepository _connectorTagRepo;
        private readonly MolarMosaicRepository _molarMosaicRepo;
        private readonly MolecularMosaicRepository _molecularMosaicRepo;
        private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;

        public MolarMosaicsController(DigitalThesisDbContext context, GeoTagRepository geoTagRepo,
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

        // GET: MolarMosaics
        public async Task<IActionResult> Index()
        {
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
                                        select item.value);

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
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
            var kaleidoscopeSelectList = Kaleidoscope
                .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Name })
                .ToList();


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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Content,Becomings,PdfFilePath,HasAudio,AudioFilePath")] MolarMosaic molarMosaic, string[] ConnectorTags, string[] KaleidoscopeTags)
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
                    var MolarMosaics = await _context.MolarMosaics
                        .Include(m => m.ConnectorTags)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (MolarMosaics == null)
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
                                            select item.value);

                        molarMosaic.Becomings = valuesList;
                    }
                    if (ConnectorTags != null)
                    {
                        MolarMosaics.ConnectorTags.Clear();
                        foreach (var tagId in ConnectorTags)
                        {
                            var connectorTag = await _context.ConnectorTags.FindAsync(Guid.Parse(tagId));
                            if (connectorTag != null)
                            {
                                MolarMosaics.ConnectorTags.Add(connectorTag);
                            }
                        }
                    }
                    if (KaleidoscopeTags != null)
                    {
                        MolarMosaics.KaleidoscopeTags.Clear();
                        foreach (var tagId in KaleidoscopeTags)
                        {
                            var kaleidoscopeTags = await _context.KaleidoscopeTags.FindAsync(Guid.Parse(tagId));
                            if (kaleidoscopeTags != null)
                            {
                                MolarMosaics.KaleidoscopeTags.Add(kaleidoscopeTags);
                            }
                        }
                    }

                    // Update other properties if necessary
                    MolarMosaics.Title = molarMosaic.Title;
                    MolarMosaics.Content = molarMosaic.Content;
                    MolarMosaics.PdfFilePath = molarMosaic.PdfFilePath;
                    MolarMosaics.HasAudio = molarMosaic.HasAudio;
                    MolarMosaics.AudioFilePath = molarMosaic.AudioFilePath;
                    MolarMosaics.Becomings = molarMosaic.Becomings;

                    _context.Update(MolarMosaics);
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
    }
}
