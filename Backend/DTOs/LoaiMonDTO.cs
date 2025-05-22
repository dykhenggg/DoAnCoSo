using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LoaiMonDTO
    {
        [Required]
        public required string TenLoai { get; set; }
        
        public IFormFile? HinhAnh { get; set; }
    }
}