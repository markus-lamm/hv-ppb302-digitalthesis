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

        public HomeController(ILogger<HomeController> logger, GeoTagRepository geoTagRepo)
        {
            _logger = logger;
            _geoTagRepo = geoTagRepo;
        }

        public IActionResult Home()
        {
            return View(_geoTagRepo.GetAll());
        }

        public IActionResult About()
        {
            return View();
        }

        [Route("Home/Mosaics/{mosaicId}")]
        public IActionResult Mosaics(string mosaicId)
        {
            ViewBag.MosaicId = mosaicId;
            return View();
        }

        public IActionResult MolarMosaics()
        { 
            return View(); 
        }

        public IActionResult MolecularMosaics()
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
