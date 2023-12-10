using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaveIt.Models
{
    public class Board
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Introduceti un nume!")]
        public string Name { get; set; }

        public string? UserId { get; set; }

        //public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        [NotMapped]
        public ICollection<PinBoard>? PinBoards { get; set; } 
    }
}
