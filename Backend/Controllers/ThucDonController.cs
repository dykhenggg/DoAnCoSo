using Backend.Models;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThucDonController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ThucDonController(RestaurantDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // LẤY DANH SÁCH MÓN ĂN
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.ThucDon.ToListAsync();
            return Ok(items);
        }

        // LẤY MÓN ĂN THEO ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.ThucDon.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        // THÊM MÓN ĂN
        [HttpPost]
        public async Task<ActionResult<ThucDon>> PostThucDon([FromForm] ThucDonDTO thucDonDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (thucDonDTO.HinhAnh == null || thucDonDTO.HinhAnh.Length == 0)
                    return BadRequest("Hình ảnh không được để trống");

                // Save image first
                string fileName;
                try
                {
                    fileName = await SaveImage(thucDonDTO.HinhAnh);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = $"Lỗi lưu hình ảnh: {ex.Message}" });
                }

                var thucDon = new ThucDon
                {
                    TenMon = thucDonDTO.TenMon.Trim(),
                    Gia = decimal.Parse(thucDonDTO.Gia.ToString()),
                    LoaiMon = thucDonDTO.LoaiMon.Trim(),
                    HinhAnh = fileName,
                    TrangThai = TrangThaiMonAn.Available,
                    NgayTao = DateTime.UtcNow,
                    NguoiTao = "System" // Hoặc user.Identity.Name nếu có authentication
                };

                _context.ThucDon.Add(thucDon);
                await _context.SaveChangesAsync();

                return Ok(thucDon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.InnerException?.Message ?? ex.Message}" });
            }
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        // CẬP NHẬT MÓN ĂN
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ThucDonDto dto)
        {
            var item = await _context.ThucDon.FindAsync(id);
            if (item == null)
                return NotFound();

            item.TenMon = dto.Name;
            item.Gia = dto.Price;
            item.LoaiMon = dto.Category;
            item.NgayCapNhat = DateTime.UtcNow;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                var imagePath = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                item.HinhAnh = $"/uploads/{fileName}";
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Cập nhật thành công." });
        }

        // XÓA MÓN ĂN
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ThucDon.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.ThucDon.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Xóa món thành công." });
        }
    }

    // DTO dùng cho nhận dữ liệu từ frontend
    public class ThucDonDto
    {
        [FromForm(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [FromForm(Name = "description")]
        public string Description { get; set; } = string.Empty;

        [FromForm(Name = "price")]
        public decimal Price { get; set; }

        [FromForm(Name = "category")]
        public string Category { get; set; } = string.Empty;

        [FromForm(Name = "image")]
        public IFormFile? Image { get; set; }
    }

    public class ThucDonDTO
    {
        [FromForm(Name = "tenMon")]
        public string TenMon { get; set; } = string.Empty;

        [FromForm(Name = "gia")]
        public decimal Gia { get; set; }

        [FromForm(Name = "loaiMon")]
        public string LoaiMon { get; set; } = string.Empty;

        [FromForm(Name = "hinhAnh")]
        public IFormFile HinhAnh { get; set; } = null!;
    }
}
