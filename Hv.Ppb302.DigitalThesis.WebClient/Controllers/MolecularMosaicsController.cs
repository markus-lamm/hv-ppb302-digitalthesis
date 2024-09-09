using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class MolecularMosaicsController(MolecularMosaicRepository molecularMosaicRepo,
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
        return View(molecularMosaicRepo.GetAll());
    }

    public IActionResult Create()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var excludedIds = new[] { "f2d2a02b-73bb-42e4-8774-2102ef9c3102", "1ac2b7b1-c3bf-4fc3-a5fb-37c88eeb1e97" };
        var crudViewModel = new MolecularMosaicCrudViewModel()
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
    public IActionResult Create(MolecularMosaicCrudViewModel molecularMosaicCrudViewModel)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (!ModelState.IsValid)
        {
            RedirectToAction(nameof(Index));
        }

        var dbMolarMosaic = new MolecularMosaic()
        {
            Title = molecularMosaicCrudViewModel.MolecularMosaic?.Title,
            Content = molecularMosaicCrudViewModel.MolecularMosaic?.Content,
            IsVisible = molecularMosaicCrudViewModel.MolecularMosaic?.IsVisible,
            PdfFilePath = molecularMosaicCrudViewModel.MolecularMosaic?.PdfFilePath,
            AudioFilePath = molecularMosaicCrudViewModel.MolecularMosaic?.AudioFilePath,
            AssemblageTagId = molecularMosaicCrudViewModel.MolecularMosaic?.AssemblageTagId,
            ConnectorTags = connectorTagRepo.GetAll()?
                .Where(tag => molecularMosaicCrudViewModel.SelectedConnectorsTagsIds.Contains(tag.Id))
                .ToList(),
            KaleidoscopeTags = kaleidoscopeTagRepo.GetAll()?
                .Where(tag => molecularMosaicCrudViewModel.SelectedKaleidoscopeTagsIds.Contains(tag.Id))
                .ToList(),
            Becomings = molecularMosaicCrudViewModel.Becomings.ToStringListFromTagifyFormat()
        };

        molecularMosaicRepo.Create(dbMolarMosaic);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molecularMosaic = molecularMosaicRepo.Get(id);
        if (molecularMosaic == null)
        {
            return NotFound();
        }

        var excludedIds = new[] { "f2d2a02b-73bb-42e4-8774-2102ef9c3102", "1ac2b7b1-c3bf-4fc3-a5fb-37c88eeb1e97" };
        var crudViewModel = new MolecularMosaicCrudViewModel()
        {
            MolecularMosaic = molecularMosaic,
            ConnectorTagsItemList = connectorTagRepo.GetAll().ToSelectListItemsList(
                tag => tag.Id.ToString(),
                tag => tag.Name,
                selectedValues: molecularMosaic.ConnectorTags?.Select(ct => ct.Id.ToString())
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
                    selectedValues: molecularMosaic.KaleidoscopeTags?.Select(ct => ct.Id.ToString())
                ),
            //Send becoming as string containing Tagify formation
            Becomings = molecularMosaic.Becomings != null && molecularMosaic.Becomings.Count != 0
                ? string.Join(",", molecularMosaic.Becomings)
                : null
        };

        return View(crudViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, MolecularMosaicCrudViewModel molecularMosaicCrudViewModel)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (molecularMosaicCrudViewModel.MolecularMosaic != null && id != molecularMosaicCrudViewModel.MolecularMosaic.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(molecularMosaicCrudViewModel);
        }

        var dbMolecularMosaic = new MolecularMosaic()
        {
            Id = id,
            Title = molecularMosaicCrudViewModel.MolecularMosaic?.Title,
            Content = molecularMosaicCrudViewModel.MolecularMosaic?.Content,
            IsVisible = molecularMosaicCrudViewModel.MolecularMosaic?.IsVisible,
            PdfFilePath = molecularMosaicCrudViewModel.MolecularMosaic?.PdfFilePath,
            AudioFilePath = molecularMosaicCrudViewModel.MolecularMosaic?.AudioFilePath,
            AssemblageTagId = molecularMosaicCrudViewModel.MolecularMosaic?.AssemblageTagId,
            ConnectorTags = connectorTagRepo.GetAll()?
                .Where(tag => molecularMosaicCrudViewModel.SelectedConnectorsTagsIds.Contains(tag.Id))
                .ToList(),
            KaleidoscopeTags = kaleidoscopeTagRepo.GetAll()?
                .Where(tag => molecularMosaicCrudViewModel.SelectedKaleidoscopeTagsIds.Contains(tag.Id))
                .ToList(),
            Becomings = molecularMosaicCrudViewModel.Becomings.ToStringListFromTagifyFormat()

        };

        molecularMosaicRepo.Update(dbMolecularMosaic);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var molecularMosaic = molecularMosaicRepo.Get(id);
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

        var molecularMosaic = molecularMosaicRepo.Get(id);
        if (molecularMosaic == null)
        {
            return NotFound();
        }
        molecularMosaicRepo.Delete(id);

        return RedirectToAction(nameof(Index));
    }

    public bool CheckAuthentication()
    {
        return HttpContext.Session.GetString("Username") != null;
    }
}
