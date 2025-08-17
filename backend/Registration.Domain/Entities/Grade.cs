using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.Domain.Entities
{
    public class Grade
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GradeId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Grade level must be between 3 and 50 characters.")]
        public required string GradeLevel { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters.")]
        public required string GradeDescription { get; set; }
    }
}
