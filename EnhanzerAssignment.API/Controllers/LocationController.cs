using EnhanzerAssignment.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnhanzerAssignment.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationController : ControllerBase
        {
         private readonly ApplicationDbContext _context;

         public LocationController(ApplicationDbContext context)
            {
                _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _context.LocationDetails.Select(x => new
            {
                x.Id,
                x.Location_Code,
                x.Location_Name
            }).ToListAsync();

            return Ok(locations);
        }


    }
    
}
