using GuestManagementAPI.DTOs;
using GuestManagementAPI.Models;
using GuestManagementAPI.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace GuestManagementAPI.Data.Repositories
{
    public class ReservationRepository(AppDbContext _context) : IReservationRepository
    {

        public async Task<ReservationDto> AddAsync(ReservationDto reservationDto, CancellationToken ct)
        {
            var reservation = reservationDto.ToNewDbModel();

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync(ct);
            return reservation?.ToDto();

        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync(string? status,
            string? roomNumber,
            string? sortBy,
            string? order, CancellationToken ct)
        {
            
            IQueryable<ReservationDBModel> query = _context.Reservations;

            if (!string.IsNullOrWhiteSpace(status))
            {
                // Case-insensitive compare; ToLowerInvariant is EF-translatable
                var normalized = status.Trim().ToLowerInvariant();
                query = query.Where(r => r.Status != null && r.Status.ToLower() == normalized);
            }

            if (!string.IsNullOrWhiteSpace(roomNumber))
            {
                var normalizedRoom = roomNumber.Trim().ToLowerInvariant();
                query = query.Where(r => r.RoomNumber != null && r.RoomNumber.ToLower() == normalizedRoom);
            }

            // Sorting
            bool desc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
            query = sortBy?.ToLowerInvariant() switch
            {
                "status" => desc ? query.OrderByDescending(r => r.Status) : query.OrderBy(r => r.Status),
                "roomnumber" => desc ? query.OrderByDescending(r => r.RoomNumber) : query.OrderBy(r => r.RoomNumber),
                "createdat" => desc ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
                _ => query // no sort
            };

            var results = await query.ToListAsync(ct);

            return results?.ToDto();
        }

        public async Task<ReservationDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var dBResponse = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id, ct);
            return dBResponse?.ToDto();
        }

        public async Task<ReservationDto> UpdateAsync(Guid id, ReservationDto reservation, CancellationToken ct)
        {
            var reservationDbModel = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);

            reservationDbModel = reservation.ToUpdateDbModel(reservationDbModel);

            _context.Reservations.Update(reservationDbModel);
            await _context.SaveChangesAsync(ct);
            return reservationDbModel.ToDto();

        }
    }
}
