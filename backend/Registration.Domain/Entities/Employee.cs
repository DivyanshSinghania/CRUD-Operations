using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.Domain.Entities
{
    public class Employee
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Employee_Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public required string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public required string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Department name must be between 2 and 50 characters.")]
        public required string Department { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Designation must be between 2 and 50 characters.")]
        public required string Designation { get; set; }


        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}
