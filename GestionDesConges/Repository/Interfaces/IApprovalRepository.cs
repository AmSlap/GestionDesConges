using GestionDesConges.Models;

namespace GestionDesConges.Repository.Interfaces
{
    public interface IApprovalRepository
    {
        Task<IEnumerable<Approval>> GetAll();
        Task<IEnumerable<Approval>> GetApprovalsByLeaveRequest(int leaveRequest);
        Task<Approval> FindById(int id);
        bool Add(Approval approval);
        bool Update(Approval approval);
        bool Delete(Approval approval);
        bool Save();
    }
}
