
using global::GuestManagementAPI.Models;
using GuestManagementAPI.DTOs;
using GuestManagementAPI.Repository.Models;

namespace GuestManagementAPI.Data.Repositories
{
    public interface IReservationRepository
    {
        Task<ReservationDto> AddAsync(ReservationDto reservation, CancellationToken ct);
        Task<IEnumerable<ReservationDto>> GetAllAsync(string? status,
            string? roomNumber,
            string? sortBy,
            string? order, CancellationToken ct);
        Task<ReservationDto?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<ReservationDto> UpdateAsync(Guid id, ReservationDto reservation, CancellationToken ct);
    }
}
