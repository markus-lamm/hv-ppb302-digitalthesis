using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using MimeDetective;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class AdminController : Controller
{
    private readonly UserRepository _userRepo;
    private readonly PageRepository _pageRepo;
    private readonly UploadRepository _uploadRepo;
    

    public AdminController(UserRepository userRepo, PageRepository pageRepo, UploadRepository uploadRepository)
    {
        _userRepo = userRepo;
        _pageRepo = pageRepo;
        _uploadRepo = uploadRepository;
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

    [DisableRequestSizeLimit]
    [HttpPost]
    public async Task<IActionResult> FileUpload(IFormFile file, Upload viewmodel)
    {
        if (file != null)
        {
            var fileName = Path.GetFileName(file.FileName);
            viewmodel.Name = fileName;
            var path = Path.Combine(@"C:\Uploads", fileName); // Specify the absolute path

            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }
            _uploadRepo.Create(viewmodel);
        }
        return View("FileView", GetAllFiles());
    }

    [HttpGet]
    public IActionResult FileView()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(GetAllFiles());
    }

    [HttpPost]
    public IActionResult DeleteFile(string FileName)
    {
        var path = Path.Combine(@"C:\Uploads", FileName);
        FileInfo file = new(path);
        if (file.Exists) 
        {
            file.Delete();
            _uploadRepo.Delete(FileName);
        }
        return View("FileView", GetAllFiles());
    }

    [HttpPost]
    public IActionResult UpdateMaterials(string materialsData)
    {

        var materialsStatus = JsonConvert.DeserializeObject<Dictionary<string, bool>>(materialsData);
        if (materialsStatus?.Count != 0)
        {
            var uploadsToUpdate = (from entry in materialsStatus
                                   select new Upload
                                   {
                                       Name = entry.Key,
                                       IsMaterial = entry.Value
                                   }).ToList();

            _uploadRepo.Update(uploadsToUpdate);
        }

        return RedirectToAction("FileView", GetAllFiles());
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

        _userRepo.Update(user);
        ViewBag.Success = "Password has been changed";
        ViewBag.Username = user.Username;

        return View();
    }

    public IActionResult AboutAdmin()
    {
        return View(_pageRepo.GetByName("About"));
    }

    [HttpPost]
    public IActionResult AboutAdmin(Page page)
    {
        _pageRepo.Update(page);
        return View(page);
    }

    public IActionResult StartAdmin()
    {
        return View(_pageRepo.GetByName("Start"));
    }

    [HttpPost]
    public IActionResult StartAdmin(Page page)
    {
        _pageRepo.Update(page);
        return View(page);
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
        return RedirectToAction("GeoTags", "Home");
    }

    public IActionResult AddAuthentication(string username, string password)
    {
        var user = _userRepo.GetByCredentials(username, password);
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

    public List<FileViewViewModel> GetAllFiles()
    {
        var Inspector = new ContentInspectorBuilder()
        {
            Definitions = MimeDetective.Definitions.Default.All()
        }.Build();

        var path = Path.Combine(@"C:\Uploads");
        var files = Directory.GetFiles(path)
                             .Select(path => Path.GetFileName(path))
                             .ToList();

        var uploadsList = _uploadRepo.GetAll();
        List<FileViewViewModel> fileViewModels = [];
        foreach (var file in files)
        {
            var Results = Inspector.Inspect(Path.Combine(@"C:\Uploads", file));
            var fileType = Results.FirstOrDefault()!.Definition.File.Categories.FirstOrDefault();
            var fileUrl = String.Concat("https://informatik13.ei.hv.se/DigitalThesis/staticfiles/", file);
            var upload = uploadsList?.FirstOrDefault(u => u.Name == file);

            fileViewModels.Add(new FileViewViewModel
            {
                Category = fileType,
                Name = file,
                FileUrl = fileUrl,
                IsMaterial = upload?.IsMaterial
            });
        }
        return fileViewModels;
    }
}
