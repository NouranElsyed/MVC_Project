using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,IMapper mapper) 
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

                    }).ToListAsync();
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
                return View(new List<UserViewModel> ());

            }

        }
        public async Task<IActionResult> Details(string Id, string viewName = "Detils") 
        {
            if (Id is null) return BadRequest();
            var User = await _userManager.FindByIdAsync(Id);
            if(User is null)
                return NotFound();
            var MappedUser = _mapper.Map<ApplicationUser,UserViewModel>(User);
            return View(MappedUser);

        }
    }
}
