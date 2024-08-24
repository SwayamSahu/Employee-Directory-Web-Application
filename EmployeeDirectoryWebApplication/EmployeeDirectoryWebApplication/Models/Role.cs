namespace EmployeeDirectoryWebApplication.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public virtual ICollection<UserAuthentication> UserAuthentications { get; set; } = new List<UserAuthentication>();
    }
}
