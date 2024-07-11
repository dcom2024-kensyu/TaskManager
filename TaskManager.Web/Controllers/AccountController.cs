using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Web.Models;
using TaskManager.Web.Filter;

namespace TaskManager.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ToDoDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ToDoDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
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

            _logger.LogInformation(
                $"Controller:{nameof(AccountController)} " +
                $"Action:{nameof(Login)} " +
                $"User:{user.UserName} Success!");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (userName is not null)
            {
                _logger.LogInformation(
                    $"Controller:{nameof(AccountController)} " +
                    $"Action:{nameof(Logout)} " +
                    $"User:{userName} Success!");
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Forbidden() => View();

        private IEnumerable<Claim> CreateClaims(User user)
        {
            yield return new Claim(ClaimTypes.Email, user.Email!);
            //yield return new Claim(ClaimTypes.NameIdentifier, $"{user.Id}");
            yield return new Claim(ClaimTypes.Name, user.UserName);
            foreach (var role in user.Roles)
            {
                yield return new Claim(ClaimTypes.Role, role.RoleName);
            }
        }
    }
}