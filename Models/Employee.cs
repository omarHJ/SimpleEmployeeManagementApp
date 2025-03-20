using System.ComponentModel.DataAnnotations;

namespace SimpleEmployeeManagementApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string Position { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
