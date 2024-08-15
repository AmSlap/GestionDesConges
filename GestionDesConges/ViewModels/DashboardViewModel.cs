using GestionDesConges.Models;

namespace GestionDesConges.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalLeaveDays { get; set; }
        public int LeaveDaysTaken { get; set; }
        public int RemainingLeaveDays { get; set; }
        public IEnumerable<LeaveRequest> RecentLeaveRequests { get; set; }
    }
}
