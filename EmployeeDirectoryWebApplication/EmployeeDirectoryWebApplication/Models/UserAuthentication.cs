using Microsoft.AspNetCore.Identity;

namespace EmployeeDirectoryWebApplication.Models
{
    public class UserAuthentication:IdentityUser
    {
        //public int UserId { get; set; }

        

        //public string Username { get; set; } = null!;

        //public string? Email { get; set; }

        //public string Password { get; set; } = null!;

        public string UserType { get; set; }

        public int? RoleId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public virtual Role? Role { get; set; }
    }
}
