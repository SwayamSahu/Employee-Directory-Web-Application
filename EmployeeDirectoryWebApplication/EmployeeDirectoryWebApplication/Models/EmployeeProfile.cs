using System.ComponentModel.DataAnnotations;

namespace EmployeeDirectoryWebApplication.Models
{
    public class EmployeeProfile
    {
        [Key]
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int? DesignationId { get; set; }

        public int? DeptId { get; set; }

        public int? ManagerId { get; set; }

        public string? EmployeeProfilePic { get; set; }

        public string? EmploymentStatus { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public virtual ICollection<ContactInformation> ContactInformations { get; set; } = new List<ContactInformation>();

        public virtual Department? Dept { get; set; }

        public virtual Designation? Designation { get; set; }

        public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();
    }
}
