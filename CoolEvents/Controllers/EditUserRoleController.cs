using CoolEvents.Data;
using CoolEvents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoolEvents.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EditUserRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public EditUserRoleController(UserManager<ApplicationUser> usermanager, ApplicationDbContext context, RoleManager<IdentityRole> rolemanager)
        {
            _userManager = usermanager;
            _context = context;
            _roleManager = rolemanager;
        }

        [HttpGet]
        public IActionResult Index()
        {

            AdminIndexModel model = new(_userManager)
            {
                Users = _userManager.Users.ToList(),
                UserRoles = new Dictionary<ApplicationUser, string>()
            };
            model.AddToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult EditRole(string userId)
        {
            ViewBag.Roles = new SelectList(_context.Roles.AsNoTracking().ToList(), nameof(IdentityRole.Id), nameof(IdentityRole.Name));
            EditRoleViewModel viewModel = new EditRoleViewModel()
            {
                Id = _userManager.FindByIdAsync(userId).Result.Id,
                Email = _userManager.FindByIdAsync(userId).Result.Email,
                Username = _userManager.FindByIdAsync(userId).Result.UserName
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel viewModel, string roleId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                var role = _roleManager.FindByIdAsync(roleId).Result.Name;
                await _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(user).Result);
                await _userManager.AddToRoleAsync(user, role);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
