using DisasterAlleviationFoundationn.Models;

namespace DisasterAlleviationFoundationn.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalIncidents { get; set; }
        public int TotalVolunteers { get; set; }
        public int TotalDonations { get; set; }
        public int UserIncidents { get; set; }
        public List<DisasterIncident> RecentIncidents { get; set; } = new List<DisasterIncident>();

        //I added New statistics HERE
        public int ActiveUsers { get; set; }
        public int PendingDonations { get; set; }
        public int ResolvedIncidents { get; set; }
    }
}