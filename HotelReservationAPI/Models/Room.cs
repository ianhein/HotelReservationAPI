namespace HotelReservationAPI.Models
{
    public class Room
    {
        public int Id { get; set; }
        public required string Number { get; set; }
        public required string Type { get; set; } // Sencilla, Doble, Suite
        public decimal Price { get; set; }
    }
}
