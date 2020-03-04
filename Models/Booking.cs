﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Models {
    public class Booking {
        public long Id { get; set; }
        public long hotel { get; set; }
        public DateTime from { get; set; }

        public DateTime to { get; set; }

        public int numSingle { get; set; }
        public int numDouble { get; set; }

        public int numDeluxe { get; set; }

    }
}
