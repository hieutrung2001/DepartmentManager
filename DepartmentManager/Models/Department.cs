using System.ComponentModel.DataAnnotations;

namespace DepartmentManager.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Modified { get; set; }

        public ICollection<Employee>? employees { get; set; }
    }
}
