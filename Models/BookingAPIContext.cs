using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Models {
    public class BookingAPIContext {

        private HashSet<HotelContext> Hotels;
        private Dictionary<Hotel, Booking> Bookings;

        public BookingAPIContext() {
            Hotels = new HashSet<HotelContext>();
            Bookings = new Dictionary<Hotel, Booking>();
        }

        public HashSet<HotelContext> getHotels() {
            return Hotels;
        }

        public bool addHotel(Hotel hotel) {
            return Hotels.Add(new HotelContext(hotel));
        }

       

    }
}
