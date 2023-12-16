using System.ComponentModel.DataAnnotations.Schema;

namespace SaveIt.Models
{
    public class Like
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? PinId { get; set; }
        public string? UserId { get; set; }

        public virtual Pin? Pin { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
