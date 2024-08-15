using GestionDesConges.Models;

namespace GestionDesConges.Repository.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAll();
        Task<IEnumerable<Department>> FindByName(String name);
        Task<Department> FindById(int id);
        Task<Department> FindByIdNoTracking(int id);
        bool Add(Department department);
        bool Update(Department department);
        bool Delete(Department department);
        bool Save();
    }
}
