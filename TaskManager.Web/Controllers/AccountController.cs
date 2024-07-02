using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ToDoDbContext _context;

        public AccountController(ToDoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "ログイン情報に誤りがあります。");
                return View();
            }

            var user = _context.Users
                .Include(e => e.Roles)
                .Where(e => e.Email == email && e.Password == password)
                .FirstOrDefault();

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "ログイン情報に誤りがあります。");
                return View();
            }

            var identity = new ClaimsIdentity(CreateClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Forbidden() => View();

        private IEnumerable<Claim> CreateClaims(User user)
        {
            yield return new Claim(ClaimTypes.Email, user.Email!);
            foreach (var role in user.Roles)
            {
                yield return new Claim(ClaimTypes.Role, role.RoleName);
            }
        }
    }
}