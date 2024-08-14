using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

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

        return View(geoTag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, [Bind("Id,Title,Content,PdfFilePath,AudioFilePath,IsVisible")] GeoTag geoTag)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != geoTag.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(geoTag);
        }
        _geoTagRepo.Update(geoTag);

        return RedirectToAction(nameof(Index));
    }

    private bool GeoTagExists(Guid id) => _geoTagRepo.Get(id) != null;

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
