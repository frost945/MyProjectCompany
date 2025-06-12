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

            //���������� � ������������ ���� appsettings.json
            IConfigurationBuilder configBuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //����������� ������ project � ��������� �����
            IConfiguration configuration =configBuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            //���������� �������� ��
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString)
            //�� ������ �������� ���������� � ������ ������ EF ��� ���, ���� ������ ���, ������� ��������� ��������������
            .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            builder.Services.AddTransient<IServiceCategoriesRepository, EFServiceCategoriesRepository>();
            builder.Services.AddTransient<IServicesRepository, EFServicesRepository>();
            builder.Services.AddTransient<DataManager>();

            //���������� Identity �������
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //����������� Authcookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            //���������� ���������� ������������
            builder.Services.AddControllersWithViews();

            //���������� �����������
            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)); 

            //�������� ������������
            WebApplication app = builder.Build();

            //����� ���������� �����������
            app.UseSerilogRequestLogging();

            //���������� ��������� ����������
            if(app.Environment.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            //���������� ������������� ��������� ������
            app.UseStaticFiles();

            //���������� �������������
            app.UseRouting();

            //���������� ����������� � ��������������
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


            await app.RunAsync();
        }
    }
}
