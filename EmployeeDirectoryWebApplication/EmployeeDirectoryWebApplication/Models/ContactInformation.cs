using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeDirectoryWebApplication.Models
{
    public class ContactInformation
    {
        [Key]
        public int ContactId { get; set; }

        public int? EmployeeId { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? OfficeLocation { get; set; }

        public string? SocialMediaProfiles { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        [ValidateNever]

        public virtual EmployeeProfile Employee { get; set; } = null!;

        
        //public virtual ICollection<EmployeeProfile> EmployeeProfiles { get; set; } = new List<EmployeeProfile>();
    }
}
