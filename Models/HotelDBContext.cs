using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Models {
    public class HotelDBContext : DbContext {
        public HotelDBContext(DbContextOptions<HotelDBContext> options) : base(options) {}

        public DbSet<Hotel> Hotels { get; set; }
    }
}