using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public DepartmentController( IUnitOfWork unitOfWork,IWebHostEnvironment env) 
        {
  
            _unitOfWork = unitOfWork;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var department = await _unitOfWork.departmentRepository.GetAllAsync();
            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) 
            {
                await _unitOfWork.departmentRepository.AddAsync(department);
                await _unitOfWork.completeAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public async Task<IActionResult> Details(int? id,string ViewName = "Details") 
        {
            if (id is null) { return BadRequest(); }

            var department = await _unitOfWork.departmentRepository.GetByIdAsync(id.Value);
            if (department is null) { return NotFound(); }
            return View(ViewName,department);

        }
        [HttpGet]
        public async  Task<IActionResult> Update(int? id)
        {
            //if (id is null) { return BadRequest(); }

            //var department = await _unitOfWork.departmentRepository.GetByIdAsync(id.Value);
            //if (department is null) { return NotFound(); }
            return await Details(id, "Update");// View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Department department, [FromRoute] int id)
        {
            if (id != department.Id) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.departmentRepository.Update(department);
                    await _unitOfWork.completeAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(System.Exception ex)
                { 
                ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }
      
        public async Task<IActionResult> Delete(int? id) 
        {
            return await Details(id,"Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Department department, [FromRoute] int id) 
        {
                if ((id != department.Id))
                { return BadRequest(); }
                try
                {
                    _unitOfWork.departmentRepository.Delete(department);
                    await  _unitOfWork.completeAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }

        }

    }
}
