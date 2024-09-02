using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class MolarMosaicsController : Controller
{
    private readonly ConnectorTagRepository _connectorTagRepo;
    private readonly MolarMosaicRepository _molarMosaicRepo;
    private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;
    private readonly AssemblageTagRepository _assemblageTagRepository;


    public MolarMosaicsController(MolarMosaicRepository molarMosaicRepo,
        ConnectorTagRepository connectorTagRepo,
        KaleidoscopeTagRepository kaleidoscopeTagRepo,
        AssemblageTagRepository assemblageTagRepository)
    {
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

    public IActionResult Create()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molarMosaicLists = _molarMosaicRepo.GetAll();
        var kaleidoscopeTagLists = _kaleidoscopeTagRepo.GetAll();
        var excludedIds = new[] { "f2d2a02b-73bb-42e4-8774-2102ef9c3102", "1ac2b7b1-c3bf-4fc3-a5fb-37c88eeb1e97" };


        var crudViewModel = new MolarMosaicCrudViewModel
        {
            Becomings = molarMosaicLists!.SelectMany(m => m.Becomings!).Distinct().ToList(),
            ConnectorTags = _connectorTagRepo.GetAll().ToSelectListItems(
                tag => tag.Id.ToString(),
                tag => tag.Name
            ),
            AssemblageTags = _assemblageTagRepository.GetAll().ToSelectListItems(
                tag => tag.Id.ToString(),
                tag => tag.Name
            ),
            KaleidoscopeTags = kaleidoscopeTagLists!
                .Where(k => !excludedIds.Contains(k.Id.ToString()))
                .ToSelectListItems(
                    tag => tag.Id.ToString(),
                    tag => tag.Name
                )
        };

        //var excludedIds = new[] { "f2d2a02b-73bb-42e4-8774-2102ef9c3102", "1ac2b7b1-c3bf-4fc3-a5fb-37c88eeb1e97" };
        //var molarMosaicList = _molarMosaicRepo.GetAll();
        //var becomingsList = molarMosaicList!.SelectMany(m => m.Becomings!).Distinct().ToList();
        //var connectorTagList = _connectorTagRepo.GetAll();
        //var kaleidoscopeTagList = _kaleidoscopeTagRepo.GetAll();
        //var assemblageTagList = _assemblageTagRepository.GetAll();

        //var becomingsSelectListItems = becomingsList
        //    .Select(b => new SelectListItem { Value = b, Text = b })
        //    .ToList();
        //var connectorsSelectList = connectorTagList!
        //    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
        //    .ToList();
        //var kaleidoscopeSelectList = kaleidoscopeTagList!
        //    .Where(k => !excludedIds.Contains(k.Id.ToString()))
        //    .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Name })
        //    .ToList();

        //ViewData["AssemblageTags"] = new SelectList(assemblageTagList, "Id", "Name");
        //ViewData["Becomings"] = becomingsSelectListItems;
        //ViewData["Connectors"] = connectorsSelectList;
        //ViewData["Kaleidoscope"] = kaleidoscopeSelectList;

        return View(crudViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MolarMosaic molarMosaic, string[] connectorTags, string[] kaleidoscopeTags, string becomings)
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

        molarMosaic.ConnectorTags ??= [];
        molarMosaic.KaleidoscopeTags ??= [];
        if (molarMosaic.Becomings.Count == 1 && molarMosaic.Becomings[0] is null )
        {
            molarMosaic.Becomings = [];
        }

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
            var connectorTagToAdd = _connectorTagRepo.Get(Guid.Parse(connectorTagId));
            if (connectorTagToAdd != null)
            {
                molarMosaic.ConnectorTags.Add(connectorTagToAdd);
            }
        }
        foreach (var kaleidoscopeTagId in kaleidoscopeTags)
        {
            var kaleidoscopeTagToAdd = _kaleidoscopeTagRepo.Get(Guid.Parse(kaleidoscopeTagId));
            if (kaleidoscopeTagToAdd != null)
            {
                molarMosaic.KaleidoscopeTags.Add(kaleidoscopeTagToAdd);
            }
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

        var excludedIds = new[] { "f2d2a02b-73bb-42e4-8774-2102ef9c3102", "1ac2b7b1-c3bf-4fc3-a5fb-37c88eeb1e97" };
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
            .Where(k => !excludedIds.Contains(k.Id.ToString()))
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
    public IActionResult Edit(Guid id, MolarMosaic molarMosaic, string[] connectorTags, string[] kaleidoscopeTags)
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
        var existingMolarMosaic = _molarMosaicRepo.Get(id);
        if (existingMolarMosaic == null)
        {
            return NotFound();
        }

        if (molarMosaic.Becomings.Count == 1 && molarMosaic.Becomings[0] is null)
        {
            molarMosaic.Becomings = [];
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
            molarMosaic.KaleidoscopeTags?.Clear();
            var newKaleidoscopeTagIds = kaleidoscopeTags.Distinct().Select(Guid.Parse).ToList();
            foreach (var tagId in newKaleidoscopeTagIds)
            {
                var tagToAdd = _kaleidoscopeTagRepo.Get(tagId);
                if (tagToAdd != null)
                {
                    molarMosaic.KaleidoscopeTags!.Add(tagToAdd);
                }
            }
        }
        if (connectorTags != null)
        {
            molarMosaic.ConnectorTags?.Clear();
            var newConnectorTagIds = connectorTags.Distinct().Select(Guid.Parse).ToList();
            foreach (var tagId in newConnectorTagIds)
            {
                var tagToAdd = _connectorTagRepo.Get(tagId);
                if (tagToAdd != null)
                {
                    molarMosaic.ConnectorTags!.Add(tagToAdd);
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

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
