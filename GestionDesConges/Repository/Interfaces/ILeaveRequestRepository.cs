using GestionDesConges.Models;

namespace GestionDesConges.Repository.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<IEnumerable<LeaveRequest>> GetAll();
        Task<IEnumerable<LeaveRequest>> FindPending(int id);
        Task<IEnumerable<LeaveRequest>> FindByEmployee(int employee);
        Task<LeaveRequest> FindById(int id);
        Task<LeaveRequest> FindByIdNoTracking(int id);
        Task<int> GetNumberOfDays(int employeeId, int? year = null);

        bool Add(LeaveRequest leaveRequest);
        bool Update(LeaveRequest leaveRequest);
        bool Delete(LeaveRequest leaveRequest);
        bool Save();
    }
}
