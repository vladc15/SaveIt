using System.ComponentModel.DataAnnotations.Schema;

namespace SaveIt.Models
{
    public class PinBoard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? PinId { get; set; }
        public int? BoardId { get; set; }

        public virtual Pin? Pin { get; set; }
        public virtual Board? Board { get; set; }

        public DateTime Date { get; set; }

    }
}
