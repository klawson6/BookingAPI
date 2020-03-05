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
        private readonly BookingDBContext BookingDB; // The database of bookings
        private readonly HotelDBContext HotelDB; // The database of hotels

        public BookingAPIController(HotelDBContext hdb, BookingDBContext bdb) {
            BookingDB = bdb;
            HotelDB = hdb;
        }

        // GET: api/BookingAPI/Hotels
        // Respond 200 with all the hotels in the body.
        [HttpGet("Hotels")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels() {
            return await HotelDB.Hotels.ToListAsync(); // Respond OK 200 
        }

        // GET: api/BookingAPI/Bookings
        // Respond 200 with all the hotels in the body.
        [HttpGet("Bookings")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings() {
            return await BookingDB.Bookings.ToListAsync(); // Respond OK 200 
        }

        // GET: api/BookingAPI/Hotel/id/{id}
        // Respond 200 with the Hotel with the specified id in the body, if it does not exist, respond 404 resource not found.
        [HttpGet("Hotel/id/{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(long id) {
            var hotel = await HotelDB.Hotels.FindAsync(id);

            if (hotel == null) {
                return NotFound(); // Respond OK 404 
            }

            return hotel; // Respond OK 200 
        }

        // GET: api/BookingAPI/Hotel/search/{name}
        // Respond 200 with all hotels with names matching the specified name, in the body
        [HttpGet("Hotel/search/{name}")]
        public async Task<ActionResult<IEnumerable<Hotel>>> SearchHotel(string name) {
            return await HotelDB.Hotels.Where(h => h.name == name).ToListAsync(); // Respond OK 200 
        }

        // POST: api/BookingAPI/Hotel/add
        // Respond 201 with the newly created hotel in the body
        [HttpPost("Hotel/add")]
        public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel) { // The hotel fields are given as JSON in the body fo the post, and implicitly converted to a Hotel object.
            HotelDB.Hotels.Add(hotel); // Add Hotel object to the database context that accesses Hotels tables of database
            await HotelDB.SaveChangesAsync(); // Update database

            // Return a CreatedAtActionResult (201) response, that indicates a new record has been made serverside, to the client. 
            // The response will also build the url reference to this new object and put this in te location field of the header: 
            // GetHotels function -> Get request to root of this API /api/BookingAPI/Hotel/id 
            // + The id of the new record to access it = /api/BookingAPI/Hotel/id/{id}
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        // POST: api/BookingAPI/Hotel/delete/{id}
        // Respond 200 with the hotel with id matching the specified id, that has been deleted, in the response body. Respond 404 if no hotel existed with the specified id.
        // Deletes all bookings associated with the hotel if the hotel existed and was deleted.
        [HttpDelete("Hotel/delete/{id}")]
        public async Task<ActionResult<Hotel>> DeleteHotel(long id) {
            var hotel = await HotelDB.Hotels.FindAsync(id);

            if (hotel == null) {
                return NotFound(); // Respond 404
            }

            HotelDB.Hotels.Remove(hotel);
            await HotelDB.SaveChangesAsync(); // Update database

            foreach (Booking booking in BookingDB.Bookings.Where(b => b.hotel == id).ToList()) { // For every booking at this hotel
                BookingDB.Bookings.Remove(booking); // Delete the booking
                await BookingDB.SaveChangesAsync(); // Update database
            }

            return hotel; // Respond OK 200
        }

        // POST: api/BookingAPI/Hotel/deleteall
        // Respond 200 with every hotel in the body, once all deleted
        [HttpDelete("Hotel/deleteall")]
        public async Task<ActionResult<IEnumerable<Hotel>>> DeleteHotels() { 
            List<Hotel> hotels = HotelDB.Hotels.ToList();
            foreach (Hotel hotel in hotels) { // For every hotel
                HotelDB.Hotels.Remove(hotel); // Delete hotel
                await HotelDB.SaveChangesAsync(); // Update database
            }
            foreach (Booking booking in BookingDB.Bookings.ToList()) { // For every booking
                BookingDB.Bookings.Remove(booking); // Delete booking
                await BookingDB.SaveChangesAsync(); // Update database
            }
            return hotels; // Respond OK 200
        }

        // POST: api/BookingAPI/Hotel/Book
        // Respond 201 with the newly created booking in the body and url to it in location field of header. 
        // Respond 200 with an empty body if the hotel the booking was for, does not exist, or the booking was invalid (not enough room in hotel, too many poeple per room)
        [HttpPost("Hotel/Book")]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking) { 
            var hotel = await HotelDB.Hotels.FindAsync(booking.hotel); // Get hotel for booking
            if (hotel == null) {
                return Ok(); // Respond 200
            }
            var bookings = await BookingDB.Bookings.Where(b => b.hotel == booking.hotel).ToListAsync(); // Get all bookings at the hotel that the booking is for
            if (BookingSupport.createBooking(booking, hotel, bookings) == null) { // Attempt to create booking. Checks validity of booking behind the scenes.
                return Ok(); // Respond 200
            }
            else {
                BookingDB.Bookings.Add(booking); // Add booking to database
                await BookingDB.SaveChangesAsync(); // Update database
            }


            // Return a CreatedAtActionResult (201) response, that indicates a new record has been made serverside, to the client. 
            // The response will also build the url refernce to this new object: 
            // GetBooking function -> Get request to root of this API /api/BookingAPI/Bookings/
            // + The id of the new record to access it = /api/BookingAPI/Bookings/{id}
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        // GET: api/BookingAPI/Hotel/{id}/check
        // Respond 200 if the booking is available, with the booking in the body.
        // Respond 200 with empty body if booking is not available
        [HttpGet("Hotel/{id}/check")]
        public async Task<ActionResult<Booking>> CheckHotelBooking(Booking booking) {
            var hotel = await HotelDB.Hotels.FindAsync(booking.hotel); // The hotel where the booking is
            var bookings = await BookingDB.Bookings.Where(b => b.hotel == booking.hotel).ToListAsync(); // All bookings at hotel where booking will be

            if (BookingSupport.checkAvailabilty(booking, hotel, bookings)) return booking; // Check if the booking is available and respond 200 with booking in body
            else return Ok(); // respond 200
        }

        // GET: api/BookingAPI/Hotel/{id}/Bookings
        // Respond 200 with all bookings at the hotel with the matching specified id, in the body
        [HttpGet("Hotel/{id}/Bookings")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetHotelBookings(long id) {
            return await BookingDB.Bookings.Where(b => b.hotel == id).ToListAsync(); //  Respond 200 with bookings in body
        }

        // GET: api/BookingAPI/Bookings/{id}
        // Respond 200 with booking with id specified, in the body
        // Respond 404 if no booking exists with specified id
        [HttpGet("Bookings/{id}")]
        public async Task<ActionResult<Booking>> GetBooking(long id) {
            var booking = await BookingDB.Bookings.FindAsync(id);

            if (booking == null) {
                return NotFound(); // Respond 404
            }

            return booking; //  Respond 200
        }

        // POST: api/BookingAPI/Bookings/delete/{id}
        // Respond 200 with the booking with the specified id, in the body, if it existed and was deleted.
        // Respond 404 if no booking with the specified id existed. 
        [HttpDelete("Bookings/delete/{id}")]
        public async Task<ActionResult<Booking>> DeleteBooking(long id) {
            var booking = await BookingDB.Bookings.FindAsync(id);

            if (booking == null) {
                return NotFound(); // Respond 404
            }

            BookingDB.Bookings.Remove(booking);
            await BookingDB.SaveChangesAsync();

            return booking; // Respond 200 with booking in body
        }

        // POST: api/BookingAPI/Hotel/{id}/Bookings/deleteall
        // Respond 200, with all bookings at the hotel with the specified id, in the body
        [HttpDelete("Hotel/{id}/Bookings/deleteall")]
        public async Task<ActionResult<IEnumerable<Booking>>> DeleteHotelBookings(long id) {
            var bookings = await BookingDB.Bookings.Where(b => b.hotel == id).ToListAsync(); // All bookings at hotel with specified id
            foreach (Booking booking in bookings) { // For each booking
                BookingDB.Bookings.Remove(booking); // Delete booking
                await BookingDB.SaveChangesAsync(); // Update database
            }
            return bookings; // Response 200 with deleted bookings in body.
        }
    }
}
