using System.ComponentModel.DataAnnotations;

namespace Registration.Application.DTOs
{
    public class GradeDTO
    {
        [Required]
        public int GradeId { get; set; }  // Added GradeId property

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Grade level must be between 3 and 50 characters.")]
        public required string GradeLevel { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters.")]
        public required string GradeDescription { get; set; }
    }
}
