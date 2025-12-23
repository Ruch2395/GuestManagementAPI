
using System.ComponentModel.DataAnnotations;

namespace GuestManagementAPI.DTOs
{
    public class UpdateReservationDto
    {
        [Required]
        public string GuestName { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string GuestEmail { get; set; } = default!;

        [Required]
        public string RoomNumber { get; set; } = default!;

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Range(1, 10)]
        public int NumberOfGuests { get; set; }
    }
}
