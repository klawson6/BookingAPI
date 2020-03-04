using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BookingAPI.Models {
    public class HotelContext : DbContext {
        private const int MAX_ROOMS = 6;

        private string name { get; set; }
        private long id { get; set; }

        private int numSingle { get; set; }

        private int numDouble { get; set; }

        private int numDeluxe { get; set; }

        private HashSet<Booking> bookings;

        public HotelContext(Hotel h) {
            this.id = h.Id;
            this.name = h.name;
            this.numSingle = h.numSingle;
            this.numDouble = h.numDouble;
            this.numDeluxe = h.numDeluxe;
            bookings = new HashSet<Booking>();
        }

        /*
         * Returns a Booking object with the unique id string matching the id parameter, 
         * if it exists in the set of Bookings bookings, else returns null.
         */
        public Booking getBooking(Guid id) {
            foreach (Booking b in bookings) {
                //if (string.Equals(b.Id, id)) return b; // Check if ids equals
                if (Guid.Equals(b.Id, id)) return b; // Check if ids equals
            }
            return null;
        }
        /*
         * Removes the Booking object with the id matching the id parameter, from the set of Bookings bookings. 
         * Returns true if it exists and is removed, else returns false. 
         */
        public bool removeBooking(Guid id) {
            return bookings.Remove(getBooking(id));
        }

        /*
         * Removes the Booking booking provided as a parameter, from the set of Bookings bookings. 
         * Returns true if it exists in the set and is removed, else returns false. 
         */
        public bool removeBooking(Booking booking) {
            return bookings.Remove(booking);
        }

        /*
         * Removes all Booking objects from the set of Bookings bookings.
         */
        public void removeAllBookings() {
            bookings.Clear();
        }

        /*
         * Checks if there are enough rooms available in the given timeframe. 
         * The start/end date of existing bookings can be the same date as the end/start of the booking being checked.
         * Returns true if the required number of rooms are available in the required timeframe, else reurns false.
         * 
         * @params
         * rooms - Array of the number of required rooms for this booking. 
         *         Ints at indexes correspond to:
         *              0 - Single rooms
         *              1 - Double rooms
         *              2 - Deluxe rooms
         * from - DateTime object representing the date the booking begins.
         * to - DateTime object representing the date the booking begins.
         */
        public bool checkAvailabilty(int single, int doub, int deluxe, DateTime from, DateTime to) {
            int[] availability = { numSingle, numDouble, numDeluxe }; // The maximum available rooms
            foreach (Booking b in bookings) { // For each booking
                if (!((b.to <= from && b.from < from) || (b.to > to && b.from >= to))) { // If the booking overlaps the required timeframe at all
                    availability[0] -= b.numSingle;
                    availability[1] -= b.numDouble;
                    availability[2] -= b.numDeluxe;
                }
            }
            // Return true only if there are enough rooms available of every type, required for this booking. 
            return (availability[0] >= single && availability[1] >=doub && availability[2] >= deluxe);
        }

        /*
         * Checks if the number of people given, does not exceed the capacity of the rooms given.
         * Returns true if the number at each index of rooms (the number of each room type required for a booking), 
         * multiplied by the max occupancy per room, is greater than or equal to, the number at the same index of people 
         * (the number of people to fit in each type of room for the booking). 
         * Else returns false
         * 
         * @params
         * rooms - Array of the number of required rooms for this booking. 
         *         Ints at indexes correspond to:
         *              0 - Single rooms
         *              1 - Double rooms
         *              2 - Deluxe rooms
         * people - Array of the number of people for each room type. 
         *          Ints at indexes correspond to:
         *              0 - Single rooms
         *              1 - Double rooms
         *              2 - Deluxe rooms
         */
        private bool checkOccupancy(int single, int doub, int deluxe, int[] people) {
            return (single >= people[0] && doub * 2 >= people[1] && deluxe * 2 >= people[2]);
        }

        /*
         * Attempts to create a new booking for a given number of people in each room type, 
         * in a given number of rooms of each type, between two given dates (inclusive).
         * 
         * Returns a Booking object for the timeframe given, for the given number of rooms, 
         * if the number of people per room type does not exceed the maximum occupancy of the number of 
         * rooms of those types, and there are enough rooms available in the given timeframe. 
         * Else returns null.
         * 
         * @params
         * rooms - Array of the number of required rooms for this booking. 
         *         Ints at indexes correspond to:
         *              0 - Single rooms
         *              1 - Double rooms
         *              2 - Deluxe rooms
         * people - Array of the number of people for each room type. 
         *         Ints at indexes correspond to:
         *              0 - Single rooms
         *              1 - Double rooms
         *              2 - Deluxe rooms
         * from - DateTime object representing the date the booking begins.
         * to - DateTime object representing the date the booking begins.
         */
        public Booking createBooking(int single, int doub, int deluxe, int[] people, DateTime from, DateTime to) {
            if (checkOccupancy(single, doub, deluxe, people) && checkAvailabilty(single, doub, deluxe, from, to)) { // Check a booking can be made to meet the given booking specifications
                Booking booking = new Booking { // Create a new booking
                    from = from,
                    to = to,
                    numSingle = single,
                    numDouble = doub,
                    numDeluxe = deluxe,
                    hotel = id
                };
                bookings.Add(booking); // Add a reference of the new booking to the set of bookings
                return booking;
            }
            return null;
        }
    }
}
