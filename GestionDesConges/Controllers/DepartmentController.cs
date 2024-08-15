using GestionDesConges.Data;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionDesConges.Controllers
{
    [Authorize(Policy = "HRPolicy")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Department> departments = await _departmentRepository.GetAll();
            return View(departments);
        }
        public async Task<IActionResult> Details(int id)
        {
            Department department = await _departmentRepository.FindById(id);
            return View(department);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentRepository.Add(department);
                return RedirectToAction("Index");
            }

            // If validation fails, redisplay the form
            return View(department);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Department department = await _departmentRepository.FindById(id);
            if (department == null) return View("Error");
            var editdep = new Department
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };
            return View(editdep);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit department");
                return View("Edit", department);
            }
            var dep = await _departmentRepository.FindByIdNoTracking(id);
            if (dep != null)
            {
                var updep = new Department
                {
                    DepartmentId = id,
                    DepartmentName = department.DepartmentName
                };
                _departmentRepository.Update(updep);
                return RedirectToAction("index");
            }
            return View(department);

        }
    }
}
