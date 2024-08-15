using System.Diagnostics;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using GestionDesConges.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionDesConges.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(ILeaveRequestRepository leaveRequestRepository, IEmployeeRepository employeeRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            int employeeId = (int)HttpContext.Session.GetInt32("employeeId");

            
            int totalLeaveDays = 25; // Example: 30 days per year

            // Calculate leave days taken
            int leaveDaysTaken = await _leaveRequestRepository.GetNumberOfDays(employeeId, DateTime.Now.Year);

            // Calculate remaining leave days
            int remainingLeaveDays = totalLeaveDays - leaveDaysTaken;

            // Get recent leave requests
            var leaveRequests = await _leaveRequestRepository.FindByEmployee(employeeId);

            var model = new DashboardViewModel
            {
                TotalLeaveDays = totalLeaveDays,
                LeaveDaysTaken = leaveDaysTaken,
                RemainingLeaveDays = remainingLeaveDays,
                RecentLeaveRequests = leaveRequests
            };

            return View(model);
        }
    }
}
