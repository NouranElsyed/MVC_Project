﻿using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModels model)
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    Name = model.Name,
                    IAgree = model.IAgree


                };
                var Result = await _userManager.CreateAsync(User,model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else 
                {
                    foreach (var error in Result.Errors) 
                    {
                        ModelState.AddModelError(string.Empty,error.Description);
                        return View(model);
                    }
                }
            }
            return View(model);


        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModels model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var Flag = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (Flag)
                    {
                     var Result =  await  _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe,false);
                        if (Result.Succeeded) { return RedirectToAction("Index", "Home"); }
                    }
                    else 
                    {
                    ModelState.AddModelError(string.Empty,"Incorrect Password");

                    }
                }
                else 
                {
                    ModelState.AddModelError(string.Empty,"Email is not existed");
                }
            }
            return View(model);

        }
    }
}
