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
        private readonly GroupTagRepository _groupTagRepo;
        private readonly MolarMosaicRepository _molarMosaicRepo;
        private readonly MolecularMosaicRepository _molecularMosaicRepo;
        private readonly KaleidoscopeMosaicRepository _kaleidoscopeMosaicRepo;
        private readonly TestDataUtils _testDataUtils;

        public HomeController(ILogger<HomeController> logger, 
            GeoTagRepository geoTagRepo, 
            MolarMosaicRepository molarMosaicRepo, 
            MolecularMosaicRepository molecularMosaicRepo,
            TestDataUtils testDataUtils,
            GroupTagRepository groupTagRepo,
            KaleidoscopeMosaicRepository kaleidoscopeMosaicRepository)
        {
            _logger = logger;
            _geoTagRepo = geoTagRepo;
            _molarMosaicRepo = molarMosaicRepo;
            _molecularMosaicRepo = molecularMosaicRepo;
            _testDataUtils = testDataUtils;
            _groupTagRepo = groupTagRepo;
            _kaleidoscopeMosaicRepo = kaleidoscopeMosaicRepository;
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
        public IActionResult Detail(string objectId)
        {
            Guid guid = Guid.Parse(objectId);

            var geoTag = _geoTagRepo.Get(guid);
            if (geoTag != null)
            {
                return View(BuildViewModel(geoTag));
            }

            var molarMosaics = _molarMosaicRepo.Get(guid);
            if (molarMosaics != null)
            {
                return View(BuildViewModel(molarMosaics));
            }

            var molecularMosaics = _molecularMosaicRepo.Get(guid);
            if (molecularMosaics != null)
            {
                return View(BuildViewModel(molecularMosaics));
            }

            var kaleidoscopeMosaic = _kaleidoscopeMosaicRepo.Get(guid);
            if (kaleidoscopeMosaic != null)
            {
                return View(BuildViewModel(kaleidoscopeMosaic));
            }

            return NotFound();

            static DetailViewModel BuildViewModel(dynamic model)
            {
                return new DetailViewModel
                {
                    ObjectId = model.Id,
                    Title = model.Title,
                    Content = model.Content,
                    GroupTags = model.GroupTags,
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
                    GroupTags = molarMosaics.SelectMany(x => x.GroupTags).Distinct().ToList(),
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
                    GroupTags = molecularMosaics.SelectMany(x => x.GroupTags).Distinct().ToList(),
                };
            }
        }

        public IActionResult Kaleidoscoping()
        {
            return View(_groupTagRepo.GetAll());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
