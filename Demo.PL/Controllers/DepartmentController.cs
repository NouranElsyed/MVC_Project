using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository) 
        {
            _departmentRepository = departmentRepository;

        }
        public IActionResult Index()
        {
            var department = _departmentRepository.GetAll();
            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) 
            {
                _departmentRepository.Add(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public IActionResult Details(int? id,string ViewName = "Details") 
        {
            if (id is null) { return BadRequest(); }

            var department = _departmentRepository.GetById(id.Value);
            if (department is null) { return NotFound(); }
            return View(ViewName,department);

        }
        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id is null) { return BadRequest(); }

            var department = _departmentRepository.GetById(id.Value);
            if (department is null) { return NotFound(); }
            return View(department);
        }
        [HttpPost]
        public IActionResult Update(Department department, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch(System.Exception ex)
                { 
                ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }
      
        public IActionResult Delete(int? id) 
        {
            return Details(id,"Delete");
        }
        [HttpPost]
        public IActionResult Delete(Department department, [FromRoute] int id) 
        {
                if ((id != department.Id))
                { return BadRequest(); }
                try
                {
                    _departmentRepository.Delete(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            return View(department);
        }

    }
}
