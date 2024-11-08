using System.ComponentModel.DataAnnotations;

namespace HotelReservationAPI.Models
{
    public class LoginModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
