using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentifyRole> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<IdentifyRole> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                var users = await _userManager.Users.Select
                    (U => new UserViewModel
                    {
                        id = U.Id,
                        Name = U.Name,
                        Email = U.Email,
                        PhoneNumber = U.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(U).Result

                    }).AsNoTracking().ToListAsync();
                return View(users);

            }
            else
            {
                var user = await _userManager.FindByEmailAsync(searchValue);
                if (user == null) { user = await _userManager.FindByNameAsync(searchValue); }

                if (user is not null)
                {
                    var MappedUser = new UserViewModel()
                    {
                        id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(user).Result
                    };
                    return View(new List<UserViewModel> { MappedUser });

                }
                return View(new List<UserViewModel>());

            }

        }
        public async Task<IActionResult> Details(string Id, string viewName = "Details")
        {
            if (Id is null) return BadRequest();
            var User = await _userManager.FindByIdAsync(Id);
            if (User is null)
                return NotFound();
            var MappedUser = _mapper.Map<IdentifyRole, UserViewModel>(User);
            return View(MappedUser);

        }
        public async Task<IActionResult> Update(string Id, string viewName = "Details")
        { return await Details(Id, "Update"); }
        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel model, [FromRoute]string Id )
        { 
            if(Id != model.id) return BadRequest();
            if (ModelState.IsValid) 
            {
                try
                {
                    var User = await _userManager.FindByIdAsync(Id);
                    User.PhoneNumber = model.PhoneNumber;
                    User.Name = model.Name;
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                } 
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(string Id)
        { return await Details(Id, "Delete"); }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string Id)
        {

            try
            {
                var User = await _userManager.FindByIdAsync(Id);
                await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
