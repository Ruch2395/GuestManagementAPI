using GuestManagementAPI.DTOs;

namespace GuestManagementAPI.Services
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateAsync(ReservationDto dto, CancellationToken ct);
        Task<IEnumerable<ReservationDto>> GetAllAsync(
            string? status,
            string? roomNumber,
            string? sortBy,
            string? order, CancellationToken ct);

        Task<ReservationDto> GetByIdAsync(Guid id, CancellationToken ct);
        Task<ReservationDto> UpdateAsync(Guid id, ReservationDto dto, CancellationToken ct);
        Task CheckInAsync(Guid id, CancellationToken ct);
        Task CheckOutAsync(Guid id, CancellationToken ct);
        Task CancelAsync(Guid id, CancellationToken ct);
    }
}
