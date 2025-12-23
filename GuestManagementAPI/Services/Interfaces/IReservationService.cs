using GuestManagementAPI.DTOs;

namespace GuestManagementAPI.Services
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateAsync(CreateReservationDto dto);
        Task<IEnumerable<ReservationResponseDto>> GetAllAsync(
            string? status,
            string? roomNumber,
            string? sortBy,
            string? order);

        Task<ReservationResponseDto> GetByIdAsync(Guid id);
        Task<ReservationResponseDto> UpdateAsync(Guid id, UpdateReservationDto dto);
        Task CheckInAsync(Guid id);
        Task CheckOutAsync(Guid id);
        Task CancelAsync(Guid id);
    }
}
