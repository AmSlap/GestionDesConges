using GestionDesConges.Data;
using GestionDesConges.Data.enums;
using GestionDesConges.Models;
using GestionDesConges.Repository;
using GestionDesConges.Repository.Interfaces;
using GestionDesConges.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionDesConges.Controllers
{
    [Authorize(Policy = "HRPolicy")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Employee> employees = await _employeeRepository.GetAll();
            return View(employees);
        }
        public async Task<IActionResult> Details(int id)
        {
            Employee employee = await _employeeRepository.FindById(id);
            return View(employee);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Position = Enum.GetValues(typeof(Position))
                                  .Cast<Position>()
                                  .Select(p => new SelectListItem
                                  {
                                      Value = ((int)p).ToString(),
                                      Text = p.ToString()
                                  }).ToList();

            ViewBag.DepartmentId = new SelectList(await _departmentRepository.GetAll(), "DepartmentId", "DepartmentName");
            return View();

        }
        [HttpPost]
        public async Task<ActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                return View(employee);
            }
            _employeeRepository.Add(employee);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Edit(int id)
        {
            Employee employee = await _employeeRepository.FindById(id);
            if (employee == null) return View("Error");
            var editemp= new EditEmployeeViewModel
            {
                Id = id,
                Name = employee.FullName,
                position = employee.Position,
                Email = employee.Email,
                DepId = employee.DepartmentId,
            };
            ViewBag.DepartmentId = new SelectList(await _departmentRepository.GetAll(), "DepartmentId", "DepartmentName");

            return View(editemp);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditEmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit department");
                return View("Edit", employee);
            }
            var emp = await _employeeRepository.FindByIdNoTracking(id);
            if (emp != null)
            {
                var upemp = new Employee
                {
                    EmployeeId = id,
                    FullName = employee.Name,
                    Position = employee.position,
                    Email = employee.Email,
                    DepartmentId = employee.DepId,
                    Department = await _departmentRepository.FindById(employee.DepId)
                };
                _employeeRepository.Update(upemp);
                return RedirectToAction("index");
            }
            return View(employee);

        }
    }
}
