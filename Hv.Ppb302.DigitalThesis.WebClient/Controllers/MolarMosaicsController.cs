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
    private readonly ConnectorTagRepository _connectorTagRepo;
    private readonly MolarMosaicRepository _molarMosaicRepo;
    private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;
    private readonly AssemblageTagRepository _assemblageTagRepository;


    public MolarMosaicsController(DigitalThesisDbContext context, 
        MolarMosaicRepository molarMosaicRepo,
        ConnectorTagRepository connectorTagRepo,
        KaleidoscopeTagRepository kaleidoscopeTagRepo,
        AssemblageTagRepository assemblageTagRepository)
    {
        _context = context;
        _molarMosaicRepo = molarMosaicRepo;
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
        return View(_molarMosaicRepo.GetAll());
    }

    public IActionResult Details(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molarMosaic = _molarMosaicRepo.Get(id);
        if (molarMosaic == null)
        {
            return NotFound();
        }

        return View(molarMosaic);
    }

    public IActionResult Create()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molarMosaicList = _molarMosaicRepo.GetAll();
        var becomingsList = molarMosaicList!.SelectMany(m => m.Becomings!).Distinct().ToList();
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
    public IActionResult Create(MolarMosaic molarMosaic, string[] connectorTags, string[] kaleidoscopeTags, string becomings)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (!ModelState.IsValid)
        {
            return View(molarMosaic);
        }

        molarMosaic.ConnectorTags ??= [];
        molarMosaic.KaleidoscopeTags ??= [];

        if (!string.IsNullOrEmpty(becomings))
        {
            molarMosaic.Becomings ??= [];

            var data = JsonSerializer.Deserialize<List<ValueContainer>>(becomings);

            // Extract the "value" field from each object and collect into a list
            List<string> valuesList = [];
            valuesList.AddRange(from item in data select item.Value);

            molarMosaic.Becomings = valuesList;
        }
        foreach (var connectorTagId in connectorTags)
        {
            var connectorTag = new ConnectorTag { Id = Guid.Parse(connectorTagId) };
            _context.Attach(connectorTag); // Try to remove this direct database call
            molarMosaic.ConnectorTags.Add(connectorTag);
        }
        foreach (var kaleidoscopeId in kaleidoscopeTags)
        {
            var kaleidoscopeTag = new KaleidoscopeTag { Id = Guid.Parse(kaleidoscopeId) };
            _context.Attach(kaleidoscopeTag); // Try to remove this direct database call
            molarMosaic.KaleidoscopeTags.Add(kaleidoscopeTag);
        }
        _molarMosaicRepo.Create(molarMosaic);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molarMosaic = _molarMosaicRepo.Get(id);
        if (molarMosaic == null)
        {
            return NotFound();
        }

        var molarMosaicList = _molarMosaicRepo.GetAll();
        var becomingsList = molarMosaicList!.SelectMany(m => m.Becomings!).Distinct().ToList();
        var connectorTagList = _connectorTagRepo.GetAll();
        var kaleidoscopeTagList = _kaleidoscopeTagRepo.GetAll();
        var assemblageTagList = _assemblageTagRepository.GetAll();

        var becomingsSelectListItems = becomingsList
            .Select(b => new SelectListItem { Value = b, Text = b })
            .ToList();
        var connectorsSelectList = connectorTagList!
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = molarMosaic.ConnectorTags!.Any(ct => ct.Id == c.Id) })
            .ToList();
        var kaleidoscopeSelectList = kaleidoscopeTagList!
            .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Name, Selected = molarMosaic.KaleidoscopeTags!.Any(ct => ct.Id == k.Id) })
            .ToList();

        ViewData["AssemblageTags"] = new SelectList(assemblageTagList, "Id", "Name", molarMosaic.AssemblageTagId);
        ViewData["Becomings"] = becomingsSelectListItems;
        ViewData["Connectors"] = connectorsSelectList;
        ViewData["Kaleidoscope"] = kaleidoscopeSelectList;

        return View(molarMosaic);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MolarMosaic molarMosaic, string[] connectorTags, string[] kaleidoscopeTags)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != molarMosaic.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(molarMosaic);
        }

        // Update ConnectorTags collection
        var existingmolarMosaic = _molarMosaicRepo.Get(id);
        if (existingmolarMosaic == null)
        {
            return NotFound();
        }

        if (molarMosaic.Becomings != null && molarMosaic.Becomings.Count > 0 && !string.IsNullOrEmpty(molarMosaic.Becomings[0]?.Trim()))
        {
            molarMosaic.Becomings ??= [];

            var data = JsonSerializer.Deserialize<List<ValueContainer>>(molarMosaic.Becomings[0]);

            // Extract the "value" field from each object and collect into a list
            List<string> valuesList = [];
            valuesList.AddRange(from item in data select item.Value);

            molarMosaic.Becomings = valuesList;
        }
        if (kaleidoscopeTags != null)
        {
            var newKaleidoscopeTagIds = kaleidoscopeTags.Distinct().Select(Guid.Parse).ToList();
            var existingKaleidoscopeTagIds = existingmolarMosaic.KaleidoscopeTags!.Select(t => t.Id).ToList();

            var kaleidoscopeTagsToAdd = newKaleidoscopeTagIds.Except(existingKaleidoscopeTagIds).ToList();
            var kaleidoscopeTagsToRemove = existingKaleidoscopeTagIds.Except(newKaleidoscopeTagIds).ToList();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Remove old Kaleidoscope tags
                    foreach (var tagId in kaleidoscopeTagsToRemove)
                    {
                        var tagToRemove = existingmolarMosaic.KaleidoscopeTags!.FirstOrDefault(t => t.Id == tagId);
                        if (tagToRemove != null)
                        {
                            existingmolarMosaic.KaleidoscopeTags!.Remove(tagToRemove);
                        }
                    }

                    // Add new Kaleidoscope tags
                    foreach (var tagId in kaleidoscopeTagsToAdd)
                    {
                        var tagToAdd = _kaleidoscopeTagRepo.Get(tagId);
                        if (tagToAdd != null)
                        {
                            existingmolarMosaic.KaleidoscopeTags!.Add(tagToAdd);
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
        if (connectorTags != null)
        {
            var newConnectorTagIds = connectorTags.Distinct().Select(Guid.Parse).ToList();
            var existingConnectorTagIds = existingmolarMosaic.ConnectorTags!.Select(t => t.Id).ToList();

            var connectorTagsToAdd = newConnectorTagIds.Except(existingConnectorTagIds).ToList();
            var connectorTagsToRemove = existingConnectorTagIds.Except(newConnectorTagIds).ToList();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Remove old Connector tags
                    foreach (var tagId in connectorTagsToRemove)
                    {
                        var tagToRemove = existingmolarMosaic.ConnectorTags!.FirstOrDefault(t => t.Id == tagId);
                        if (tagToRemove != null)
                        {
                            existingmolarMosaic.ConnectorTags!.Remove(tagToRemove);
                        }
                    }

                    // Add new Connector tags
                    foreach (var tagId in connectorTagsToAdd)
                    {
                        var tagToAdd = _connectorTagRepo.Get(tagId);
                        if (tagToAdd != null)
                        {
                            existingmolarMosaic.ConnectorTags!.Add(tagToAdd);
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
        _molarMosaicRepo.Update(molarMosaic);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molarMosaic = _molarMosaicRepo.Get(id);
        if (molarMosaic == null)
        {
            return NotFound();
        }

        return View(molarMosaic);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molarMosaic = _molarMosaicRepo.Get(id);
        if (molarMosaic == null)
        {
            return NotFound();
        }
        _molarMosaicRepo.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    private bool MolarMosaicExists(Guid id) => _molarMosaicRepo.Get(id) != null;

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
