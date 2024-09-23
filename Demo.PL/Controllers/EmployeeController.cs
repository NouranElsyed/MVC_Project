using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository,IWebHostEnvironment env/*,IDepartmentRepository departmentRepository*/,IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _env = env;
            _mapper = mapper;

            //_departmentRepository = departmentRepository;
        }
        public IActionResult Index(string InputSearch)
        {
            var Employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(InputSearch)) 
            {
                Employees = _employeeRepository.GetAll();
            } else {
                Employees = _employeeRepository.GetEmployeeByName(InputSearch.ToLower());
            }
            //ViewBag["Message"] = "hello viewbag";
            //ViewBag.Message = "hello viewbag";
            TempData.Keep();
            var EmployeeMapped = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel> >(Employees);
            return View(EmployeeMapped);
        }
        [HttpGet]
        public IActionResult Create()
        {
           // ViewData["departments"] = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) 
            {
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image,"Images");
                var EmployeeMapped = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                var count =_employeeRepository.Add(EmployeeMapped);
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
            var EmployeeMapped = _mapper.Map<Employee,EmployeeViewModel>(employee);

            return View(ViewName, EmployeeMapped);

        }
        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id is null) { return BadRequest(); }

            var employee = _employeeRepository.GetById(id.Value);
            //ViewData["departments"] = _departmentRepository.GetAll();
            if (employee is null) { return NotFound(); }
            var EmployeeMapped = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(EmployeeMapped);
        }
        [HttpPost]
        public IActionResult Update(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                    var EmployeeMapped = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);

                    _employeeRepository.Update(EmployeeMapped);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            var EmployeeMapped = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

            if ((id != EmployeeMapped.Id))
            { return BadRequest(); }
            try
            {

                _employeeRepository.Delete(EmployeeMapped);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employeeVM);
        }

    }
}
