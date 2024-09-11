using GestionDesConges.Data;
using GestionDesConges.Data.enums;
using GestionDesConges.Data.Providers;
using GestionDesConges.Models;
using GestionDesConges.Repository;
using GestionDesConges.Repository.Interfaces;
using GestionDesConges.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionDesConges.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepository, IEmployeeRepository employeeRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            int employeeId = (int)HttpContext.Session.GetInt32("employeeId");
            int numberOfDays = await _leaveRequestRepository.GetNumberOfDays(employeeId);

            bool canCreateNewLeaveRequest = numberOfDays < 25;

            ViewBag.CanCreateNewLeaveRequest = canCreateNewLeaveRequest;
            IEnumerable<LeaveRequest> leaveRequests = await _leaveRequestRepository.FindByEmployee((int)HttpContext.Session.GetInt32("employeeId"));
            return View(leaveRequests);
        }
        public async Task<IActionResult> Details(int id)
        {
            LeaveRequest leaveRequest = await _leaveRequestRepository.FindById(id);
            return View(leaveRequest);
        }
        public async Task<IActionResult> Create()
        {
            int? employeeId = HttpContext.Session.GetInt32("employeeId");

            if (employeeId == null)
            {
                // Handle the case where employeeId is not set in the session.
                return RedirectToAction("Login", "Account");
            }

            var employee = await _employeeRepository.FindById((int)employeeId);
            ViewBag.EmployeeName = employee.FullName; // Passing the full name to the view
            ViewBag.EmployeeId = employee.EmployeeId; // Passing the ID to the view

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveRequestViewModel leaveRequest)
        {
            if (!ModelState.IsValid)
            {
                var employee = await _employeeRepository.FindById((int)leaveRequest.EmployeeId);
                ViewBag.EmployeeName = employee.FullName; // Passing the full name to the view
                ViewBag.EmployeeId = employee.EmployeeId; 
                return View(leaveRequest);
            }

            // Retrieve the list of holidays
            var holidays = HolidayProvider.GetHolidays();
            int numberOfDays = 0;

            // Calculate the number of working days excluding holidays
            for (DateTime date = leaveRequest.StartDate; date <= leaveRequest.EndDate; date = date.AddDays(1))
            {
                if (!holidays.Contains(date))
                {
                    numberOfDays++;
                }
            }

            int daysTaken = await _leaveRequestRepository.GetNumberOfDays((int)HttpContext.Session.GetInt32("employeeId"));

            if ((numberOfDays + daysTaken) > 25){
                return View("AddError");
            }
            LeaveRequest lv = new LeaveRequest
            {
                EmployeeId = leaveRequest.EmployeeId,
                Employee = await _employeeRepository.FindById(leaveRequest.EmployeeId),
                AbsenceType = leaveRequest.AbsenceType,
                NumberOfDays = numberOfDays,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
                Comments = leaveRequest.Comments,
                Status = ApprovalStatus.Pending
            };

            // Add the leave request to the repository
            _leaveRequestRepository.Add(lv);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            LeaveRequest leaveRequest= await _leaveRequestRepository.FindByIdNoTracking(id);
            if (leaveRequest == null) return View("Error");
            var editLR = new EditLeaveRequestViewModel
            {
                LeaveRequestId = id,
                EmployeeId = leaveRequest.EmployeeId,
                AbsenceType = leaveRequest.AbsenceType,
                NumberOfDays = leaveRequest.NumberOfDays,
                StartDate = leaveRequest.StartDate,
                Reason = leaveRequest.Reason
            };
            ViewBag.EmployeeId = new SelectList(await _employeeRepository.GetAll(), "EmployeeId", "FullName");


            return View(editLR);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditLeaveRequestViewModel leaverequest)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit department");
                return View("Edit", leaverequest);
            }
            var upleaverequest = await _leaveRequestRepository.FindByIdNoTracking(id);
            if (leaverequest != null)
            {
                var upemp = new LeaveRequest
                {
                    LeaveRequestId = id,
                    EmployeeId = leaverequest.EmployeeId,
                    AbsenceType = leaverequest.AbsenceType,
                    NumberOfDays = leaverequest.NumberOfDays,
                    StartDate = leaverequest.StartDate,
                    EndDate = leaverequest.StartDate.AddDays(leaverequest.NumberOfDays),
                    Reason = leaverequest.Reason,
                    Status = ApprovalStatus.Pending,
                    Comments = ""
                };
                _leaveRequestRepository.Update(upemp);
                return RedirectToAction("index");
            }
            return View(leaverequest);

        }
        public async Task<IActionResult> Delete(int id)
        {
            var leaveRequest = await _leaveRequestRepository.FindById(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }
            return View(leaveRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequest = await _leaveRequestRepository.FindById(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            _leaveRequestRepository.Delete(leaveRequest);
            return RedirectToAction(nameof(Index));
        }
    }
}
