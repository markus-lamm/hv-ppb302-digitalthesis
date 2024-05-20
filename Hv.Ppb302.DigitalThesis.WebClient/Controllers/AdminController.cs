using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using MimeDetective;
using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class AdminController : Controller
{
    private readonly UserRepository _userRepo;
    private readonly PageRepository _pageRepo;

    public AdminController(UserRepository userRepo, PageRepository pageRepo)
    {
        _userRepo = userRepo;
        _pageRepo = pageRepo;
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
    public IActionResult FileView(string FileName)
    {
        var path = Path.Combine(@"C:\Uploads", FileName);
        FileInfo file = new(path);
        if (file.Exists) // Check if file exists  
        {
            file.Delete();
        }
        return View("FileView", GetAllFiles());
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
        return RedirectToAction("Index", "Home");
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

        List<FileViewViewModel> fileViewModels = [];
        foreach (var file in files)
        {
            var Results = Inspector.Inspect(Path.Combine(@"C:\Uploads", file));
            var fileType = Results.FirstOrDefault()!.Definition.File.Categories.FirstOrDefault();
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
