using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Models {
    public class BookingDBContext : DbContext {
        public BookingDBContext(DbContextOptions<BookingDBContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }    
    }
}