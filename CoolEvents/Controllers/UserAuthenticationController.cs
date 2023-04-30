using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoolEvents.Models;
using CoolEvents.Repository.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CoolEvents.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserAuthenticationController(IUserAuthenticationService service, SignInManager<ApplicationUser> signInManager) { 
            this._service = service;
            this._signInManager = signInManager;
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Role = "User";
            var result = await _service.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login (LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Events");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Events");
        }



    }
}
