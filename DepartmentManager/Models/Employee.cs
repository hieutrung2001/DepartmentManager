using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DepartmentManager.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string? Fullname { get; set; }
        public int? Age { get; set; }
        public decimal? Salary { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Modified { get; set; }

        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public Department? department { get; set; }
    }
}
