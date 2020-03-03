using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;

namespace BookingAPI.Controllers {
    [Route("api/BookingAPI")]
    [ApiController]
    public class BookingAPIController : ControllerBase {
        private readonly BookingAPIContext _context;

        public BookingAPIController(BookingAPIContext context) {
            _context = context;
        }

        // GET: api/BookingAPI
        [HttpGet]
        public HashSet<Hotel> GetHotels() {
            return _context.getHotels();
        }

        // POST: api/BookingAPI
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel) { // The hotel fields are given as JSON in the body fo the post, and implicitly converted to a Hotel object.
            _context.Hotels.Add(hotel); // Add Hotel object to the database context that accesses Hotels tables of database
            await _context.SaveChangesAsync(); // Wait for the database to update with new record

            // Return a CreatedAtActionResult (201) response, that indicates a new record has been made serverside, to the client. 
            // The response will also build the url refernce to this new object: 
            // GetHotels function -> Get request to root of this API /api/BookingAPI 
            // + The id of the new record to access it = /api/BookingAPI/{id}
            return CreatedAtAction(nameof(GetHotels), new { id = hotel.Id }, hotel);
        }

        // GET: api/BookingAPI/Bookings
        [HttpGet("Bookings")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings(long id) {
            return await _context.Hotels.;
        }
    }
}
