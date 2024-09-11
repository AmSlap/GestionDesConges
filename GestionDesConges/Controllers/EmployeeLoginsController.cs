using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionDesConges.Data;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using GestionDesConges.ViewModels;

namespace GestionDesConges.Controllers
{
    public class EmployeeLoginsController : Controller
    {
        private readonly IEmployeeLoginRepository _employeeLoginRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeLoginsController(IEmployeeLoginRepository employeeLoginRepository, IEmployeeRepository employeeRepository)
        {
            _employeeLoginRepository = employeeLoginRepository;
            _employeeRepository = employeeRepository;
        }

        // GET: EmployeeLogins
        public async Task<IActionResult> Index()
        {
            
            return View(await _employeeLoginRepository.GetAll());
        }

        // GET: EmployeeLogins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeLogin = await _employeeLoginRepository.FindByIDNoTracking(id);
            if (employeeLogin == null)
            {
                return NotFound();
            }

            return View(employeeLogin);
        }

        // GET: EmployeeLogins/Create
        public async Task<IActionResult> Create()
        {
            var employeesWithoutLogin = await _employeeRepository.GetEmployeesWithoutLogin();
            ViewBag.EmployeeId = new SelectList(employeesWithoutLogin, "EmployeeId", "FullName"); // Assuming FullName is the employee's name field
            return View();
        }

        // POST: EmployeeLogins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLoginViewModel employeeLogin)
        {
            if (ModelState.IsValid)
            {
                EmployeeLogin el = new EmployeeLogin
                {
                    EmployeeId = employeeLogin.EmployeeId,
                    Username = employeeLogin.Username,
                    PasswordHash = employeeLogin.PasswordHash, // Ensure the password is hashed
                    LastLoginDate = DateTime.Now
                };
                _employeeLoginRepository.Add(el);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EmployeeId = new SelectList(await _employeeRepository.GetAll(), "EmployeeId", "Email", employeeLogin.EmployeeId);
            return View(employeeLogin);
        }

        // GET: EmployeeLogins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeLogin = await _employeeLoginRepository.FindByIDNoTracking(id);
            if (employeeLogin == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(await _employeeRepository.GetAll(), "EmployeeId", "Email", employeeLogin.EmployeeId);
            return View(employeeLogin);
        }

        // POST: EmployeeLogins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeLoginId,EmployeeId,Username,PasswordHash,LastLoginDate")] EmployeeLogin employeeLogin)
        {
            if (id != employeeLogin.EmployeeLoginId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeeLoginRepository.Update(employeeLogin);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await EmployeeLoginExists(employeeLogin.EmployeeLoginId))
                    {
                        throw;
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(await _employeeRepository.GetAll(), "EmployeeId", "Email", employeeLogin.EmployeeId);
            return View(employeeLogin);
        }

        // GET: EmployeeLogins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeLogin = await _employeeLoginRepository.FindByIDNoTracking(id);
            if (employeeLogin == null)
            {
                return NotFound();
            }

            return View(employeeLogin);
        }

        // POST: EmployeeLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeLogin = await _employeeLoginRepository.FindByIDNoTracking(id);
            if (employeeLogin != null)
            {
                _employeeLoginRepository.Delete(employeeLogin);
            }

            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EmployeeLoginExists(int id)
        {
            return  _employeeLoginRepository.FindByIDNoTracking(id) != null;
        }
    }
}
