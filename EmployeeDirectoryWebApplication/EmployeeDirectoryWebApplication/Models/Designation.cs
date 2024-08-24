namespace EmployeeDirectoryWebApplication.Models
{
    public class Designation
    {
        public int DesignationId { get; set; }

        public string DesignationName { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public virtual ICollection<EmployeeProfile> EmployeeProfiles { get; set; } = new List<EmployeeProfile>();
    }
}
