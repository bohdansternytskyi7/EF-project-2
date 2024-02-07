using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripCompany.DAL;
using TripCompany.DTO;

namespace TripCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly TripCompanyContext _context;

        public TripsController(TripCompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _context.Trips
                .Include(x => x.Countries)
                .Include(x => x.ClientTrips)
                .OrderByDescending(x => x.DateFrom)
                .Select(x => new TripDTO
                {
                    Name = x.Name,
                    Description = x.Description,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    MaxPeople = x.MaxPeople,
                    Countries = x.Countries.Select(y => new CountryDTO
                    {
                        Name = y.Name,
                    }).ToList(),
                    Clients = x.ClientTrips.Select(y => new ClientDTO
                    {
                        FirstName = y.Client.FirstName,
                        LastName = y.Client.LastName
                    }).ToList()
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] ClientTripDTO clientTripDTO)
        {
            clientTripDTO.IdTrip = idTrip;
            var client = await _context.Clients.Where(x => x.Pesel == clientTripDTO.Pesel).FirstOrDefaultAsync();

            if (client == null)
            {
                client = new Client()
                {
                    FirstName = clientTripDTO.FirstName,
                    LastName = clientTripDTO.LastName,
                    Email = clientTripDTO.Email,
                    Telephone = clientTripDTO.Telephone,
                    Pesel = clientTripDTO.Pesel
                };

                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
            }

            if (!_context.Trips.Any(x => x.IdTrip == idTrip))
                return BadRequest("Trip doesn't exist");

            if (_context.ClientTrips.Any(x => x.IdClient == client.IdClient && x.IdTrip == clientTripDTO.IdTrip))
                return BadRequest("Client is already added to that trip.");

            var clientTrip = new ClientTrip()
            {
                IdClient = client.IdClient,
                IdTrip = clientTripDTO.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientTripDTO.PaymentDate,
            };

            await _context.ClientTrips.AddAsync(clientTrip);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
