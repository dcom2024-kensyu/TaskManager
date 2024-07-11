using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TaskManager.Web.Filter;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers
{
    [TypeFilter(typeof(AccessLogFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var role = User.FindFirst(ClaimTypes.Role).Value;
            ViewData["Email"] = email;
            ViewData["Role"] = role;

            return View();
        }

        public IActionResult Privacy()
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
