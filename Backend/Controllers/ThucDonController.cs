using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.DTOs;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThucDonController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ThucDonController(RestaurantDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThucDon>>> GetAll()
        {
            return await _context.ThucDon
                .OrderBy(t => t.TenMon)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ThucDon>> Create([FromForm] ThucDonDTO dto)
        {
            try 
            {
                var fileName = await SaveImage(dto.HinhAnh);
                
                var thucDon = new ThucDon
                {
                    TenMon = dto.TenMon,
                    Gia = dto.Gia,
                    LoaiMon = dto.LoaiMon,
                    HinhAnh = fileName
                };

                _context.ThucDon.Add(thucDon);
                await _context.SaveChangesAsync();

                return Ok(thucDon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }
    }
}
