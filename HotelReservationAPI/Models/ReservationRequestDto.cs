namespace HotelReservationAPI.Models
{
    public class ReservationRequestDto
    {
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
