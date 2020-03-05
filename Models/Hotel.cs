namespace BookingAPI.Models {
    public class Hotel {
        public long Id { get; set; }
        public string name { get; set; }
        public int numSingleRooms { get; set; }
        public int numDoubleRooms { get; set; }
        public int numDeluxeRooms { get; set; }
    }
}
