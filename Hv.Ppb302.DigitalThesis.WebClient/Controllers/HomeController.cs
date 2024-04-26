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
        private readonly MolarMosaicRepository _molarMosaicRepo;
        private readonly MolecularMosaicRepository _molecularMosaicRepo;

        public HomeController(ILogger<HomeController> logger, 
            GeoTagRepository geoTagRepo, 
            MolarMosaicRepository molarMosaicRepo, 
            MolecularMosaicRepository molecularMosaicRepo)
        {
            _logger = logger;
            _geoTagRepo = geoTagRepo;
            _molarMosaicRepo = molarMosaicRepo;
            _molecularMosaicRepo = molecularMosaicRepo;
        }

        public IActionResult Index()
        {
            return View(_geoTagRepo.GetAll());
        }

        public IActionResult About()
        {
            return View();
        }

        [Route("Home/Detail/{objectId}")]
        public IActionResult Detail(string objectId)
        {
            Guid guid = Guid.Parse(objectId);
            var geoTag = _geoTagRepo.Get(guid);
            var molarMosaics = _molarMosaicRepo.Get(guid);
            var molecularMosaics = _molecularMosaicRepo.Get(guid);

            static DetailViewModel BuildViewModel(dynamic model)
            {
                return new DetailViewModel
                {
                    ObjectId = model.Id,
                    Title = model.Title,
                    Content = model.Content,
                    PdfFilePath = model.PdfFilePath,
                    HasAudio = model.HasAudio,
                    AudioFilePath = model.AudioFilePath,
                };
            }

            if (geoTag != null)
            {
                return View(BuildViewModel(geoTag));
            }
            else if (molarMosaics != null)
            {
                return View(BuildViewModel(molarMosaics));
            }
            else if (molecularMosaics != null)
            {
                return View(BuildViewModel(molecularMosaics));
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult MolarMosaics()
        { 
            return View(); 
        }

        public IActionResult MolecularMosaics()
        {
            return View();
        }

        public IActionResult Kaleidoscoping()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
