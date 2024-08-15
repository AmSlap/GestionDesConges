using GestionDesConges.Models;
using Microsoft.EntityFrameworkCore;
using GestionDesConges.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GestionDesConges.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }
        public DbSet<Employee> Employees{ get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Approval> approvals { get; set; }
        public DbSet<EmployeeLogin> EmployeeLogins { get; set; }
        public DbSet<LeaveRequest> leaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust as per your needs

            modelBuilder.Entity<EmployeeLogin>()
                .HasOne(el => el.Employee)
                .WithMany()
                .HasForeignKey(el => el.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust as per your needs

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests) // Assuming Employee has a navigation property ICollection<LeaveRequest>
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust as per your needs

            modelBuilder.Entity<Approval>()
                .HasOne(a => a.Approver)
                .WithMany()
                .HasForeignKey(a => a.ApproverId)
                .OnDelete(DeleteBehavior.NoAction); // Change to NO ACTION if needed

            modelBuilder.Entity<Approval>()
                .HasOne(a => a.LeaveRequest)
                .WithMany()
                .HasForeignKey(a => a.LeaveRequestId)
                .OnDelete(DeleteBehavior.NoAction); // Change to NO ACTION if needed

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<GestionDesConges.ViewModels.EditLeaveRequestViewModel> EditLeaveRequestViewModel { get; set; } = default!;
     }
}
