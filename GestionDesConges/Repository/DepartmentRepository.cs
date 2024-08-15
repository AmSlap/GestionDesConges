using GestionDesConges.Data;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionDesConges.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Department department)
        {
            _context.departments.Add(department);
            return Save();
        }

        public bool Delete(Department department)
        {
            _context.departments.Remove(department);
            return Save();
        }

        public async Task<Department> FindById(int id)
        {
            return await _context.departments.FirstOrDefaultAsync(a => a.DepartmentId == id);
        }
        public async Task<Department> FindByIdNoTracking(int id)
        {
            return await _context.departments.AsNoTracking().FirstOrDefaultAsync(a => a.DepartmentId == id);
        }

        public async Task<IEnumerable<Department>> FindByName(string name)
        {
            return await _context.departments.Where(a=> a.DepartmentName == name).ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await _context.departments.ToListAsync();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Department department)
        {
            _context.departments.Update(department);
            return Save();
        }
    }
}
