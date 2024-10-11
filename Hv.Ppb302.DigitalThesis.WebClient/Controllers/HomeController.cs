using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Hv.Ppb302.DigitalThesis.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using MimeDetective;
using System.Diagnostics;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class HomeController(GeoTagRepository geoTagRepo,
    MolarMosaicRepository molarMosaicRepo,
    MolecularMosaicRepository molecularMosaicRepo,
    KaleidoscopeTagRepository kaleidoscopeTagRepo,
    PageRepository pageRepo,
    UploadRepository uploadRepo,
    MonthlyVisitRepository monthlyVisitRepo,
    YearlyVisitRepository yearlyVisitRepo) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Intro()
    {
        return View();
    }

    public IActionResult Meanwhile()
    {
        return View();
    }

    public IActionResult About()
    {
        return View(pageRepo.GetByName("About"));
    }

    public IActionResult Geotags(bool showTutorial = false)
    {
        ViewBag.ShowTutorial = showTutorial;
        
        // Check if the user has a cookie indicating a previous visit to the page
        var uniqueUser = ReadCookie("digital-thesis-user");
        // If not, create a cookie with the current date and time and add the user to the list of visitors
        if (uniqueUser?.Count == 0)
        {
            CreateCookie("digital-thesis-user", new List<string> { "visited-page" }, 30);
            // Add a new visitor to the database
            UpdateVisitationCount();
        }

        return View(geoTagRepo.GetAll());
    }

    public IActionResult Materials()
    {
        return View(GetAllMaterialFiles());
    }

    [Route("Home/Detail/{objectId:Guid}")]
    public IActionResult Detail(Guid objectId, string objectType)
    {
        if (objectType == "geotag")
        {
            var geoTag = geoTagRepo.Get(objectId);
            if (geoTag != null)
            {
                return View(BuildViewModel(geoTag));
            }
        }
        else if (objectType == "molarmosaic")
        {
            var molarMosaics = molarMosaicRepo.Get(objectId);
            if (molarMosaics != null)
            {
                CreateMosaicCookie(objectId);
                return View(BuildViewModel(molarMosaics));
            }
        }
        else if (objectType == "molecularmosaic")
        {
            var molecularMosaics = molecularMosaicRepo.Get(objectId);
            if (molecularMosaics != null)
            {
                CreateMosaicCookie(objectId);
                return View(BuildViewModel(molecularMosaics));
            }
        }
        else
        {
            throw new Exception("Invalid object type");
        }

        return NotFound();

        DetailViewModel BuildViewModel(dynamic model)
        {
            var viewModel = new DetailViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                PdfFilePath = model.PdfFilePath,
                AudioFilePath = model.AudioFilePath,
                ConnectorTags = [],
                Becomings = [],
                IsVisible = model.IsVisible,
                ObjectType = objectType,
            };
            if (model != null)
            {
                var propertyInfo = model.GetType().GetProperty("ConnectorTags");
                if (propertyInfo != null)
                {
                    viewModel.ConnectorTags = (List<ConnectorTag>)propertyInfo.GetValue(model);
                }

                propertyInfo = model.GetType().GetProperty("Becomings");
                if (propertyInfo != null)
                {
                    viewModel.Becomings = (List<string>)propertyInfo.GetValue(model);
                }
            }

            return viewModel;
        }
    }

    public IActionResult MolarMosaics()
    {
        return View(BuildViewModel(molarMosaicRepo.GetAll()!));

        static MolarMosaicsViewModel BuildViewModel(IEnumerable<MolarMosaic> molarMosaics)
        {
            return new MolarMosaicsViewModel
            {
                MolarMosaics = molarMosaics.ToList(),
                ConnectorTags = molarMosaics.SelectMany(x => x.ConnectorTags!).Distinct().ToList(),
            };
        }
    }

    public IActionResult MolecularMosaics()
    {
        return View(BuildViewModel(molecularMosaicRepo.GetAll()!));

        static MolecularMosaicsViewModel BuildViewModel(IEnumerable<MolecularMosaic> molecularMosaics)
        {
            return new MolecularMosaicsViewModel
            {
                MolecularMosaics = molecularMosaics.ToList(),
                ConnectorTags = molecularMosaics.SelectMany(x => x.ConnectorTags!).Distinct().ToList(),
            };
        }
    }

    public IActionResult Kaleidoscoping()
    {
        return View(BuildViewModel(molarMosaicRepo.GetAll()!, 
            molecularMosaicRepo.GetAll()!, 
            kaleidoscopeTagRepo.GetAll()!,
            pageRepo.GetByName("Kaleidoscope")!));

        static KaleidoscopingViewModel BuildViewModel(IEnumerable<MolarMosaic> molarMosaics, 
            IEnumerable<MolecularMosaic> molecularMosaics, 
            IEnumerable<KaleidoscopeTag> kaleidoscopeTags,
            Page kaleidoscopePage)
        {
            return new KaleidoscopingViewModel
            {
                MolarMosaics = molarMosaics.ToList(),
                MolecularMosaics = molecularMosaics.ToList(),
                KaleidoscopeTags = kaleidoscopeTags.ToList(),
                KaleidoscopePage = kaleidoscopePage
            };
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendMail(string receiver)
    {
        var email = new Email(receiver);
        var serviceSender = new EmailService();
        await serviceSender.SendMail(email);
        return View("Index");
    }

    public void CreateMosaicCookie(Guid objectId)
    {
        var visitedMosaicsList = ReadCookie("digital-thesis-mosaics");
        visitedMosaicsList ??= new List<string>(); // If cookie is null, create a new list
        if (!visitedMosaicsList.Contains(objectId.ToString()))
        {
            visitedMosaicsList.Add(objectId.ToString());
        }
        CreateCookie("digital-thesis-mosaics", visitedMosaicsList, 30);
    }

    public void CreateCookie(string key, List<string> values, int? expirationTime)
    {
        var jsonSerializer = JsonSerializer.Serialize(values);
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(expirationTime ?? 30),
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        Response.Cookies.Append(key, jsonSerializer, cookieOptions);
    }

    public List<string>? ReadCookie(string key)
    {
        var cookieValue = Request.Cookies[key];
        return cookieValue != null ? JsonSerializer.Deserialize<List<string>>(cookieValue) : new List<string>();
    }

    public List<FilesViewModel> GetAllMaterialFiles()
    {
        var inspector = new ContentInspectorBuilder
        {
            Definitions = MimeDetective.Definitions.Default.All()
        }.Build();

        var path = Path.Combine(@"C:\Uploads");
        var files = Directory.GetFiles(path)
                             .Select(path => Path.GetFileName(path))
                             .ToList();

        var uploadsList = uploadRepo.GetAllMaterials();
        List<FilesViewModel> fileViewModels = [];
        foreach (var file in files)
        {
            var isMaterialFile = uploadsList?.FirstOrDefault(m => m.Name == file);

            if (isMaterialFile != null)
            {
                var results = inspector.Inspect(Path.Combine(@"C:\Uploads", file));
                var fileType = results.FirstOrDefault()!.Definition.File.Categories.FirstOrDefault();
                var fileUrl = string.Concat("https://da.ios.hv.se/DigitalThesis/staticfiles/", file);
                var upload = uploadsList?.OrderBy(u => u.MaterialOrder).FirstOrDefault(u => u.Name == file);

                fileViewModels.Add(new FilesViewModel
                {
                    Category = fileType,
                    Name = file,
                    FileUrl = fileUrl,
                    IsMaterial = upload?.IsMaterial,
                    MaterialOrder = upload?.MaterialOrder
                    
                });
            }

        }
        var orderList = fileViewModels.OrderBy(u => u.MaterialOrder).ToList();
        return orderList;
    }

    private void UpdateVisitationCount()
    {
        // Check if the database already contains an entry for the current year
        var yearlyVisit = yearlyVisitRepo.GetByYear(DateTime.Now.Year);
        if (yearlyVisit is null)
        {
            // If an entry does not exist create it
            yearlyVisitRepo.Create(new YearlyVisit
            {
                Year = DateTime.Now.Year,
                Visits = 1
            });
            yearlyVisit = yearlyVisitRepo.GetByYear(DateTime.Now.Year);
        }
        else
        {
            // If an entry exists update the number of visits
            yearlyVisitRepo.Update(new YearlyVisit
            {
                Id = yearlyVisit.Id,
                Year = yearlyVisit.Year,
                Visits = yearlyVisit.Visits + 1
            });
        }


        // Check if the database already contains an entry for the current month and year
        var monthlyVisit = monthlyVisitRepo.GetByMonthAndYear(DateTime.Now.Month, DateTime.Now.Year);
        if (monthlyVisit is null)
        {
            // If an entry does not exist create it
            monthlyVisitRepo.Create(new MonthlyVisit
            {
                Month = DateTime.Now.Month,
                YearlyVisitId = yearlyVisit!.Id,
                Visits = 1
            });
        }
        else
        {
            // If an entry exists update the number of visits
            monthlyVisitRepo.Update(new MonthlyVisit
            {
                Id = monthlyVisit.Id,
                Month = monthlyVisit.Month,
                YearlyVisitId = yearlyVisit!.Id,
                Visits = monthlyVisit.Visits + 1
            });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
