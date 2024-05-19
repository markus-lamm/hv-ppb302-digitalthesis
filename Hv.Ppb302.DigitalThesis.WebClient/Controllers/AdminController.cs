using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Microsoft.AspNetCore.Http;
using MimeDetective;
using MimeDetective.Storage;
using Microsoft.AspNetCore.Http.HttpResults;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using System.Drawing;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly GeoTagRepository _geoTagRepo;
        private readonly ConnectorTagRepository _connectorTagRepo;
        private readonly MolarMosaicRepository _molarMosaicRepo;
        private readonly MolecularMosaicRepository _molecularMosaicRepo;
        private readonly KaleidoscopeTagRepository _kaleidoscopeTagRepo;

        public AdminController(UserRepository userRepository, GeoTagRepository geoTagRepo,
            MolarMosaicRepository molarMosaicRepo,
            MolecularMosaicRepository molecularMosaicRepo,
            ConnectorTagRepository connectorTagRepo,
            KaleidoscopeTagRepository kaleidoscopeTagRepo)
        {
            _userRepository = userRepository;
            _geoTagRepo = geoTagRepo;
            _molarMosaicRepo = molarMosaicRepo;
            _molecularMosaicRepo = molecularMosaicRepo;
            _connectorTagRepo = connectorTagRepo;
            _kaleidoscopeTagRepo = kaleidoscopeTagRepo;
        }

        public IActionResult Index()
        {
            if (!CheckAuthentication())
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        public IActionResult FileUpload()
        {
            if (!CheckAuthentication())
            {
                return RedirectToAction("Login", "Admin");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(@"C:\Uploads", fileName); // Specify the absolute path

                using (var stream = System.IO.File.Create(path))
                {
                    await file.CopyToAsync(stream);
                }
            }

            var fileViewModels = GetFiles();
            return View("FileView", fileViewModels);
        }

        [HttpGet]
        public IActionResult FileView()
        {
            if (!CheckAuthentication())
            {
                return RedirectToAction("Login", "Admin");
            }

            var fileViewModels = GetFiles();

            return View(fileViewModels);
        }

        [HttpPost]
        public IActionResult FileView(string FileName)
        {
            var path = Path.Combine(@"C:\Uploads", FileName);
            FileInfo file = new(path);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }
            var fileViewModels = GetFiles();

            return View("FileView", fileViewModels);
        }

        public IActionResult Profile()
        {
            if (!CheckAuthentication())
            {
                return RedirectToAction("Login", "Admin");
            }

            string? username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;

            return View();
        }

        [HttpPost]
        public IActionResult Profile(User user)
        {
            if (!CheckAuthentication())
            {
                return RedirectToAction("Login", "Admin");
            }

            _userRepository.Update(user);
            ViewBag.lyckad = "Lösenordet har ändrats";
            ViewBag.Username = user.Username;

            return View();
        }

        public IActionResult Login()
        {
            if (TempData["LoginError"] != null)
            {
                TempData.Remove("LoginError");
                ViewBag.Error = "Invalid username or password";
            }
            return View();
        }

        public IActionResult Logout()
        {
            RemoveAuthentication();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddAuthentication(string username, string password)
        {
            var user = _userRepository.GetByCredentials(username, password);
            if(user == null)
            {
                TempData["LoginError"] = true;
                return RedirectToAction("Login", "Admin");
            }

            HttpContext.Session.SetString("Username", username);

            return RedirectToAction("Index", "Admin");
        }

        public void RemoveAuthentication()
        {
            HttpContext.Session.Remove("Username");
        }

        public bool CheckAuthentication()
        {
            return HttpContext.Session.GetString("Username") != null;
        }

        public List<FileViewViewModel> GetFiles()
        {
            var Inspector = new ContentInspectorBuilder()
            {
                Definitions = MimeDetective.Definitions.Default.All()
            }.Build();

            var path = Path.Combine(@"C:\Uploads");
            var files = Directory.GetFiles(path)
                                 .Select(path => Path.GetFileName(path))
                                 .ToList();

            List<FileViewViewModel> fileViewModels = [];
            foreach (var file in files)
            {
                var Results = Inspector.Inspect(Path.Combine(@"C:\Uploads", file));
                var fileType = Results.FirstOrDefault().Definition.File.Categories.FirstOrDefault();
                var fileUrl = String.Concat("https://informatik13.ei.hv.se/DigitalThesis/staticfiles/", file);

                fileViewModels.Add(new FileViewViewModel
                {
                    Category = fileType,
                    File = file,
                    FileUrl = fileUrl
                });
            }
            return fileViewModels;
        }
    }
}
