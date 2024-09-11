using GestionDesConges.Data;
using GestionDesConges.Models;
using GestionDesConges.Repository;
using GestionDesConges.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionDesConges.Controllers
{
    [Authorize(Policy = "HRPolicy")]
    public class ApprovalController : Controller
    {
        
        private readonly IApprovalRepository _approvalRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public ApprovalController(IApprovalRepository approvalRepository, IEmployeeRepository employeeRepository,ILeaveRequestRepository leaveRequestRepository)
        {
            
            _approvalRepository = approvalRepository;
            _employeeRepository = employeeRepository;
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Approval> approvals = await _approvalRepository.GetAll();
            return  View(approvals);
        }
        public async Task<IActionResult> Details(int id)
        {
            Approval approval = await _approvalRepository.FindById(id);
            return View(approval);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.ApproverId = new SelectList(await _employeeRepository.FindByPosition(Data.enums.Position.HumanResources), "EmployeeId", "FullName");
            ViewBag.LeaveRequestId = new SelectList(await _leaveRequestRepository.GetAll(), "LeaveRequestId", "LeaveRequestId");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Approval approval)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ApproverId = new SelectList(await _employeeRepository.FindByPosition(Data.enums.Position.HumanResources), "EmployeeId", "FullName");
                ViewBag.LeaveRequestId = new SelectList(await _leaveRequestRepository.GetAll(), "LeaveRequestId", "AbsenceType");
                return View();
            }
            _approvalRepository.Add(approval);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approval = await _approvalRepository.FindById(id.Value);
            if (approval == null)
            {
                return NotFound();
            }

            return View(approval);
        }

        // POST: Approval/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var approval = await _approvalRepository.FindById(id);
            if (approval != null)
            {
                _approvalRepository.Delete(approval);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
