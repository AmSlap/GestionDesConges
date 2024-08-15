using GestionDesConges.Models;

namespace GestionDesConges.Repository.Interfaces
{
    public interface IEmployeeLoginRepository
    {

        Task<EmployeeLogin> FindByUsernameAndPw(String username, String password);
        Task<EmployeeLogin> FindByID(int? id);
        Task<EmployeeLogin> FindByIDNoTracking(int? id);
        Task<IEnumerable<EmployeeLogin>> GetAll();
        Task<IEnumerable<EmployeeLogin>> FindByEmployee(int employeeId);
        bool Add(EmployeeLogin employee);
        bool Update(EmployeeLogin employee);
        bool Delete(EmployeeLogin employee);
        bool Save();
    }
}
