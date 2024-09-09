using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class KaleidoscopeTagsController(KaleidoscopeTagRepository kaleidoscopeTagRepo) : Controller
{
    public IActionResult Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(kaleidoscopeTagRepo.GetAll());
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var kaleidoscopeTag = kaleidoscopeTagRepo.Get(id);
        if (kaleidoscopeTag == null)
        {
            return NotFound();
        }

        return View(kaleidoscopeTag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, [Bind("Id,Name,Content")] KaleidoscopeTag kaleidoscopeTag)
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
        kaleidoscopeTagRepo.Update(kaleidoscopeTag);

        return RedirectToAction(nameof(Index));
    }

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
