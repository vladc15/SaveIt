using System.ComponentModel.DataAnnotations;

namespace SaveIt.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Introduceti un nume!")]
        public string TagName { get; set; }

        public virtual ICollection<PinTag>? PinTags { get; set; }
    }
}
