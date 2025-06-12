using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyProjectCompany.Domain;
using MyProjectCompany.Domain.Repositories.Abstract;
using MyProjectCompany.Domain.Repositories.EntityFramework;
using MyProjectCompany.Infrastructure;
using Serilog;

namespace MyProjectCompany
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            //подключаем в конфигурацию файл appsettings.json
            IConfigurationBuilder configBuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //оборачиваем секцию project в объектную форму
            IConfiguration configuration =configBuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            //подключаем контекст БД
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString)
            //на момент создания приложения в данной версии EF был баг, хотя ошибки нет, поэтому подавляем предупреждения
            .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            builder.Services.AddTransient<IServiceCategoriesRepository, EFServiceCategoriesRepository>();
            builder.Services.AddTransient<IServicesRepository, EFServicesRepository>();
            builder.Services.AddTransient<DataManager>();

            //подключаем Identity систему
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //настраиваем Authcookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            //подключаем функционал контроллеров
            builder.Services.AddControllersWithViews();

            //подключаем логирование
            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)); 

            //собираем конфигурацию
            WebApplication app = builder.Build();

            //сразу используем логирование
            app.UseSerilogRequestLogging();

            //подключаем обработку исключений
            if(app.Environment.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            //подключаем использование статичных файлов
            app.UseStaticFiles();

            //подключаем маршрутизацию
            app.UseRouting();

            //подключаем авторизацию и аутентификацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


            await app.RunAsync();
        }
    }
}
