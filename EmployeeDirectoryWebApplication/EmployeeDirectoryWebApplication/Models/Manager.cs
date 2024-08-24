namespace EmployeeDirectoryWebApplication.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public int EmployeeId { get; set; }

        public virtual ICollection<EmployeeProfile> EmployeeProfiles { get; set; } = new List<EmployeeProfile>();

    }
}
