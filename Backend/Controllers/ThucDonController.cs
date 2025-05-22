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

        [HttpGet("LoaiMon")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetLoaiMon()
        {
            var loaiMons = await _context.ThucDon
                .Select(t => t.LoaiMon)
                .Distinct()
                .Select(loai => new
                {
                    loaiMon = loai,
                    hinhAnh = _context.ThucDon
                        .Where(t => t.LoaiMon == loai)
                        .Select(t => t.HinhAnh)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(loaiMons);
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
                    MaLoai = dto.MaLoai,  // Sử dụng MaLoai thay vì LoaiMon
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

        [HttpDelete("{id}")]  
        public async Task<IActionResult> Delete(int id)
        {
            var thucDon = await _context.ThucDon.FindAsync(id);
            if (thucDon == null)
            {
                return NotFound();
            }

            // Xóa file ảnh cũ nếu tồn tại
            if (!string.IsNullOrEmpty(thucDon.HinhAnh))
            {
                var imagePath = Path.Combine(_env.WebRootPath, "images", thucDon.HinhAnh);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.ThucDon.Remove(thucDon);
            await _context.SaveChangesAsync();

            return NoContent();
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
