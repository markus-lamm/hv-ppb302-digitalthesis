using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class KaleidoscopeTagsController : Controller
{
    private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;

    public KaleidoscopeTagsController(KaleidoscopeTagRepository kaleidoscopeTagRepo)
    {
        _kaleidoscopeTagRepo = kaleidoscopeTagRepo;
    }

    public IActionResult Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(_kaleidoscopeTagRepo.GetAll());
    }

    public IActionResult Details(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var kaleidoscopeTag = _kaleidoscopeTagRepo.Get(id);
        if (kaleidoscopeTag == null)
        {
            return NotFound();
        }

        return View(kaleidoscopeTag);
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var kaleidoscopeTag = _kaleidoscopeTagRepo.Get(id);
        if (kaleidoscopeTag == null)
        {
            return NotFound();
        }

        return View(kaleidoscopeTag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, [Bind("Id,Name")] KaleidoscopeTag kaleidoscopeTag)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != kaleidoscopeTag.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(kaleidoscopeTag);
        }
        _kaleidoscopeTagRepo.Update(kaleidoscopeTag);

        return RedirectToAction(nameof(Index));
    }

    private bool KaleidoscopeTagExists(Guid id) => _kaleidoscopeTagRepo.Get(id) != null;

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
