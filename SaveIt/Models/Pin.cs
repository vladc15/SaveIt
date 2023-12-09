using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaveIt.Models
{
    public class Pin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Introduceti un titlu!")]
        [StringLength(50, ErrorMessage = "Titlul nu poate avea mai mult de 50 de caractere!")]
        [MinLength(3, ErrorMessage = "Titlul nu poate avea mai putin de 3 caractere!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Introduceti o descriere!")]
        [StringLength(500, ErrorMessage = "Descrierea nu poate avea mai mult de 500 de caractere!")]
        [MinLength(3, ErrorMessage = "Descrierea nu poate avea mai putin de 3 caractere!")]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Alegeti un board!")]
        public virtual ICollection<PinTag>? PinTags { get; set; }

        public string? UserId { get; set; }

        //public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Tags { get; set; }
        public ICollection<PinBoard>? PinBoards { get; set; }

    }
}
