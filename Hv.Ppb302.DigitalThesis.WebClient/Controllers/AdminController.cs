using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;
using MimeDetective;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Newtonsoft.Json;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers;

public class AdminController(UserRepository userRepo,
    UploadRepository uploadRepo,
    YearlyVisitRepository yearlyVisitRepo) : Controller
{
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

        var user = userRepo.GetByUsername(username);
        if (user == null)
        {
            return NotFound();
        }

        var profile = new Profile
        {
            Id = user.Id,
            Username = user.Username
        };

        return View(profile);
    }

    [HttpPost]
    public IActionResult Profile(Guid id, [Bind("Id,Username,NewPassword,OldPassword")] Profile profile)
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }

        if (id != profile.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return RedirectToAction("Profile");
        }

        if (IsProfileDataInvalid(profile))
        {
            ViewBag.Error = "Both Username and NewPassword cannot be empty";
            return View(profile);
        }

        var user = userRepo.Get(id);
        if (IsInvalidPassword(user, profile.OldPassword))
        {
            ViewBag.Error = "Invalid existing password";
            return View(profile);
        }

        userRepo.Update(profile);

        var authenticationUser = CreateAuthenticationUser(profile, user);
        return AddAuthentication(authenticationUser.Username!, authenticationUser.Password!);
    }

    private static bool IsProfileDataInvalid(Profile profile)
    {
        return string.IsNullOrWhiteSpace(profile.Username) && string.IsNullOrWhiteSpace(profile.NewPassword);
    }

    private static bool IsInvalidPassword(User? user, string? oldPassword)
    {
        return user?.Password != oldPassword;
    }

    private static User CreateAuthenticationUser(Profile profile, User? user)
    {
        return new User
        {
            Username = profile.Username ?? user?.Username,
            Password = profile.NewPassword ?? user?.Password
        };
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

    public IActionResult Statistics()
    {
        if (!CheckAuthentication())
        {
            return RedirectToAction("Login", "Admin");
        }
        return View(yearlyVisitRepo.GetAll());
    }

    public IActionResult AddAuthentication(string username, string password)
    {
        var user = userRepo.GetByCredentials(username, password);
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
            uploadRepo.Create(viewmodel);
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
            uploadRepo.Delete(FileName);
        }
        return View("Files", GetAllFiles());
    }

    [HttpPost]
    public IActionResult UpdateMaterialsOrder(string fileOrders)
    {
        var materialFileOrder = JsonConvert.DeserializeObject<List<FileOrder>>(fileOrders);
        if (materialFileOrder?.Count != 0)
        {
            var uploadsToUpdate = (from entry in materialFileOrder
                select new Upload
                {
                    Name = entry.Name,
                    MaterialOrder = entry.Order
                }).ToList();

            uploadRepo.Update(uploadsToUpdate);
        }
        return RedirectToAction("Files", GetAllFiles());
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

            uploadRepo.Update(uploadsToUpdate);
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

        var uploadsList = uploadRepo.GetAll();
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
                IsMaterial = upload?.IsMaterial,
                MaterialOrder = upload?.MaterialOrder
            });
        }

        var orderList = filesViewModels.OrderBy(u => u.MaterialOrder).ToList();
        return orderList;
    }
}
