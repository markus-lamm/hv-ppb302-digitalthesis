using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hv.Ppb302.DigitalThesis.WebClient.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                // Autentisering lyckades, skapa en claims identity med användarens id
                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "admin") }, CookieAuthenticationDefaults.AuthenticationScheme);

                // Skapa en autentiseringscookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                // Omdirigera till önskad vy
                return RedirectToAction("Index");
            }

            // Felaktiga autentiseringsuppgifter, visa felmeddelande eller hantera på lämpligt sätt
            TempData["Message"] = "Invalid username or password";
            return RedirectToAction("Index", "Home");


        }
        public async Task<IActionResult> Logout()
        {
            // Logga ut användaren genom att ta bort autentiseringscookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
