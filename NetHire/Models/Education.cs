using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetHire.Models
{
    public class Education
    {
        [Key]
        public Guid EducationId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [StringLength(255)]
        public string? InstituteName { get; set; }

        [StringLength(255)]
        public string? CourseName { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        [StringLength(10)]
        public string? Grade { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public virtual ICollection<Application>? Applications { get; set; }
    }
}