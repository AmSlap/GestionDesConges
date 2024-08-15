using GestionDesConges.Data.enums;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = "HRPolicy")]
public class DecisionController : Controller
{
    private readonly IApprovalRepository _approvalRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILeaveRequestRepository _leaveRequestRepository;

    public DecisionController(IApprovalRepository approvalRepository, IEmployeeRepository employeeRepository, ILeaveRequestRepository leaveRequestRepository)
    {
        _approvalRepository = approvalRepository;
        _employeeRepository = employeeRepository;
        _leaveRequestRepository = leaveRequestRepository;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<LeaveRequest> leaveRequests = await _leaveRequestRepository.FindPending((int)HttpContext.Session.GetInt32("employeeId"));
        return View(leaveRequests);
    }

    public async Task<IActionResult> Decide(int leaveRequestId)
    {
        LeaveRequest leaveRequest = await _leaveRequestRepository.FindById(leaveRequestId);
        return View(leaveRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int leaveRequestId)
    {
        var leaveRequest = await _leaveRequestRepository.FindById(leaveRequestId);
        if (leaveRequest == null)
        {
            return NotFound();
        }

        // Update leave request status to Approved
        leaveRequest.Status = ApprovalStatus.Approved;
        _leaveRequestRepository.Update(leaveRequest);

        // Create a new approval record
        var approval = new Approval
        {
            LeaveRequestId = leaveRequestId,
            ApprovalStatus = ApprovalStatus.Approved,
            ApprovalDate = DateTime.Now,
            Comments = "Leave approved.",
            ApproverId = (int)HttpContext.Session.GetInt32("employeeId")
            // You can also add ApproverId if you have the current user information
        };
        _approvalRepository.Add(approval);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Deny(int leaveRequestId)
    {
        
        var leaveRequest = await _leaveRequestRepository.FindById(leaveRequestId);
        if (leaveRequest == null)
        {
            return NotFound();
        }

        // Update leave request status to Denied
        leaveRequest.Status = ApprovalStatus.Rejected;
        _leaveRequestRepository.Update(leaveRequest);

        // Create a new approval record
        var approval = new Approval
        {
            LeaveRequestId = leaveRequestId,
            ApprovalStatus = ApprovalStatus.Rejected,
            ApprovalDate = DateTime.Now,
            Comments = "Leave denied.",
            ApproverId = 1
            
            // You can also add ApproverId if you have the current user information
        };
        _approvalRepository.Add(approval);

        return RedirectToAction(nameof(Index));
    }
}
