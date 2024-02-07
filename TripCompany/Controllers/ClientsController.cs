using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripCompany.DAL;

namespace TripCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly TripCompanyContext _context;

        public ClientsController(TripCompanyContext context)
        {
            _context = context;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClient([FromQuery] int id)
        {
            var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.IdClient == id);
            if (client == null)
                return NotFound();

            if (_context.ClientTrips.Any(x => x.IdClient == id))
                return BadRequest("Client has assigned trips.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
