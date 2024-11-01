﻿using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class AssemblageTagsController(AssemblageTagRepository assemblageTagRepo) : Controller
{
    public IActionResult Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(assemblageTagRepo.GetAll());
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
    public IActionResult Create([Bind("Id,Name")] AssemblageTag assemblageTag)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (!ModelState.IsValid)
        {
            return View(assemblageTag);
        }
        assemblageTagRepo.Create(assemblageTag);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var assemblageTag = assemblageTagRepo.Get(id);
        if (assemblageTag == null)
        {
            return NotFound();
        }

        return View(assemblageTag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, [Bind("Id,Name")] AssemblageTag assemblageTag)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != assemblageTag.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(assemblageTag);
        }
        assemblageTagRepo.Update(assemblageTag);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var assemblageTag = assemblageTagRepo.Get(id);
        if (assemblageTag == null)
        {
            return NotFound();
        }

        return View(assemblageTag);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var assemblageTag = assemblageTagRepo.Get(id);
        if (assemblageTag == null)
        {
            return NotFound();
        }
        assemblageTagRepo.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
