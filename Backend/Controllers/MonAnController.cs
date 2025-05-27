using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.DTOs;
using Backend.Data;
using System.Data; // Thêm dòng này

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonAnController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MonAnController(RestaurantDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAll()
        {
            var monAns = await _context.MonAn
                .Include(m => m.LoaiMon)
                .Include(m => m.KhuyenMai)
                .Select(m => new
                {
                    m.MaMon,
                    m.TenMon,
                    m.Gia,
                    m.GiaSauGiam,
                    m.MaLoai,
                    LoaiMon = new
                    {
                        m.LoaiMon.MaLoai,
                        m.LoaiMon.TenLoai
                    },
                    m.HinhAnh,
                    m.MaKM,
                    PhanTramGiam = m.KhuyenMai != null ? (decimal?)m.KhuyenMai.PhanTramGiam : null
                })
                .OrderBy(m => m.TenMon)
                .ToListAsync();

            return Ok(monAns);
        }

        [HttpGet("LoaiMon")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetLoaiMon()
        {
            var loaiMons = await _context.MonAn
                .Select(t => t.LoaiMon)
                .Distinct()
                .Select(loai => new
                {
                    loaiMon = loai,
                    hinhAnh = _context.MonAn
                        .Where(t => t.LoaiMon == loai)
                        .Select(t => t.HinhAnh)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(loaiMons);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetFoodCount()
        {
            return await _context.MonAn.CountAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MonAn>> GetById(int id)
        {
            var monAn = await _context.MonAn.FindAsync(id);

            if (monAn == null)
            {
                return NotFound();
            }

            return monAn;
        }

        [HttpPost]
        public async Task<ActionResult<MonAn>> Create([FromForm] MonAnDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try 
            {
                var loaiMon = await _context.LoaiMon.FindAsync(dto.MaLoai);
                if (loaiMon == null)
                {
                    return BadRequest("Loại món không tồn tại");
                }

                if (dto.HinhAnh == null)
                {
                    return BadRequest("Vui lòng chọn hình ảnh");
                }

                var fileName = await SaveImage(dto.HinhAnh);
                
                var monAn = new MonAn
                {
                    TenMon = dto.TenMon,
                    Gia = dto.Gia,
                    MaLoai = dto.MaLoai,
                    LoaiMon = loaiMon,
                    HinhAnh = fileName
                };

                _context.MonAn.Add(monAn);
                await _context.SaveChangesAsync();

                // Thêm nguyên liệu cho món ăn
                if (dto.NguyenLieu != null && dto.NguyenLieu.Any())
                {
                    foreach (var nl in dto.NguyenLieu)
                    {
                        var nguyenLieu = new NguyenLieu
                        {
                            MaMon = monAn.MaMon,
                            MaNL = nl.MaNL,
                            SoLuong = nl.SoLuong,
                            DonVi = nl.DonVi
                        };
                        _context.NguyenLieu.Add(nguyenLieu);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                // Lấy món ăn với thông tin đầy đủ để trả về
                var result = await _context.MonAn
                    .Include(m => m.LoaiMon)
                    .Include(m => m.NguyenLieu)
                    .FirstOrDefaultAsync(m => m.MaMon == monAn.MaMon);

                return CreatedAtAction(nameof(GetById), new { id = monAn.MaMon }, result);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]  
        public async Task<IActionResult> Delete(int id)
        {
            var monAn = await _context.MonAn.FindAsync(id);
            if (monAn == null)
            {
                return NotFound();
            }

            // Xóa file ảnh cũ nếu tồn tại
            if (!string.IsNullOrEmpty(monAn.HinhAnh))
            {
                var imagePath = Path.Combine(_env.WebRootPath, "images", monAn.HinhAnh);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.MonAn.Remove(monAn);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MonAnDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(
                IsolationLevel.ReadCommitted,
                new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(30)).Token
            );
            try
            {
                var monAn = await _context.MonAn
                    .Include(m => m.LoaiMon)
                    .FirstOrDefaultAsync(m => m.MaMon == id);

                if (monAn == null)
                {
                    return NotFound("Không tìm thấy món ăn");
                }

                var loaiMon = await _context.LoaiMon.FindAsync(dto.MaLoai);
                if (loaiMon == null)
                {
                    return BadRequest("Loại món không tồn tại");
                }

                // Cập nhật thông tin cơ bản
                monAn.TenMon = dto.TenMon;
                monAn.Gia = dto.Gia;
                monAn.MaLoai = dto.MaLoai;
                monAn.LoaiMon = loaiMon;

                // Cập nhật hình ảnh nếu có
                if (dto.HinhAnh != null && dto.HinhAnh.Length > 0)
                {
                    // Xóa file ảnh cũ
                    if (!string.IsNullOrEmpty(monAn.HinhAnh))
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, "images", monAn.HinhAnh);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu file ảnh mới
                    monAn.HinhAnh = await SaveImage(dto.HinhAnh);
                }

                _context.Entry(monAn).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Trả về món ăn đã được cập nhật với thông tin đầy đủ
                var updatedMonAn = await _context.MonAn
                    .Include(m => m.LoaiMon)
                    .FirstOrDefaultAsync(m => m.MaMon == id);

                return Ok(updatedMonAn);
            }
            catch (OperationCanceledException)
            {
                await transaction.RollbackAsync();
                return StatusCode(408, "Request timeout");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Lỗi khi cập nhật món ăn: {ex.Message}");
            }
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded");

            try
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa tồn tại

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving image: {ex.Message}");
            }
        }
    }
}
