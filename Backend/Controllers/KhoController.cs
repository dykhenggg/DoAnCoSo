using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhoController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public KhoController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet("canbaotoncao")]
        public async Task<ActionResult<IEnumerable<Kho>>> GetSapHet()
        {
            return await _context.Kho
                .Where(k => k.SoLuongHienTai <= k.SoLuongToiThieu)
                .OrderBy(k => k.SoLuongHienTai)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kho>>> GetAll()
        {
            return await _context.Kho
                .Include(k => k.NhaCungCap)
                .OrderBy(k => k.TenNguyenLieu)
                .ToListAsync();
        }

        [HttpPost("nhapkho")]
        public async Task<ActionResult> NhapKho(GiaoDichKhoDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var nguyenLieu = await _context.Kho.FindAsync(dto.MaNguyenLieu);
                if (nguyenLieu == null) return NotFound();

                nguyenLieu.SoLuongHienTai += dto.SoLuong;
                
                var giaoDich = new GiaoDichKho
                {
                    MaNguyenLieu = dto.MaNguyenLieu,
                    SoLuong = dto.SoLuong,
                    Loai = "NhapKho",
                    NgayGio = DateTime.UtcNow,
                    LyDo = dto.GhiChu ?? string.Empty
                };

                _context.GiaoDichKho.Add(giaoDich);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(giaoDich);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Kho>> Create(KhoDTO dto)
        {
            if (string.IsNullOrEmpty(dto.TenNguyenLieu))
                return BadRequest("Tên nguyên liệu không được để trống");

            // Load nhà cung cấp từ database
            var nhaCungCap = await _context.NhaCungCap.FindAsync(dto.MaNCC);
            if (nhaCungCap == null)
                return BadRequest("Không tìm thấy nhà cung cấp");

            var kho = new Kho
            {
                TenNguyenLieu = dto.TenNguyenLieu,
                DonVi = dto.DonVi,
                SoLuongHienTai = dto.SoLuongHienTai,
                SoLuongToiThieu = dto.SoLuongToiThieu,
                MaNCC = dto.MaNCC,
                NgayNhap = DateTime.UtcNow,
                TrangThai = "Active",
                NhaCungCap = nhaCungCap
            };

            _context.Kho.Add(kho);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = kho.MaNguyenLieu }, kho);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Kho kho)
        {
            if (id != kho.MaNguyenLieu)
                return BadRequest();

            if (string.IsNullOrEmpty(kho.TenNguyenLieu))
                return BadRequest("Tên nguyên liệu không được để trống");

            var existingKho = await _context.Kho.FindAsync(id);
            if (existingKho == null)
                return NotFound();

            existingKho.TenNguyenLieu = kho.TenNguyenLieu;
            existingKho.DonVi = kho.DonVi;
            existingKho.SoLuongToiThieu = kho.SoLuongToiThieu;
            existingKho.MaNCC = kho.MaNCC;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Kho.AnyAsync(k => k.MaNguyenLieu == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kho = await _context.Kho.FindAsync(id);
            if (kho == null)
                return NotFound();

            // Kiểm tra xem nguyên liệu có đang được sử dụng trong công thức món ăn không
            var isUsedInRecipe = await _context.NguyenLieu
                .AnyAsync(nl => nl.MaNguyenLieu == id);

            if (isUsedInRecipe)
                return BadRequest("Không thể xóa nguyên liệu đang được sử dụng trong công thức món ăn");

            kho.TrangThai = "Inactive";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
