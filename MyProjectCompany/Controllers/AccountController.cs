﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProjectCompany.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MyProjectCompany.Controllers
{
    public class AccountController:Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            await _signInManager.SignOutAsync();//намеренный выход с сессии, перед новым логином

            //раздел сайта (url) куда перенаправить пользователя после успешного логина
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            //раздел сайта (url) куда перенаправить пользователя после успешного логина
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            SignInResult result= await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!, model.RememberMe, false);

            if (result.Succeeded)
                return Redirect(returnUrl ?? "/");

            ModelState.AddModelError(string.Empty, "Неправильний пароль або логін");
            return View(model);
        }

        [HttpPost]
        public  async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
