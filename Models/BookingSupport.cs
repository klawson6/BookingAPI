using System;
using System.Collections.Generic;

namespace BookingAPI.Models {
    public static class BookingSupport {

        /*
        * Checks if there are enough rooms available in the given timeframe. 
        * The start/end date of existing bookings can be the same date as the end/start of the booking being checked.
        * Returns true if the required number of rooms are available in the required timeframe, else returns false.
        * 
        * @params
        * booking - The Booking object representing the number of rooms available, the number of people per room, and the timeframe
        * hotel - The Hotel object representing the hotel the booking is for
        * bookings - The bookings already booked into the hotel
        */
        public static bool checkAvailabilty(Booking booking, Hotel h, List<Booking> bookings) {
            int[] availability = { h.numSingleRooms, h.numDoubleRooms, h.numDeluxeRooms }; // The maximum available rooms
            foreach (Booking b in bookings) { // For each booking
                //if (!((b.to <= booking.from && b.from < booking.from) || (b.to > booking.to && b.from >= booking.to))) { // If the booking overlaps the required timeframe at all
                if (!(((DateTime.Compare(b.from, booking.from) < 0 && DateTime.Compare(b.to, booking.from) <= 0)) 
                    || ((DateTime.Compare(b.to, booking.to) >= 0 && DateTime.Compare(b.to, booking.to) > 0)))) { // If the booking overlaps the required timeframe at all
                    // Decrement the available rooms, by the rooms used in this booking
                    availability[0] -= b.numSingleRooms;
                    availability[1] -= b.numDoubleRooms;
                    availability[2] -= b.numDeluxeRooms;
                }
            }
            // Return true only if there are enough rooms available of every type, required for this booking. 
            return (availability[0] >= booking.numSingleRooms && availability[1] >= booking.numDoubleRooms && availability[2] >= booking.numDeluxeRooms);
        }

        /*
         * Checks if the number of people in the booking, does not exceed the capacity of the rooms in the booking.
         * Returns true if the number of each of the rooms in the booking, 
         * multiplied by the max occupancy per room type, is greater than or equal to, the number of people in the booking for each room type.  
         * Else returns false
         * 
         * @params
         * booking - The Booking object representing the number of rooms available, the number of people per room, and the timeframe
         */
        private static bool checkOccupancy(Booking b) {
            return (b.numSingleRooms >= b.numSingle && b.numDoubleRooms * 2 >= b.numDouble && b.numDeluxeRooms * 2 >= b.numDeluxe);
        }

        /*
         * Attempts to create a new booking for a given number of people in each room type, 
         * in a given number of rooms of each type, between two given dates (inclusive).
         * 
         * Returns the Booking object being checked if the number of people per room type does not exceed the maximum occupancy of the number of 
         * rooms of those types, and there are enough rooms available in the given timeframe, in the hotel. 
         * Else returns null.
         * 
         * @params
         * booking - The Booking object representing the number of rooms available, the number of people per room, and the timeframe
         * hotel - The Hotel object representing the hotel the booking is for
         * bookings - The bookings already booked into the hotel
         */
        public static Booking createBooking(Booking booking, Hotel hotel, List<Booking> bookings) {
            if (checkOccupancy(booking)
                && checkAvailabilty(booking, hotel, bookings)) { // Check a booking can be made to meet the given booking specifications
                return booking;
            }
            return null;
        }
    }
}
