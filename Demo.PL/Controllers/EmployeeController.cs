using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Runtime.InteropServices;


namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _env;
        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository,IWebHostEnvironment env/*,IDepartmentRepository departmentRepository*/)
        {
            _employeeRepository = employeeRepository;
            _env = env;
            //_departmentRepository = departmentRepository;
        }
        public IActionResult Index(string InputSearch)
        {
            var Employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(InputSearch)) 
            {
                Employees = _employeeRepository.GetAll();
            } else {
                Employees = _employeeRepository.GetEmployeeByAddress(InputSearch);
            }
            //ViewBag["Message"] = "hello viewbag";
            //ViewBag.Message = "hello viewbag";
            TempData.Keep();
             Employees = _employeeRepository.GetAll();   
            return View(Employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
           // ViewData["departments"] = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) 
            {
                var count =_employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee created successfully";
                }
                else 
                {
                    TempData["Message"] = "An Error occured";

                }
                
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
            //ViewData["departments"] = _departmentRepository.GetAll();
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
