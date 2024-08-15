using GestionDesConges.Data;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionDesConges.Repository
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ApplicationDbContext _context;
        public ApprovalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Approval approval)
        {
            _context.approvals.Add(approval);
            return Save();
        }

        public bool Delete(Approval approval)
        {
            _context.approvals.Remove(approval);
            return Save();
        }

        public async Task<Approval> FindById(int id)
        {
            return await _context.approvals.Include(a => a.LeaveRequest).Include(a => a.Approver).FirstOrDefaultAsync(a => a.ApprovalId == id);
        }

        public async Task<IEnumerable<Approval>> GetAll()
        {
            return await _context.approvals.Include(a => a.LeaveRequest).Include(a => a.Approver).ToListAsync();

        }

        public async Task<IEnumerable<Approval>> GetApprovalsByLeaveRequest(int leaveRequest)
        {
            return await _context.approvals.Include(a => a.LeaveRequest).Include(a => a.Approver).Where(a=>a.LeaveRequestId == leaveRequest).ToListAsync();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Approval approval)
        {
            _context.approvals.Update(approval);
            return Save();
        }
    }
}
