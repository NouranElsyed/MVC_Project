using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IUnitOfWork unitOfWork,IWebHostEnvironment env/*,IDepartmentRepository departmentRepository*/,IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;

            //_departmentRepository = departmentRepository;
        }
        public async Task<IActionResult> Index(string InputSearch)
        {
            var Employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(InputSearch)) 
            {
                Employees = await _unitOfWork.employeeRepository.GetAllAsync();
            } else {
                Employees = _unitOfWork.employeeRepository.GetEmployeeByName(InputSearch.ToLower());
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
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) 
            {
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image,"Images");
                var EmployeeMapped = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await _unitOfWork.employeeRepository.AddAsync(EmployeeMapped);
                var count = await _unitOfWork.completeAsync();
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
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) { return BadRequest(); }

            var employee =await  _unitOfWork.employeeRepository.GetByIdAsync(id.Value);
            if (employee is null) { return NotFound(); }
            var EmployeeMapped = _mapper.Map<Employee,EmployeeViewModel>(employee);

            return View(ViewName, EmployeeMapped);

        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) { return BadRequest(); }

            var employee = await _unitOfWork.employeeRepository.GetByIdAsync(id.Value);
            //ViewData["departments"] = _departmentRepository.GetAll();
            if (employee is null) { return NotFound(); }
            var EmployeeMapped = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(EmployeeMapped);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                    var EmployeeMapped = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);

                    _unitOfWork.employeeRepository.Update(EmployeeMapped);
                    await _unitOfWork.completeAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

        public Task<IActionResult> Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            var EmployeeMapped = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

            if ((id != EmployeeMapped.Id))
            { return BadRequest(); }
            try
            {

                _unitOfWork.employeeRepository.Delete(EmployeeMapped);
                var count = await _unitOfWork.completeAsync();
                if (count >0) 
                {
                DocumentSettings.DeleteFile(employeeVM.ImageName,"Images");
                }
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
