using GestionDesConges.Data.enums;
using GestionDesConges.Models;

namespace GestionDesConges.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<IEnumerable<Employee>> FindByName(String name);
        Task<IEnumerable<Employee>> FindByPosition(Position position);
        Task<Employee> FindById(int id);
        Task<Employee> FindByIdNoTracking(int id);
        bool Add(Employee employee);
        bool Update(Employee employee);
        bool Delete(Employee employee);
        bool Save();
    }
}
