using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using TaskManager.Web.Models;

namespace TaskManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //dev�u�����`
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ToDoDbContext>();

            // Cookie�F�؃T�[�r�X��ǉ�
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Account/Login";
                    option.AccessDeniedPath = "/Account/Forbidden";
                });

            // ���ׂẴ��[�U�[�̔F�؂�v������t�H�[���o�b�N�F�|���V�[��ݒ�
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
    }
}
