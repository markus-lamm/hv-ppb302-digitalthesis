using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GeoTagRepository _geoTagRepo;
        private readonly ConnectorTagRepository _connectorTagRepo;
        private readonly MolarMosaicRepository _molarMosaicRepo;
        private readonly MolecularMosaicRepository _molecularMosaicRepo;
        private readonly TestDataUtils _testDataUtils;

        public HomeController(ILogger<HomeController> logger, 
            GeoTagRepository geoTagRepo, 
            MolarMosaicRepository molarMosaicRepo, 
            MolecularMosaicRepository molecularMosaicRepo,
            TestDataUtils testDataUtils,
            ConnectorTagRepository connectorTagRepo)
        {
            _logger = logger;
            _geoTagRepo = geoTagRepo;
            _molarMosaicRepo = molarMosaicRepo;
            _molecularMosaicRepo = molecularMosaicRepo;
            _testDataUtils = testDataUtils;
            _connectorTagRepo = connectorTagRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Geotags()
        {
            return View(_geoTagRepo.GetAll());
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
                    return View(BuildViewModel(molarMosaics));
                }
            }
            else if (objectType == "molecularmosaic")
            {
                var molecularMosaics = _molecularMosaicRepo.Get(objectId);
                if (molecularMosaics != null)
                {
                    return View(BuildViewModel(molecularMosaics));
                }
            }
            else
            {
                throw new Exception("Invalid object type");
            }


            return NotFound();

            static DetailViewModel BuildViewModel(dynamic model)
            {
                return new DetailViewModel
                {
                    ObjectId = model.Id,
                    Title = model.Title,
                    Content = model.Content,
                    ConnectorTags = model.ConnectorTags,
                    Becomings = model.Becomings,
                    PdfFilePath = model.PdfFilePath,
                    HasAudio = model.HasAudio,
                    AudioFilePath = model.AudioFilePath,
                };
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
                    ConnectorTags = molarMosaics.SelectMany(x => x.ConnectorTags).Distinct().ToList(),
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
                    ConnectorTags = molecularMosaics.SelectMany(x => x.ConnectorTags).Distinct().ToList(),
                };
            }
        }

        public IActionResult Kaleidoscoping()
        {
            return View(BuildViewModel(_molarMosaicRepo.GetAll()!, _molecularMosaicRepo.GetAll()!));

            static KaleidoscopingViewModel BuildViewModel(IEnumerable<MolarMosaic> molarMosaics, IEnumerable<MolecularMosaic> molecularMosaics)
            {
                return new KaleidoscopingViewModel
                {
                    MolarMosaics = molarMosaics.ToList(),
                    MolecularMosaics = molecularMosaics.ToList(),
                };
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
