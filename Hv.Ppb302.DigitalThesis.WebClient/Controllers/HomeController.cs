using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using MimeDetective;
using System.Diagnostics;
using System.Text.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class HomeController : Controller
{
    private readonly GeoTagRepository _geoTagRepo;
    private readonly MolarMosaicRepository _molarMosaicRepo;
    private readonly MolecularMosaicRepository _molecularMosaicRepo;
    private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;
    private readonly PageRepository _pageRepository;
    private readonly UploadRepository _uploadRepository;
    private readonly MonthlyVisitRepository _monthlyVisitRepository;
    private readonly YearlyVisitRepository _yearlyVisitRepository;

    public HomeController(GeoTagRepository geoTagRepo, 
        MolarMosaicRepository molarMosaicRepo, 
        MolecularMosaicRepository molecularMosaicRepo,
        KaleidoscopeTagRepository kaleidoscopeTagRepo,
        PageRepository pageRepo,
        UploadRepository uploadRepository,
        MonthlyVisitRepository monthlyVisitRepository,
        YearlyVisitRepository yearlyVisitRepository)
    {
        _geoTagRepo = geoTagRepo;
        _molarMosaicRepo = molarMosaicRepo;
        _molecularMosaicRepo = molecularMosaicRepo;
        _kaleidoscopeTagRepo = kaleidoscopeTagRepo;
        _pageRepository = pageRepo;
        _uploadRepository = uploadRepository;
        _monthlyVisitRepository = monthlyVisitRepository;
        _yearlyVisitRepository = yearlyVisitRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Intro()
    {
        return View();
    }

    public IActionResult About()
    {
        return View(_pageRepository.GetByName("About"));
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

        return View(_geoTagRepo.GetAll());
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
            var geoTag = _geoTagRepo.Get(objectId);
            if (geoTag != null)
            {
                return View(BuildViewModel(geoTag));
            }
        }
        else if (objectType == "molarmosaic")
        {
            var molarMosaics = _molarMosaicRepo.Get(objectId);
            if (molarMosaics != null)
            {
                CreateMosaicCookie(objectId);
                return View(BuildViewModel(molarMosaics));
            }
        }
        else if (objectType == "molecularmosaic")
        {
            var molecularMosaics = _molecularMosaicRepo.Get(objectId);
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
        return View(BuildViewModel(_molarMosaicRepo.GetAll()!));

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
        return View(BuildViewModel(_molecularMosaicRepo.GetAll()!));

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
        return View(BuildViewModel(_molarMosaicRepo.GetAll()!, 
            _molecularMosaicRepo.GetAll()!, 
            _kaleidoscopeTagRepo.GetAll()!,
            _pageRepository.GetByName("Kaleidoscope")!));

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

        var uploadsList = _uploadRepository.GetAllMaterials();
        List<FilesViewModel> fileViewModels = [];
        foreach (var file in files)
        {
            var isMaterialFile = uploadsList?.FirstOrDefault(m => m.Name == file);

            if (isMaterialFile != null)
            {
                var results = inspector.Inspect(Path.Combine(@"C:\Uploads", file));
                var fileType = results.FirstOrDefault()!.Definition.File.Categories.FirstOrDefault();
                var fileUrl = string.Concat("https://informatik13.ei.hv.se/DigitalThesis/staticfiles/", file);
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
        var yearlyVisit = _yearlyVisitRepository.GetByYear(DateTime.Now.Year);
        if (yearlyVisit is null)
        {
            // If an entry does not exist create it
            _yearlyVisitRepository.Create(new YearlyVisit
            {
                Year = DateTime.Now.Year,
                Visits = 1
            });
            yearlyVisit = _yearlyVisitRepository.GetByYear(DateTime.Now.Year);
        }
        else
        {
            // If an entry exists update the number of visits
            _yearlyVisitRepository.Update(new YearlyVisit
            {
                Id = yearlyVisit.Id,
                Year = yearlyVisit.Year,
                Visits = yearlyVisit.Visits + 1
            });
        }


        // Check if the database already contains an entry for the current month and year
        var monthlyVisit = _monthlyVisitRepository.GetByMonthAndYear(DateTime.Now.Month, DateTime.Now.Year);
        if (monthlyVisit is null)
        {
            // If an entry does not exist create it
            _monthlyVisitRepository.Create(new MonthlyVisit
            {
                Month = DateTime.Now.Month,
                YearlyVisitId = yearlyVisit!.Id,
                Visits = 1
            });
        }
        else
        {
            // If an entry exists update the number of visits
            _monthlyVisitRepository.Update(new MonthlyVisit
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
