namespace HotelReservationAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public  Room? Room { get; set; }
        public  string? UserId { get; set; }
        public  ApplicationUser? User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
