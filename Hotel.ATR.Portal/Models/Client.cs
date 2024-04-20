using System.ComponentModel.DataAnnotations;

namespace Hotel.ATR.Portal.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PathToImage { get; set; }
        [Required]
        public string PathToHoveredImage { get; set; }

    }
}
