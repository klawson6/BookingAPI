using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BookingAPI.Models {
    public class HotelDBContext : DbContext {
        public HotelDBContext(DbContextOptions<HotelDBContext> options) : base(options) {}

        public DbSet<Hotel> Hotels { get; set; }
    }
}