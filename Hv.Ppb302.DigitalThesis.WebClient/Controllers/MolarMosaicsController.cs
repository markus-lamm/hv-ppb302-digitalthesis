﻿using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class MolarMosaicsController(MolarMosaicRepository molarMosaicRepo,
    ConnectorTagRepository connectorTagRepo,
    KaleidoscopeTagRepository kaleidoscopeTagRepo,
    AssemblageTagRepository assemblageTagRepo) : Controller
{
    public IActionResult Index()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(molarMosaicRepo.GetAll());
    }

    public IActionResult Create()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var excludedIds = new[] { "f2d2a02b-73bb-42e4-8774-2102ef9c3102", "1ac2b7b1-c3bf-4fc3-a5fb-37c88eeb1e97" };

        var crudViewModel = new MolarMosaicCrudViewModel
        {
            ConnectorTagsItemList = connectorTagRepo.GetAll().ToSelectListItemsList(
                tag => tag.Id.ToString(),
                tag => tag.Name
            ),
            AssemblageTagsItemList = assemblageTagRepo.GetAll().ToSelectListItemsList(
                tag => tag.Id.ToString(),
                tag => tag.Name
            ),
            KaleidoscopeTagsItemList = kaleidoscopeTagRepo.GetAll()?
                .Where(k => !excludedIds.Contains(k.Id.ToString()))
                .ToSelectListItemsList(
                    tag => tag.Id.ToString(),
                    tag => tag.Name
                )
        };

        return View(crudViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MolarMosaicCrudViewModel molarMosaicCrudViewModel)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (!ModelState.IsValid)
        {
            RedirectToAction(nameof(Index));
        }

        var dbMolarMosaic = new MolarMosaic
        {
            Title = molarMosaicCrudViewModel.MolarMosaic?.Title,
            Content = molarMosaicCrudViewModel.MolarMosaic?.Content,
            IsVisible = molarMosaicCrudViewModel.MolarMosaic?.IsVisible,
            PdfFilePath = molarMosaicCrudViewModel.MolarMosaic?.PdfFilePath,
            AudioFilePath = molarMosaicCrudViewModel.MolarMosaic?.AudioFilePath,
            AssemblageTagId = molarMosaicCrudViewModel.MolarMosaic?.AssemblageTagId,
            ConnectorTags = connectorTagRepo.GetAll()?
            .Where(tag => molarMosaicCrudViewModel.SelectedConnectorsTagsIds.Contains(tag.Id))
            .ToList(),
            KaleidoscopeTags = kaleidoscopeTagRepo.GetAll()?
                .Where(tag => molarMosaicCrudViewModel.SelectedKaleidoscopeTagsIds.Contains(tag.Id))
                .ToList(),
            Becomings = molarMosaicCrudViewModel.Becomings.ToStringListFromTagifyFormat()
        };

        molarMosaicRepo.Create(dbMolarMosaic);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molarMosaic = molarMosaicRepo.Get(id);
        if (molarMosaic == null)
        {
            return NotFound();
        }

        var excludedIds = new[] { "f2d2a02b-73bb-42e4-8774-2102ef9c3102", "1ac2b7b1-c3bf-4fc3-a5fb-37c88eeb1e97" };
        var crudViewModel = new MolarMosaicCrudViewModel
        {
            MolarMosaic = molarMosaic,
            ConnectorTagsItemList = connectorTagRepo.GetAll().ToSelectListItemsList(
                tag => tag.Id.ToString(),
                tag => tag.Name,
                selectedValues: molarMosaic.ConnectorTags?.Select(ct => ct.Id.ToString())
            ),
            AssemblageTagsItemList = assemblageTagRepo.GetAll().ToSelectListItemsList(
                tag => tag.Id.ToString(),
                tag => tag.Name
            ),
            KaleidoscopeTagsItemList = kaleidoscopeTagRepo.GetAll()?
                .Where(k => !excludedIds.Contains(k.Id.ToString()))
                .ToSelectListItemsList(
                    tag => tag.Id.ToString(),
                    tag => tag.Name, 
                    selectedValues: molarMosaic.KaleidoscopeTags?.Select(ct => ct.Id.ToString())
                ),
            //Send becoming as string containing Tagify formation
            Becomings = molarMosaic.Becomings != null && molarMosaic.Becomings.Count != 0
                ? string.Join(",", molarMosaic.Becomings)
                : null
        };

        return View(crudViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, MolarMosaicCrudViewModel molarMosaicCrudViewModel)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (molarMosaicCrudViewModel.MolarMosaic != null && id != molarMosaicCrudViewModel.MolarMosaic.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(molarMosaicCrudViewModel);
        }

        var dbMolarMosaic = new MolarMosaic
        {
            Id = id,
            Title = molarMosaicCrudViewModel.MolarMosaic?.Title,
            Content = molarMosaicCrudViewModel.MolarMosaic?.Content,
            IsVisible = molarMosaicCrudViewModel.MolarMosaic?.IsVisible,
            PdfFilePath = molarMosaicCrudViewModel.MolarMosaic?.PdfFilePath,
            AudioFilePath = molarMosaicCrudViewModel.MolarMosaic?.AudioFilePath,
            AssemblageTagId = molarMosaicCrudViewModel.MolarMosaic?.AssemblageTagId,
            ConnectorTags = connectorTagRepo.GetAll()?
                .Where(tag => molarMosaicCrudViewModel.SelectedConnectorsTagsIds.Contains(tag.Id))
                .ToList(),
            KaleidoscopeTags = kaleidoscopeTagRepo.GetAll()?
                .Where(tag => molarMosaicCrudViewModel.SelectedKaleidoscopeTagsIds.Contains(tag.Id))
                .ToList(),
            Becomings = molarMosaicCrudViewModel.Becomings.ToStringListFromTagifyFormat()
        };

        molarMosaicRepo.Update(dbMolarMosaic);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molarMosaic = molarMosaicRepo.Get(id);
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

        var molarMosaic = molarMosaicRepo.Get(id);
        if (molarMosaic == null)
        {
            return NotFound();
        }
        molarMosaicRepo.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
