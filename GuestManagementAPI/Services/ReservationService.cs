using GuestManagementAPI.Data.Repositories;
using GuestManagementAPI.DTOs;
using GuestManagementAPI.Models;

namespace GuestManagementAPI.Services
{
    public class ReservationService(IReservationRepository _repository) : IReservationService
    {

        public async Task<ReservationDto> CreateAsync(ReservationDto dto, CancellationToken ct)
        {
            dto.Status = ReservationStatus.Pending;
            if (dto.CheckOutDate <= dto.CheckInDate)
                throw new InvalidOperationException("Check-out date must be after check-in date.");

             return await _repository.AddAsync(dto, ct);
        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync(
            string? status,
            string? roomNumber,
            string? sortBy,
            string? order, CancellationToken ct)
        {
            return await _repository.GetAllAsync(status,roomNumber,sortBy,order, ct);
        }

        public async Task<ReservationDto> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await GetReservationOrThrow(id, ct);
        }

        public async Task<ReservationDto> UpdateAsync(Guid id, ReservationDto dto, CancellationToken ct)
        {
            var reservation = await GetReservationOrThrow(id, ct);
            dto.Status = reservation.Status;
            if (reservation.Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Only pending reservations can be updated.");
            
            return await _repository.UpdateAsync(id,dto, ct);
        }

        public async Task CheckInAsync(Guid id, CancellationToken ct)
        {
            var reservation = await GetReservationOrThrow(id, ct);

            if (reservation.Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Only pending reservations can be checked in.");
            ReservationDto updateReservationDto = UpdateStatus(reservation, ReservationStatus.CheckedIn);

            await _repository.UpdateAsync(id, updateReservationDto, ct);
        }

       

        public async Task CheckOutAsync(Guid id, CancellationToken ct)
        {
            var reservation = await GetReservationOrThrow(id, ct);

            if (reservation.Status != ReservationStatus.CheckedIn)
                throw new InvalidOperationException("Only checked-in reservations can be checked out.");
            ReservationDto updateReservationDto = UpdateStatus(reservation, ReservationStatus.CheckedOut);
            
            await _repository.UpdateAsync(id,updateReservationDto, ct);
        }

        public async Task CancelAsync(Guid id, CancellationToken ct)
        {
            var reservation = await GetReservationOrThrow(id, ct);

            ReservationDto updateReservationDto = UpdateStatus(reservation, ReservationStatus.Cancelled);
            
            await _repository.UpdateAsync(id, updateReservationDto, ct);
        }

        private async Task<ReservationDto> GetReservationOrThrow(Guid id, CancellationToken ct)
        {
            var reservation = await _repository.GetByIdAsync(id, ct);
            return reservation ?? throw new KeyNotFoundException("Reservation not found.");
        }

        private static ReservationDto UpdateStatus(ReservationDto reservation, ReservationStatus reservationStatus)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                GuestName = reservation.GuestName,
                GuestEmail = reservation.GuestEmail,
                RoomNumber = reservation.RoomNumber,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                NumberOfGuests = reservation.NumberOfGuests,
                Status = reservationStatus,
            };
        }
    }
}
