using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using static Hv.Ppb302.DigitalThesis.WebClient.Controllers.MolecularMosaicsController;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class GeoTagsController : Controller
{
    private readonly GeoTagRepository _geoTagRepo;
    private readonly ConnectorTagRepository _connectorTagRepo;

    public GeoTagsController(GeoTagRepository geoTagRepo, ConnectorTagRepository connectorTagRepo)
    {
        _geoTagRepo = geoTagRepo;
        _connectorTagRepo = connectorTagRepo;
    }

    public IActionResult Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(_geoTagRepo.GetAll());
    }

    public IActionResult Details(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var geoTag = _geoTagRepo.Get(id);
        if (geoTag == null)
        {
            return NotFound();
        }

        return View(geoTag);
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var geoTag = _geoTagRepo.Get(id);
        if (geoTag == null)
        {
            return NotFound();
        }

        var geoTagList = _geoTagRepo.GetAll();
        var connectorTagList = _connectorTagRepo.GetAll();
        var becomingsList = geoTagList!.SelectMany(m => m.Becomings!).Distinct().ToList();

        var becomingsSelectListItems = becomingsList
            .Select(b => new SelectListItem { Value = b, Text = b })
            .ToList();
        var connectorsSelectList = connectorTagList!
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();

        ViewData["Becomings"] = becomingsSelectListItems;
        ViewData["Connectors"] = connectorsSelectList;

        return View(geoTag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, [Bind("Id,Title,Content,Becomings,PdfFilePath,AudioFilePath")] GeoTag geoTagInput, string[] connectorTags)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != geoTagInput.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(geoTagInput);
        }

        var geoTagDb = _geoTagRepo.Get(id);
        if (geoTagDb == null)
        {
            return NotFound();
        }

        // Unsure if GeoTags are supposed to have becomings and connectors
        if (geoTagInput.Becomings != null && geoTagInput.Becomings.Count > 0 && !string.IsNullOrEmpty(geoTagInput.Becomings[0]?.Trim()))
        {
            geoTagDb.Becomings ??= [];

            var data = JsonSerializer.Deserialize<List<ValueContainer>>(geoTagInput.Becomings[0]);

            // Extract the "value" field from each object and collect into a list
            List<string> valuesList = [];
            valuesList.AddRange(from item in data select item.value);

            geoTagDb.Becomings = valuesList;
        }
        //if (connectorTags != null)
        //{
        //    geoTagDb.ConnectorTags!.Clear();
        //    foreach (var tagId in connectorTags)
        //    {
        //        var connectorTag = await _context.ConnectorTags.FindAsync(Guid.Parse(tagId));
        //        if (connectorTag != null)
        //        {
        //            geoTagDb.ConnectorTags.Add(connectorTag);
        //        }
        //    }
        //}
        _geoTagRepo.Update(geoTagInput);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var geoTag = _geoTagRepo.Get(id);
        if (geoTag == null)
        {
            return NotFound();
        }

        return View(geoTag);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var geoTag = _geoTagRepo.Get(id);
        if (geoTag == null)
        {
            return NotFound();
        }
        _geoTagRepo.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    private bool GeoTagExists(Guid id) => _geoTagRepo.Get(id) != null;

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
