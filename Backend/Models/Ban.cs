using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Ban
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public int SucChua { get; set; }
        
        [Required]
        public bool TrangThai { get; set; }
    }
}