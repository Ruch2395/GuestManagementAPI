using GuestManagementAPI.Models;

namespace GuestManagementAPI.Repository.Models
{
    public record ReservationDBModel
    {
        public Guid Id { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
