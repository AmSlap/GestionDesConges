using GestionDesConges.Data;
using GestionDesConges.Data.enums;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionDesConges.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Employee employee)
        {
            _context.Employees.Add(employee);
            return Save();
        }

        public bool Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            return Save();
        }

        public async Task<Employee> FindById(int id)
        {
            return await _context.Employees.Include(a => a.Department).FirstOrDefaultAsync(a => a.EmployeeId == id);
        }
        public async Task<Employee> FindByIdNoTracking(int id)
        {
            return await _context.Employees.Include(a => a.Department).AsNoTracking().FirstOrDefaultAsync(a => a.EmployeeId == id);
        }

        public async Task<IEnumerable<Employee>> FindByName(string name)
        {
            return await _context.Employees.Include(a => a.Department).Where(a=> a.FullName.Contains(name)).ToListAsync();
        }

        public  async Task<IEnumerable<Employee>> FindByPosition(Position position)
        {
            return await _context.Employees.Include(a => a.Department).Where(a => a.Position ==position).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _context.Employees.Include(a => a.Department).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Employee employee)
        {
            _context.Employees.Update(employee);
            return Save();
        }

    }
}
