using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetHire.Models
{
    public class Document
    {
        [Key]
        public Guid FileId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; } = null!;

        // Navigation property
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}