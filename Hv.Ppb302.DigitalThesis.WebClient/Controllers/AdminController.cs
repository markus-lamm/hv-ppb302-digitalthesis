using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Hv.Ppb302.DigitalThesis.WebClient.Data;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserRepository _userRepository;

        public AdminController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            if (!CheckAuthentication())
            {
                return RedirectToAction("Login", "Admin");
            }
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
    }
}
