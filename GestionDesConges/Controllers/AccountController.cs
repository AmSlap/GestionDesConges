using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using GestionDesConges.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionDesConges.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmployeeLoginRepository _employeeLoginRepository;

        public AccountController(IEmployeeLoginRepository employeeLoginRepository)
        {
            _employeeLoginRepository = employeeLoginRepository;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var employee = await _employeeLoginRepository.FindByUsernameAndPw(loginModel.UserName, loginModel.Password);

                if (employee != null)
                {
                    // Set session variables
                    HttpContext.Session.SetString("FullName", employee.Employee.FullName);
                    HttpContext.Session.SetInt32("employeeId", employee.EmployeeId);
                    HttpContext.Session.SetString("Role", employee.Employee.Position.ToString()); // Assuming Role is a string property in Employee

                    // Set claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.Employee.FullName),
                new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
                new Claim("Position", employee.Employee.Position.ToString()) // Add Position claim
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(loginModel);
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            // Clear session and sign out the user
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
