using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/Role")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public VaiTroController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: api/VaiTro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VaiTro>>> GetVaiTro()
        {
            return await _context.Role.ToListAsync();
        }
    }
}