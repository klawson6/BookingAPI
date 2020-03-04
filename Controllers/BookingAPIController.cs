using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;

namespace BookingAPI.Controllers {
    [Route("api/BookingAPI")]
    [ApiController]
    public class BookingAPIController : ControllerBase {
        private readonly BookingDBContext BookingDB;
        private readonly HotelDBContext HotelDB;

        public BookingAPIController(HotelDBContext hdb, BookingDBContext bdb) {
            BookingDB = bdb;
            HotelDB = hdb;
        }

        // GET: api/BookingAPI
        [HttpGet("Hotels")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels() {
            return await HotelDB.Hotels.ToListAsync();
        }

        // GET: api/BookingAPI/5
        [HttpGet("HotelID/{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(long id) {
            var hotel = await HotelDB.Hotels.FindAsync(id);

            if (hotel == null) {
                return NotFound();
            }

            return hotel;
        }

        // GET: api/BookingAPI/5
        [HttpGet("Hotel/{name}")]
        public async Task<ActionResult<IEnumerable<Hotel>>> SearchHotel(string name) {
            return await HotelDB.Hotels.Where(h => h.name == name).ToListAsync(); ;
        }

        // POST: api/BookingAPI
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("AddHotel")]
        public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel) { // The hotel fields are given as JSON in the body fo the post, and implicitly converted to a Hotel object.
            HotelDB.Hotels.Add(hotel); // Add Hotel object to the database context that accesses Hotels tables of database
            await HotelDB.SaveChangesAsync(); // Wait for the database to update with new record

            // Return a CreatedAtActionResult (201) response, that indicates a new record has been made serverside, to the client. 
            // The response will also build the url refernce to this new object: 
            // GetHotels function -> Get request to root of this API /api/BookingAPI 
            // + The id of the new record to access it = /api/BookingAPI/{id}
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        // POST: api/BookingAPI
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Booking")]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking) { // The hotel fields are given as JSON in the body fo the post, and implicitly converted to a Hotel object.
            BookingDB.Bookings.Add(booking); // Add Hotel object to the database context that accesses Hotels tables of database
            await BookingDB.SaveChangesAsync(); // Wait for the database to update with new record

            // Return a CreatedAtActionResult (201) response, that indicates a new record has been made serverside, to the client. 
            // The response will also build the url refernce to this new object: 
            // GetHotels function -> Get request to root of this API /api/BookingAPI 
            // + The id of the new record to access it = /api/BookingAPI/{id}
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        // GET: api/BookingAPI/5
        [HttpGet("{id}/Bookings")]
        public ActionResult<IEnumerable<Booking>> GetHotelBookings(long id) {
            //var bookings = BookingDB.Bookings.Where(b => b.hotel == id).ToList();

            //if (bookings == null) {
            //    return NotFound();
            //}

            return BookingDB.Bookings.Where(b => b.hotel == id).ToList();
        }

        // GET: api/BookingAPI/5
        [HttpGet("Bookings/{id}")]
        public async Task<ActionResult<Booking>> GetBooking(long id) {
            var booking = await BookingDB.Bookings.FindAsync(id);

            if (booking == null) {
                return NotFound();
            }

            return booking;
        }
    }
}
