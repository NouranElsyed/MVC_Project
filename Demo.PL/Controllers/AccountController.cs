using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
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
        #region Register
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
        #endregion
        #region Login
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
        #endregion
        #region SignOut
        public new async Task<IActionResult> SignOut() 
        {
            await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
        }
        #endregion
        #region ForgetPassword
        public async Task<IActionResult> ForgetPassword()
        {
            return  View();
        }
    [HttpPost]
    public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
    {

        if (ModelState.IsValid)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is not null)
            {   var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    var ResetPasswordLink = Url.Action("ResetPassword","Account",new { email = User.Email , Token  = token},Request.Scheme);
                var email = new Email()
                {
                    Subject = "Reset Passwoed",
                    To = model.Email,
                    Body = "Link"
                };
                EmailSettings.SendEmail(email);
                return RedirectToAction(nameof(CheckYourInbox));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email is not existed");
            }

        }
 
    
            return View("ForgetPassword", model);
        
    }
        #endregion

        public IActionResult CheckYourInbox() 
        {
            return View();
        }

        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"]= email;
            TempData["token"] = token;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var User = await _userManager.FindByEmailAsync(email);
                var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }


    }
}
