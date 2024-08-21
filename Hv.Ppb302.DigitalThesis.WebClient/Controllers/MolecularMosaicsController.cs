using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class MolecularMosaicsController : Controller
{
    private readonly ConnectorTagRepository _connectorTagRepo;
    private readonly MolecularMosaicRepository _molecularMosaicRepo;
    private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;
    private readonly AssemblageTagRepository _assemblageTagRepository;


    public MolecularMosaicsController(MolecularMosaicRepository molecularMosaicRepo,
        ConnectorTagRepository connectorTagRepo,
        KaleidoscopeTagRepository kaleidoscopeTagRepo,
        AssemblageTagRepository assemblageTagRepository)
    {
        _molecularMosaicRepo = molecularMosaicRepo;
        _connectorTagRepo = connectorTagRepo;
        _kaleidoscopeTagRepo = kaleidoscopeTagRepo;
        _assemblageTagRepository = assemblageTagRepository;
    }

    public IActionResult Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(_molecularMosaicRepo.GetAll());
    }

    public IActionResult Create()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molecularMosaicList = _molecularMosaicRepo.GetAll();
        var becomingsList = molecularMosaicList!.SelectMany(m => m.Becomings!).Distinct().ToList();
        var connectorTagList = _connectorTagRepo.GetAll();
        var kaleidoscopeTagList = _kaleidoscopeTagRepo.GetAll();
        var assemblageTagList = _assemblageTagRepository.GetAll();

        var becomingsSelectListItems = becomingsList
            .Select(b => new SelectListItem { Value = b, Text = b })
            .ToList();
        var connectorsSelectList = connectorTagList!
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();
        var kaleidoscopeSelectList = kaleidoscopeTagList!
            .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Name })
            .ToList();

        ViewData["AssemblageTags"] = new SelectList(assemblageTagList, "Id", "Name");
        ViewData["Becomings"] = becomingsSelectListItems;
        ViewData["Connectors"] = connectorsSelectList;
        ViewData["Kaleidoscope"] = kaleidoscopeSelectList;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MolecularMosaic molecularMosaic, string[] connectorTags, string[] kaleidoscopeTags, string becomings)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        ModelState.Remove("Becomings"); // Find a better way to handle this
        if (!ModelState.IsValid)
        {
            RedirectToAction(nameof(Index));
        }

        molecularMosaic.ConnectorTags ??= [];
        molecularMosaic.KaleidoscopeTags ??= [];

        if (!string.IsNullOrEmpty(becomings))
        {
            molecularMosaic.Becomings ??= [];

            List<ValueContainer>? data = JsonSerializer.Deserialize<List<ValueContainer>>(becomings);

            // Extract the "value" field from each object and collect into a list
            List<string> valuesList = [];
            valuesList.AddRange(from item in data select item.Value);
            molecularMosaic.Becomings = valuesList;
        }
        foreach (var connectorTagId in connectorTags)
        {
            var connectorTagToAdd = _connectorTagRepo.Get(Guid.Parse(connectorTagId));
            if (connectorTagToAdd != null)
            {
                molecularMosaic.ConnectorTags.Add(connectorTagToAdd);
            }
        }
        foreach (var kaleidoscopeTagId in kaleidoscopeTags)
        {
            var kaleidoscopeTagToAdd = _kaleidoscopeTagRepo.Get(Guid.Parse(kaleidoscopeTagId));
            if (kaleidoscopeTagToAdd != null)
            {
                molecularMosaic.KaleidoscopeTags.Add(kaleidoscopeTagToAdd);
            }
        }
        _molecularMosaicRepo.Create(molecularMosaic);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molecularMosaic = _molecularMosaicRepo.Get(id);
        if (molecularMosaic == null)
        {
            return NotFound();
        }

        var molecularMosaicList = _molecularMosaicRepo.GetAll();
        var becomingsList = molecularMosaicList!.SelectMany(m => m.Becomings!).Distinct().ToList();
        var connectorTagList = _connectorTagRepo.GetAll();
        var kaleidoscopeTagList = _kaleidoscopeTagRepo.GetAll();
        var assemblageTagList = _assemblageTagRepository.GetAll();

        var becomingsSelectListItems = becomingsList
            .Select(b => new SelectListItem { Value = b, Text = b })
            .ToList();
        var connectorsSelectList = connectorTagList!
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = molecularMosaic.ConnectorTags!.Any(ct => ct.Id == c.Id) })
            .ToList();
        var kaleidoscopeSelectList = kaleidoscopeTagList!
            .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Name, Selected = molecularMosaic.KaleidoscopeTags!.Any(ct => ct.Id == k.Id) })
            .ToList();

        ViewData["AssemblageTags"] = new SelectList(assemblageTagList, "Id", "Name", molecularMosaic.AssemblageTagId);
        ViewData["Becomings"] = becomingsSelectListItems;
        ViewData["Connectors"] = connectorsSelectList;
        ViewData["Kaleidoscope"] = kaleidoscopeSelectList;

        return View(molecularMosaic);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, MolecularMosaic molecularMosaic, string[] connectorTags, string[] kaleidoscopeTags)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != molecularMosaic.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(molecularMosaic);
        }

        // Update ConnectorTags collection
        var existingMolecularMosaic = _molecularMosaicRepo.Get(id);
        if (existingMolecularMosaic == null)
        {
            return NotFound();
        }

        if (molecularMosaic.Becomings != null && molecularMosaic.Becomings.Count > 0 && !string.IsNullOrEmpty(molecularMosaic.Becomings[0]?.Trim()))
        {
            molecularMosaic.Becomings ??= [];

            var data = JsonSerializer.Deserialize<List<ValueContainer>>(molecularMosaic.Becomings[0]);

            // Extract the "value" field from each object and collect into a list
            List<string> valuesList = [];
            valuesList.AddRange(from item in data select item.Value);
            molecularMosaic.Becomings = valuesList;
        }
        if (kaleidoscopeTags != null)
        {
            molecularMosaic.KaleidoscopeTags?.Clear();
            var newKaleidoscopeTagIds = kaleidoscopeTags.Distinct().Select(Guid.Parse).ToList();
            foreach (var tagId in newKaleidoscopeTagIds)
            {
                var tagToAdd = _kaleidoscopeTagRepo.Get(tagId);
                if (tagToAdd != null)
                {
                    molecularMosaic.KaleidoscopeTags!.Add(tagToAdd);
                }
            }
        }
        if (connectorTags != null)
        {
            molecularMosaic.ConnectorTags?.Clear();
            var newConnectorTagIds = connectorTags.Distinct().Select(Guid.Parse).ToList();
            foreach (var tagId in newConnectorTagIds)
            {
                var tagToAdd = _connectorTagRepo.Get(tagId);
                if (tagToAdd != null)
                {
                    molecularMosaic.ConnectorTags!.Add(tagToAdd);
                }
            }
        }
        _molecularMosaicRepo.Update(molecularMosaic);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molecularMosaic = _molecularMosaicRepo.Get(id);
        if (molecularMosaic == null)
        {
            return NotFound();
        }

        return View(molecularMosaic);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molecularMosaic = _molecularMosaicRepo.Get(id);
        if (molecularMosaic == null)
        {
            return NotFound();
        }
        _molecularMosaicRepo.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
