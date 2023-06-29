using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DepartmentManager.Models;

namespace DepartmentManager.Data
{
    public class DepartmentManagerContext : DbContext
    {
        public DepartmentManagerContext (DbContextOptions<DepartmentManagerContext> options)
            : base(options)
        {
        }

        public DbSet<DepartmentManager.Models.Department> Department { get; set; } = default!;

        public DbSet<DepartmentManager.Models.Employee>? Employee { get; set; }
    }
}
