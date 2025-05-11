using Microsoft.AspNetCore.Mvc;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly RestaurantDbContext _context;

        public BaseController(RestaurantDbContext context)
        {
            _context = context;
        }
    }
}
