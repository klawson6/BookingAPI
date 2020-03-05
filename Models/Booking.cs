using System;

namespace BookingAPI.Models {
    public class Booking {
        public long Id { get; set; }
        public long hotel { get; set; }
        public DateTime from { get; set; }

        public DateTime to { get; set; }

        public int numSingleRooms { get; set; }
        public int numDoubleRooms { get; set; }
        public int numDeluxeRooms { get; set; }
        public int numSingle { get; set; }
        public int numDouble { get; set; }
        public int numDeluxe { get; set; }
    }
}
