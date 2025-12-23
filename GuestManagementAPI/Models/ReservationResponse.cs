namespace GuestManagementAPI.Models
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public string GuestName { get; set; } = default!;
        public string GuestEmail { get; set; } = default!;
        public string RoomNumber { get; set; } = default!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = default!;
    }
}
