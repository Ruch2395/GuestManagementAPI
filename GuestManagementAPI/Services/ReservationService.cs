using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuestManagementAPI.Data.Repositories;
using GuestManagementAPI.DTOs;
using GuestManagementAPI.Models;

namespace GuestManagementAPI.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReservationResponseDto> CreateAsync(CreateReservationDto dto)
        {
            if (dto.CheckOutDate <= dto.CheckInDate)
                throw new InvalidOperationException("Check-out date must be after check-in date.");

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                GuestName = dto.GuestName,
                GuestEmail = dto.GuestEmail,
                RoomNumber = dto.RoomNumber,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                NumberOfGuests = dto.NumberOfGuests,
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(reservation);
            return MapToDto(reservation);
        }

        public async Task<IEnumerable<ReservationResponseDto>> GetAllAsync(
            string? status,
            string? roomNumber,
            string? sortBy,
            string? order)
        {
            var reservations = await _repository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<ReservationStatus>(status, true, out var parsedStatus))
            {
                reservations = reservations.Where(r => r.Status == parsedStatus);
            }

            if (!string.IsNullOrWhiteSpace(roomNumber))
            {
                reservations = reservations.Where(r => r.RoomNumber == roomNumber);
            }

            reservations = sortBy?.ToLower() switch
            {
                "checkindate" => order == "desc"
                    ? reservations.OrderByDescending(r => r.CheckInDate)
                    : reservations.OrderBy(r => r.CheckInDate),
                _ => reservations
            };

            return reservations.Select(MapToDto);
        }

        public async Task<ReservationResponseDto> GetByIdAsync(Guid id)
        {
            var reservation = await GetReservationOrThrow(id);
            return MapToDto(reservation);
        }

        public async Task<ReservationResponseDto> UpdateAsync(Guid id, UpdateReservationDto dto)
        {
            var reservation = await GetReservationOrThrow(id);

            if (reservation.Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Only pending reservations can be updated.");

            reservation.GuestName = dto.GuestName;
            reservation.GuestEmail = dto.GuestEmail;
            reservation.RoomNumber = dto.RoomNumber;
            reservation.CheckInDate = dto.CheckInDate;
            reservation.CheckOutDate = dto.CheckOutDate;
            reservation.NumberOfGuests = dto.NumberOfGuests;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(reservation);
            return MapToDto(reservation);
        }

        public async Task CheckInAsync(Guid id)
        {
            var reservation = await GetReservationOrThrow(id);

            if (reservation.Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Only pending reservations can be checked in.");

            reservation.Status = ReservationStatus.CheckedIn;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(reservation);
        }

        public async Task CheckOutAsync(Guid id)
        {
            var reservation = await GetReservationOrThrow(id);

            if (reservation.Status != ReservationStatus.CheckedIn)
                throw new InvalidOperationException("Only checked-in reservations can be checked out.");

            reservation.Status = ReservationStatus.CheckedOut;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(reservation);
        }

        public async Task CancelAsync(Guid id)
        {
            var reservation = await GetReservationOrThrow(id);

            reservation.Status = ReservationStatus.Cancelled;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(reservation);
        }

        private async Task<Reservation> GetReservationOrThrow(Guid id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            return reservation ?? throw new KeyNotFoundException("Reservation not found.");
        }

        private static ReservationResponseDto MapToDto(Reservation r)
        {
            return new ReservationResponseDto
            {
                Id = r.Id,
                GuestName = r.GuestName,
                GuestEmail = r.GuestEmail,
                RoomNumber = r.RoomNumber,
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                NumberOfGuests = r.NumberOfGuests,
                Status = r.Status.ToString()
            };
        }
    }
}
