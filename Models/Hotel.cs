using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Models {
    public class Hotel {
        public Guid Id { get; set; }
        public string name { get; set; }
        public int numSingle { get; set; }
        public int numDouble { get; set; }
        public int numDeluxe { get; set; }
    }
}
