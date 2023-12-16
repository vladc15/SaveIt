using System.ComponentModel.DataAnnotations.Schema;

namespace SaveIt.Models
{
    public class Like
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? PinId { get; set; }
        //public int UserId { get; set; }

        public virtual Pin? Pin { get; set; }
        //public virtual Tag? Tag { get; set; }
    }
}
