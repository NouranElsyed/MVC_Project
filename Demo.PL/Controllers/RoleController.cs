using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentifyRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentifyRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                var Roles = await _roleManager.Roles.ToListAsync();
                 var MppedRole = _mapper.Map<IEnumerable<IdentifyRole>,IEnumerable<RoleViewModel>>(Roles);
                return View(Roles);

            }
            else
            {
                var Role = await _roleManager.FindByNameAsync(searchValue);
                var RoleMapped = _mapper.Map<IdentifyRole,RoleViewModel>(Role);
                return View(new List<RoleViewModel>() { RoleMapped });

            }

        }


        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel rolemodel)
        {
            if (ModelState.IsValid)
            {
              
                var RoleMapped = _mapper.Map<RoleViewModel, IdentifyRole>(rolemodel);
                await _roleManager.CreateAsync(RoleMapped);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Details(string Id, string viewName = "Details")
        {
            if (Id is null) return BadRequest();
            var Role = await _roleManager.FindByIdAsync(Id);
            if (Role is null)
                return NotFound();
            var MappedRole = _mapper.Map<IdentifyRole, RoleViewModel>(Role);
            return View(MappedRole);

        }
        public async Task<IActionResult> Update(string Id, string viewName = "Details")
        { return await Details(Id, "Update"); }
        [HttpPost]
        public async Task<IActionResult> Update(RoleViewModel model, [FromRoute] string Id)
        {
            if (Id != model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    //var User = await _roleManager.FindByIdAsync(Id);
                    //User.PhoneNumber = model.PhoneNumber;
                    //User.Name = model.Name;
                    //await _userManager.UpdateAsync(User);
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
                var User = await _roleManager.FindByIdAsync(Id);
                await _roleManager.DeleteAsync(User);
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
