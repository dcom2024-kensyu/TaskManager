using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "ログイン情報に誤りがあります。");
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}