using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.DTOs;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiMonController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        private readonly IWebHostEnvironment _env;

        public LoaiMonController(RestaurantDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiMon>>> GetAll()
        {
            return await _context.LoaiMon
                .OrderBy(l => l.TenLoai)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiMon>> GetById(int id)
        {
            var loaiMon = await _context.LoaiMon.FindAsync(id);

            if (loaiMon == null)
            {
                return NotFound();
            }

            return loaiMon;
        }

        [HttpPost]
        public async Task<ActionResult<LoaiMon>> Create([FromForm] LoaiMonDTO dto)
        {
            try
            {
                var fileName = await SaveImage(dto.HinhAnh);
                
                var loaiMon = new LoaiMon
                {
                    TenLoai = dto.TenLoai,
                    HinhAnh = fileName
                };

                _context.LoaiMon.Add(loaiMon);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = loaiMon.MaLoai }, loaiMon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] LoaiMonDTO dto)
        {
            var loaiMon = await _context.LoaiMon.FindAsync(id);

            if (loaiMon == null)
            {
                return NotFound();
            }

            try
            {
                if (dto.HinhAnh != null)
                {
                    // Xóa ảnh cũ
                    if (!string.IsNullOrEmpty(loaiMon.HinhAnh))
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, "images", loaiMon.HinhAnh);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu ảnh mới
                    loaiMon.HinhAnh = await SaveImage(dto.HinhAnh);
                }

                loaiMon.TenLoai = dto.TenLoai;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var loaiMon = await _context.LoaiMon.FindAsync(id);
            if (loaiMon == null)
            {
                return NotFound();
            }

            // Xóa file ảnh
            if (!string.IsNullOrEmpty(loaiMon.HinhAnh))
            {
                var imagePath = Path.Combine(_env.WebRootPath, "images", loaiMon.HinhAnh);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.LoaiMon.Remove(loaiMon);
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