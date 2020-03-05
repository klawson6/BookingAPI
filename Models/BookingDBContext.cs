using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BookingAPI.Models {
    public class BookingDBContext : DbContext {
        public BookingDBContext(DbContextOptions<BookingDBContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }    
    }
}