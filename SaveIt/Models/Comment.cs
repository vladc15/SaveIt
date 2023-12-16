using System.ComponentModel.DataAnnotations;

namespace SaveIt.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Introduceti un comentariu!")]
        [StringLength(500, ErrorMessage = "Comentariul nu poate avea mai mult de 500 de caractere!")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public int? PinId { get; set; }
    
        public virtual Pin? Pin { get; set; }
        
        public int? BoardId { get; set; }

        public virtual Board? Board { get; set; }
    }
}
