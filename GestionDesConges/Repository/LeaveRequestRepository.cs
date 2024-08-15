using GestionDesConges.Data;
using GestionDesConges.Data.enums;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionDesConges.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(LeaveRequest leaveRequest)
        {
            _context.leaveRequests.Add(leaveRequest);
            return Save();
        }

        public bool Delete(LeaveRequest leaveRequest)
        {
            _context.leaveRequests.Remove(leaveRequest);
            return Save();
        }

        public async Task<IEnumerable<LeaveRequest>> FindByEmployee(int employee)
        {
            return await _context.leaveRequests.Include(a => a.Employee).Where(a=>a.EmployeeId == employee).ToListAsync();

        }

        public async Task<LeaveRequest> FindById(int id)
        {
            return await _context.leaveRequests.Include(a => a.Employee).FirstOrDefaultAsync(a => a.LeaveRequestId == id);
        }
        public async Task<LeaveRequest> FindByIdNoTracking(int id)
        {
            return await _context.leaveRequests.Include(a => a.Employee).AsNoTracking().FirstOrDefaultAsync(a => a.LeaveRequestId == id);
        }

        public async Task<IEnumerable<LeaveRequest>> FindPending(int id)
        {
            return await _context.leaveRequests.Include(a=>a.Employee).Where(a=> a.Status == ApprovalStatus.Pending && a.EmployeeId!=id).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetAll()
        {
            return await _context.leaveRequests.Include(a => a.Employee).ToListAsync();

        }
        public async Task<int> GetNumberOfDays(int employeeId, int? year = null)
        {
            year ??= DateTime.Now.Year; // Use current year if not specified

            // Retrieve leave requests for the given employee and year
            var leaveRequests = await _context.leaveRequests
                .Where(a => a.EmployeeId == employeeId &&
                            ((a.StartDate.Year == year && a.EndDate.Year == year) ||
                             (a.StartDate.Year <= year && a.EndDate.Year >= year))
                             && a.Status != ApprovalStatus.Rejected)
                .ToListAsync();

            // Calculate total number of days
            int totalDays = leaveRequests.Sum(a => (a.EndDate - a.StartDate).Days + 1);

            return totalDays;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(LeaveRequest leaveRequest)
        {
            _context.leaveRequests.Update(leaveRequest);
            return Save();
        }
    }
}
