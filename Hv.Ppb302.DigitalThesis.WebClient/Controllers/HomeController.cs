using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GeoTagCrud _geoTagCrud;

        public HomeController(ILogger<HomeController> logger, GeoTagCrud geoTagCrud)
        {
            _logger = logger;
            _geoTagCrud = geoTagCrud;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View(_geoTagCrud.GetGeoTags());
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
