using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using NLog;
using NLog.Web;
using TaskManager.Web.Models;
using TaskManager.Web.Services;

namespace TaskManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Early init of NLog to allow startup and exception logging, before host is built
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                builder.Services.AddDbContext<ToDoDbContext>();
                builder.Services.AddTransient<StatisticsService>();

                // NLogをDI（依存性の注入）で使用できるように設定
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Cookie認証サービスを追加
                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(option =>
                    {
                        option.LoginPath = "/Account/Login";
                        option.AccessDeniedPath = "/Account/Forbidden";
                    });

                // すべてのユーザーの認証を要求するフォールバック認可ポリシーを設定
                // https://learn.microsoft.com/ja-jp/aspnet/core/security/authorization/secure-data?view=aspnetcore-8.0
                builder.Services.AddAuthorization(option =>
                {
                    option.FallbackPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");

                app.Run();
            }
            catch (Exception ex)
            {
                // NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}
