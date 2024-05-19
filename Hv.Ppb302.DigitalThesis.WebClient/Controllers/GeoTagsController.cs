using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using static Hv.Ppb302.DigitalThesis.WebClient.Controllers.MolecularMosaicsController;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class GeoTagsController : Controller
    {
        private readonly DigitalThesisDbContext _context;
        private readonly GeoTagRepository _geoTagRepo;
        private readonly ConnectorTagRepository _connectorTagRepo;

        public GeoTagsController(DigitalThesisDbContext context, GeoTagRepository geoTagRepo
            , ConnectorTagRepository connectorTagRepo)
        {
            _context = context;
            _geoTagRepo = geoTagRepo;
            _connectorTagRepo = connectorTagRepo;
        }

        // GET: GeoTags
        public async Task<IActionResult> Index()
        {
            if (!CheckAuthentication())
            {
                return RedirectToAction("Login", "Admin");
            }

            return View(await _context.GeoTags.ToListAsync());
        }

        // GET: GeoTags/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geoTag = await _context.GeoTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geoTag == null)
            {
                return NotFound();
            }

            return View(geoTag);
        }

        // GET: GeoTags/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geoTag = await _context.GeoTags.FindAsync(id);
            if (geoTag == null)
            {
                return NotFound();
            }

            List<GeoTag> geoTagMosaic = _geoTagRepo.GetAll();
            var becomingsList = new List<string>();

            becomingsList = geoTagMosaic.SelectMany(m => m.Becomings).Distinct().ToList();
            var connectors = _connectorTagRepo.GetAll();

            var becomingsSelectListItems = becomingsList.Select(b => new SelectListItem { Value = b, Text = b }).ToList();
            var connectorsSelectList = connectors
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            ViewData["Becomings"] = becomingsSelectListItems;
            ViewData["Connectors"] = connectorsSelectList;

            return View(geoTag);
        }

        // POST: GeoTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Content,Becomings,PdfFilePath,HasAudio,AudioFilePath")] GeoTag geoTag, string[] ConnectorTags)
        {
            if (id != geoTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var GeoTag = await _context.GeoTags
                        .Include(m => m.ConnectorTags)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (GeoTag == null)
                    {
                        return NotFound();
                    }

                    if (geoTag.Becomings != null && geoTag.Becomings.Count > 0 && !string.IsNullOrEmpty(geoTag.Becomings[0]?.Trim()))
                    {
                        GeoTag.Becomings ??= [];

                        List<ValueContainer>? data = JsonSerializer.Deserialize<List<ValueContainer>>(geoTag.Becomings[0]);

                        // Extract the "value" field from each object and collect into a list
                        List<string> valuesList = [];
                        valuesList.AddRange(from item in data
                                            select item.value);

                        GeoTag.Becomings = valuesList;


                    }
                    if (ConnectorTags != null)
                    {
                        GeoTag.ConnectorTags.Clear();
                        foreach (var tagId in ConnectorTags)
                        {
                            var connectorTag = await _context.ConnectorTags.FindAsync(Guid.Parse(tagId));
                            if (connectorTag != null)
                            {
                                GeoTag.ConnectorTags.Add(connectorTag);
                            }
                        }
                    }

                    GeoTag.Title = geoTag.Title;
                    GeoTag.Content = geoTag.Content;
                    GeoTag.PdfFilePath = geoTag.PdfFilePath;
                    GeoTag.HasAudio = geoTag.HasAudio;
                    GeoTag.AudioFilePath = geoTag.AudioFilePath;
                    GeoTag.Becomings = geoTag.Becomings;

                    _context.Update(GeoTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeoTagExists(geoTag.Id))
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
            return View(geoTag);
        }

        // GET: GeoTags/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geoTag = await _context.GeoTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geoTag == null)
            {
                return NotFound();
            }

            return View(geoTag);
        }

        // POST: GeoTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var geoTag = await _context.GeoTags.FindAsync(id);
            if (geoTag != null)
            {
                _context.GeoTags.Remove(geoTag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeoTagExists(Guid id)
        {
            return _context.GeoTags.Any(e => e.Id == id);
        }
        public bool CheckAuthentication()
        {
            return HttpContext.Session.GetString("Username") != null;
        }

    }
}
