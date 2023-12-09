using System.ComponentModel.DataAnnotations.Schema;

namespace SaveIt.Models
{
    public class PinTag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PinId { get; set; }
        public int TagId { get; set; }

        public virtual Pin? Pin { get; set; }
        public virtual Tag? Tag { get; set; }

        public DateTime Date { get; set; }
    }
}
