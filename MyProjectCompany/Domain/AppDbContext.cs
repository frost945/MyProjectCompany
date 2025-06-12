using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyProjectCompany.Domain.Entities;

namespace MyProjectCompany.Domain
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
    {
        public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string adminName = "admin";
            string roleAdminId = "e44167ff-ff18-4fe5-b215-2542f3ca1968";
            string userAdminId = "8801f965-e936-4f3f-b09b-7bd5f49af683";

            //добавляем роль админа сайта
            builder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = roleAdminId,
                Name= adminName,
                NormalizedName = adminName.ToUpper()
            });

            //добавляем нового IdentityUser в качестве админа сайта
            builder.Entity<IdentityUser>().HasData(new IdentityUser()
            {
                Id = userAdminId,
                UserName = adminName,
                NormalizedUserName = adminName.ToUpper(),
                Email = "admin@admin.com",
                NormalizedEmail = "admin@admin.com",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(new IdentityUser(), adminName),
                SecurityStamp = string.Empty,
                PhoneNumberConfirmed = true
            });

            //определяем админа в соответствующую роль
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId = roleAdminId,
                UserId = userAdminId
            });
        }
    }
}
