using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using CoolEvents.Models;
using CoolEvents.Models;

namespace CoolEvents.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
            _roleManager = roleManager;
        }

        public void Run(IServiceProvider serviceProvider, bool shouldDeleteDB)
        {
            if (shouldDeleteDB == true)
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();

                AddRoles();
                AddUsers(serviceProvider);
            }
        }
        public void AddRoles()
        {
            var roles = new List<string> { "Admin", "User" };
            Task<IdentityResult> roleResult;

            foreach (var role in roles)
            {
                Task<bool> hasRole = _roleManager.RoleExistsAsync(role);
                hasRole.Wait();

                if (!hasRole.Result)
                {
                    roleResult = _roleManager.CreateAsync(new IdentityRole(role));
                    roleResult.Wait();
                }
                
            }
        }

        public void AddUsers(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                //admin
                Task<IdentityResult> adminResult = null;
                if (adminResult == null)
                {
                    var admin = new ApplicationUser()
                    {
                        UserName = "petar_ivanov",
                        Email = "petar_ivanov@gmail.com",
                        FirstName = "Petar",
                        LastName = "Ivanov"
                    };
                    adminResult = _userManager.CreateAsync(admin, "Petar@123");
                    adminResult.Wait();
                    if (adminResult.Result.Succeeded)
                    {
                        Task<IdentityResult> newUserRole = _userManager.AddToRoleAsync(admin, "Admin");
                        newUserRole.Wait();
                    }
                }
            }
        }
    }
}
