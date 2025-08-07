using System.ComponentModel.DataAnnotations;

namespace Registration.Domain.Entities
{
    public class Role
    {
        [Key]
        [Required]
        public int Role_Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Role title must be between 3 and 50 characters.")]
        public required string Role_Title { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Project name must be at least 3 characters.")]
        public required string Project_Name { get; set; }
    }
}
