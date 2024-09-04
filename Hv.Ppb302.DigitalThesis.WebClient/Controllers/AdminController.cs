using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using MimeDetective;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Newtonsoft.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class AdminController : Controller
{
    private readonly UserRepository _userRepo;
    private readonly UploadRepository _uploadRepo;
    

    public AdminController(UserRepository userRepo, UploadRepository uploadRepository)
    {
        _userRepo = userRepo;
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

    public IActionResult Profile()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        var username = HttpContext.Session.GetString("Username");
        if (username == null)
        {
            return RedirectToAction("Login", "Admin");
        }

        var user = _userRepo.GetByUsername(username);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    public IActionResult Profile(Guid id, [Bind("Id,Username,Password")] User user)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != user.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(user);
        }
        _userRepo.Update(user);

        return AddAuthentication(user.Username, user.Password);
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

    [HttpGet]
    public IActionResult Files()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(GetAllFiles());
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
        return View("Files", GetAllFiles());
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
        return View("Files", GetAllFiles());
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

        return RedirectToAction("Files", GetAllFiles());
    }

    public List<FilesViewModel> GetAllFiles()
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
        List<FilesViewModel> filesViewModels = [];
        foreach (var file in files)
        {
            var Results = Inspector.Inspect(Path.Combine(@"C:\Uploads", file));
            var fileType = Results.FirstOrDefault()!.Definition.File.Categories.FirstOrDefault();
            var fileUrl = String.Concat("https://informatik13.ei.hv.se/DigitalThesis/staticfiles/", file);
            var upload = uploadsList?.FirstOrDefault(u => u.Name == file);

            filesViewModels.Add(new FilesViewModel
            {
                Category = fileType,
                Name = file,
                FileUrl = fileUrl,
                IsMaterial = upload?.IsMaterial
            });
        }
        return filesViewModels;
    }
}
