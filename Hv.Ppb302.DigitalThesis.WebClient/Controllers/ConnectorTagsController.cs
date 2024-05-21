using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class ConnectorTagsController : Controller
{
    private readonly ConnectorTagRepository _connectorTagRepo;

    public ConnectorTagsController(ConnectorTagRepository connectorTagRepo)
    {
        _connectorTagRepo = connectorTagRepo;
    }

    public IActionResult Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(_connectorTagRepo.GetAll());
    }

    public IActionResult Details(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var connectorTag = _connectorTagRepo.Get(id);
        if (connectorTag == null)
        {
            return NotFound();
        }

        return View(connectorTag);
    }

    public IActionResult Create()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,Name")] ConnectorTag connectorTag)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (!ModelState.IsValid)
        {
            return View(connectorTag);
        }
        _connectorTagRepo.Create(connectorTag);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var connectorTag = _connectorTagRepo.Get(id);
        if (connectorTag == null)
        {
            return NotFound();
        }

        return View(connectorTag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, [Bind("Id,Name")] ConnectorTag connectorTag)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != connectorTag.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(connectorTag);
        }
        _connectorTagRepo.Update(connectorTag);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var connectorTag = _connectorTagRepo.Get(id);
        if (connectorTag == null)
        {
            return NotFound();
        }

        return View(connectorTag);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var connectorTag = _connectorTagRepo.Get(id);
        if (connectorTag == null)
        {
            return NotFound();
        }
        _connectorTagRepo.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    private bool ConnectorTagExists(Guid id) => _connectorTagRepo.Get(id) != null;

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
