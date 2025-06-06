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

        [HttpPost("giaodich")]
        [HttpPut("giaodich")]
        public async Task<ActionResult> GiaoDichKho(GiaoDichKhoDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var nguyenLieu = await _context.Kho.FindAsync(dto.MaNguyenLieu);
                if (nguyenLieu == null)
                    return NotFound("Không tìm thấy nguyên liệu");

                if (nguyenLieu.TrangThai != "Active")
                    return BadRequest("Nguyên liệu đã ngừng hoạt động");

                // Kiểm tra số lượng
                if (dto.SoLuong <= 0)
                    return BadRequest("Số lượng phải lớn hơn 0");

                // Xử lý xuất kho
                if (dto.LoaiGiaoDich.ToLower() == "xuat")
                {
                    if (nguyenLieu.SoLuongHienTai < dto.SoLuong)
                        return BadRequest("Số lượng trong kho không đủ");
                    nguyenLieu.SoLuongHienTai -= dto.SoLuong;
                }
                // Xử lý nhập kho
                else if (dto.LoaiGiaoDich.ToLower() == "nhap")
                {
                    nguyenLieu.SoLuongHienTai += dto.SoLuong;
                }
                else
                {
                    return BadRequest("Loại giao dịch không hợp lệ");
                }

                var giaoDich = new GiaoDichKho
                {
                    MaNguyenLieu = dto.MaNguyenLieu,
                    SoLuong = dto.SoLuong,
                    Loai = dto.LoaiGiaoDich,
                    NgayGio = DateTime.UtcNow,
                    LyDo = dto.GhiChu ?? string.Empty
                };

                _context.GiaoDichKho.Add(giaoDich);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { 
                    message = "Giao dịch thành công",
                    giaoDich = giaoDich,
                    soLuongHienTai = nguyenLieu.SoLuongHienTai
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Lỗi khi thực hiện giao dịch: {ex.Message}");
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
        public async Task<IActionResult> UpdateKho(int id, KhoDTO khoDTO)
        {
            try
            {
                var kho = await _context.Kho.FindAsync(id);
                if (kho == null)
                {
                    return NotFound();
                }

                kho.TenNguyenLieu = khoDTO.TenNguyenLieu;
                kho.DonVi = khoDTO.DonVi;
                kho.SoLuongHienTai = khoDTO.SoLuongHienTai;
                kho.SoLuongToiThieu = khoDTO.SoLuongToiThieu;
                kho.MaNCC = khoDTO.MaNCC;
                kho.TrangThai = khoDTO.TrangThai;

                await _context.SaveChangesAsync();
                return Ok(kho);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKho(int id)
        {
            try
            {
                var kho = await _context.Kho.FindAsync(id);
                if (kho == null)
                {
                    return NotFound();
                }

                kho.TrangThai = "Inactive";
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("lichsu")]
        public async Task<ActionResult<IEnumerable<GiaoDichKho>>> GetLichSuGiaoDich()
        {
            try
            {
                var lichSu = await _context.GiaoDichKho
                    .Include(g => g.Kho)
                    .OrderByDescending(g => g.NgayGio)
                    .ToListAsync();

                return Ok(lichSu);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy lịch sử giao dịch: {ex.Message}");
            }
        }
    }
}
