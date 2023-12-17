using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;

namespace SaveIt.Models
{
    public class Pin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Introduceti un titlu!")]
        [StringLength(50, ErrorMessage = "Titlul nu poate avea mai mult de 50 de caractere!")]
        [MinLength(3, ErrorMessage = "Titlul nu poate avea mai putin de 3 caractere!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Introduceti o descriere!")]
        [StringLength(500, ErrorMessage = "Descrierea nu poate avea mai mult de 500 de caractere!")]
        [MinLength(3, ErrorMessage = "Descrierea nu poate avea mai putin de 3 caractere!")]
        public string? Content { get; set; }
        
        public string? mediaPath { get; set; }

        public DateTime? Date { get; set; }

        public virtual ICollection<PinTag>? PinTags { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Selectati cel putin un tag!")]
        [MinLength(1, ErrorMessage = "Selectati cel putin un tag!")]
        [NotNull]
        public List<int>? TagIds { get; set; }

        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Tags { get; set; }
        public ICollection<PinBoard>? PinBoards { get; set; }

    }
}
