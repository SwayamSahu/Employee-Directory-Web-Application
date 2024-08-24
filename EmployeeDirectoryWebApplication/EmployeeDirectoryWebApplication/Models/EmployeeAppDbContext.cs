using EmployeeDirectoryWebApplication.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EmployeeDirectoryWebApplication.Models
{
    public class EmployeeAppDbContext : IdentityDbContext<UserAuthentication>
    {
        public EmployeeAppDbContext()
        {
        }

        public EmployeeAppDbContext(DbContextOptions<EmployeeAppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContactInformation> ContactInformations { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Designation> Designations { get; set; }

        public virtual DbSet<EmployeeProfile> EmployeeProfiles { get; set; }

        public virtual DbSet<Manager> Managers { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<UserAuthentication> UserAuthentications { get; set; }
    }
}







    
