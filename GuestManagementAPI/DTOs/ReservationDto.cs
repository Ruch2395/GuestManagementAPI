using GuestManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace GuestManagementAPI.DTOs
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public ReservationStatus Status { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
