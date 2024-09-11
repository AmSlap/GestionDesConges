using GestionDesConges.Data;
using GestionDesConges.Models;
using GestionDesConges.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionDesConges.Repository
{
    public class EmployeeLoginRepository : IEmployeeLoginRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeLoginRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(EmployeeLogin employee)
        {
            _context.EmployeeLogins.Add(employee);
            return Save();
        }

        public bool Delete(EmployeeLogin employee)
        {
            _context.Remove(employee);
            return Save();
        }

        public Task<IEnumerable<EmployeeLogin>> FindByEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeLogin> FindByUsernameAndPw(string username, string password)
        {
            return await _context.EmployeeLogins.Where(a => a.Username == username && a.PasswordHash == password).Include(a=>a.Employee).FirstOrDefaultAsync();
        }

        public async Task<EmployeeLogin> FindByID(int? id)
        {
            return await _context.EmployeeLogins.Include(e => e.Employee).FirstOrDefaultAsync(m => m.EmployeeLoginId == id);
        }
        public async Task<EmployeeLogin> FindByIDNoTracking(int? id)
        {
            return await _context.EmployeeLogins.Include(e => e.Employee).AsNoTracking().FirstOrDefaultAsync(m => m.EmployeeLoginId == id);
        }
        public async Task<IEnumerable<EmployeeLogin>> GetAll()
        {
            return await _context.EmployeeLogins.Include(e=>e.Employee).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(EmployeeLogin employee)
        {
            _context.EmployeeLogins.Update(employee);
            return Save();
        }
    }
}
