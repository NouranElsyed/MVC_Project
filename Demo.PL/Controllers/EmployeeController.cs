using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;


namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeRepository employeeRepository,IWebHostEnvironment env)
        {
            _employeeRepository = employeeRepository;
            _env = env;
        }
        public IActionResult Index()
        {
            var Employees = _employeeRepository.GetAll();   
            return View(Employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) 
            {
                _employeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null) { return BadRequest(); }

            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) { return NotFound(); }
            return View(ViewName, employee);

        }
        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id is null) { return BadRequest(); }

            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) { return NotFound(); }
            return View(employee);
        }
        [HttpPost]
        public IActionResult Update(Employee employee, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Employee employee, [FromRoute] int id)
        {
            if ((id != employee.Id))
            { return BadRequest(); }
            try
            {
                _employeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employee );
        }

    }
}
