using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Models {
    public class Booking {
        public Guid Id { get; set; }
        public Guid hotel { get; set; }
        public DateTime from { get; set; }

        public DateTime to { get; set; }

        public int[] rooms { get; set; }
    }
}
