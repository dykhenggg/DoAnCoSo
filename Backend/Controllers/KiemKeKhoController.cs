using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EmployeeAndAdmin")]
    public class KiemKeKhoController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public KiemKeKhoController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: api/KiemKeKho/kiemke
        [HttpGet("kiemke")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Kho>>> GetKiemKe()
        {
            return await _context.Kho
                .Where(k => k.TrangThai == "Active")
                .ToListAsync();
        }

        // POST: api/KiemKeKho/luu
        [HttpPost("luu")]
        [AllowAnonymous]
        public async Task<ActionResult<KiemKeKhoResponseDTO>> LuuKiemKe([FromBody] KiemKeKhoDTO kiemKeKhoDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate dữ liệu đầu vào
                if (kiemKeKhoDTO == null)
                {
                    return BadRequest("Dữ liệu kiểm kê không hợp lệ");
                }

                if (string.IsNullOrEmpty(kiemKeKhoDTO.NguoiKiemKe))
                {
                    return BadRequest("Vui lòng nhập người kiểm kê");
                }

                if (kiemKeKhoDTO.ChiTietKiemKe == null || !kiemKeKhoDTO.ChiTietKiemKe.Any())
                {
                    return BadRequest("Không có chi tiết kiểm kê");
                }

                // Đảm bảo ngày kiểm kê là UTC
                DateTime ngayKiemKe;
                try
                {
                    // Parse ISO string to UTC DateTime
                    ngayKiemKe = DateTime.Parse(kiemKeKhoDTO.NgayKiemKe.ToString()).ToUniversalTime();
                }
                catch
                {
                    // Nếu không parse được, sử dụng thời gian hiện tại
                    ngayKiemKe = DateTime.UtcNow;
                }

                // Tạo bản ghi kiểm kê với ngày giờ UTC
                var kiemKeKho = new KiemKeKho
                {
                    NgayKiemKe = ngayKiemKe,
                    NguoiKiemKe = kiemKeKhoDTO.NguoiKiemKe,
                    GhiChu = kiemKeKhoDTO.GhiChu ?? string.Empty
                };

                _context.KiemKeKho.Add(kiemKeKho);
                await _context.SaveChangesAsync();

                // Lưu chi tiết kiểm kê và cập nhật số lượng
                foreach (var chiTiet in kiemKeKhoDTO.ChiTietKiemKe)
                {
                    var nguyenLieu = await _context.Kho.FindAsync(chiTiet.MaNguyenLieu);
                    if (nguyenLieu == null)
                    {
                        await transaction.RollbackAsync();
                        return NotFound($"Không tìm thấy nguyên liệu với mã {chiTiet.MaNguyenLieu}");
                    }

                    if (chiTiet.SoLuongThucTe < 0)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest($"Số lượng thực tế của nguyên liệu {nguyenLieu.TenNguyenLieu} không được âm");
                    }

                    var chiTietKiemKe = new ChiTietKiemKe
                    {
                        MaKiemKe = kiemKeKho.MaKiemKe,
                        MaNguyenLieu = chiTiet.MaNguyenLieu,
                        SoLuongThucTe = chiTiet.SoLuongThucTe,
                        ChenhLech = chiTiet.SoLuongThucTe - nguyenLieu.SoLuongHienTai,
                        GhiChu = chiTiet.GhiChu ?? string.Empty
                    };

                    _context.ChiTietKiemKe.Add(chiTietKiemKe);

                    // Cập nhật số lượng trong kho
                    nguyenLieu.SoLuongHienTai = chiTiet.SoLuongThucTe;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Trả về kết quả với ngày giờ đã chuyển về local time
                var response = new KiemKeKhoResponseDTO
                {
                    MaKiemKe = kiemKeKho.MaKiemKe,
                    NgayKiemKe = kiemKeKho.NgayKiemKe.ToLocalTime(),
                    NguoiKiemKe = kiemKeKho.NguoiKiemKe,
                    GhiChu = kiemKeKho.GhiChu,
                    ChiTietKiemKe = kiemKeKhoDTO.ChiTietKiemKe.Select(ct => new ChiTietKiemKeResponseDTO
                    {
                        MaNguyenLieu = ct.MaNguyenLieu,
                        TenNguyenLieu = _context.Kho.Find(ct.MaNguyenLieu)?.TenNguyenLieu ?? "",
                        SoLuongThucTe = ct.SoLuongThucTe,
                        ChenhLech = ct.SoLuongThucTe - _context.Kho.Find(ct.MaNguyenLieu)?.SoLuongHienTai ?? 0,
                        GhiChu = ct.GhiChu ?? ""
                    }).ToList()
                };

                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                var innerException = ex.InnerException;
                var errorMessage = "Lỗi khi lưu kiểm kê: ";
                
                while (innerException != null)
                {
                    errorMessage += innerException.Message + " ";
                    innerException = innerException.InnerException;
                }

                return StatusCode(500, errorMessage);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Lỗi khi lưu kiểm kê: {ex.Message}");
            }
        }

        // GET: api/KiemKeKho/lichsu
        [HttpGet("lichsu")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<KiemKeKhoResponseDTO>>> GetLichSuKiemKe()
        {
            var lichSu = await _context.KiemKeKho
                .Include(k => k.ChiTietKiemKe)
                .OrderByDescending(k => k.NgayKiemKe)
                .ToListAsync();

            var response = lichSu.Select(k => new KiemKeKhoResponseDTO
            {
                MaKiemKe = k.MaKiemKe,
                NgayKiemKe = k.NgayKiemKe.ToLocalTime(),
                NguoiKiemKe = k.NguoiKiemKe,
                GhiChu = k.GhiChu,
                ChiTietKiemKe = k.ChiTietKiemKe.Select(ct => new ChiTietKiemKeResponseDTO
                {
                    MaNguyenLieu = ct.MaNguyenLieu,
                    TenNguyenLieu = _context.Kho.Find(ct.MaNguyenLieu)?.TenNguyenLieu ?? "",
                    SoLuongThucTe = ct.SoLuongThucTe,
                    ChenhLech = ct.ChenhLech,
                    GhiChu = ct.GhiChu
                }).ToList()
            });

            return Ok(response);
        }
    }
} 