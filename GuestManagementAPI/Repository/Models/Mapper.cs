using GuestManagementAPI.DTOs;
using GuestManagementAPI.Models;
using System.Collections;

namespace GuestManagementAPI.Repository.Models
{
    public static class Mapper
    {
        public static ReservationDto ToDto(this ReservationDBModel dbModel)
        {
            return new ReservationDto
            {
                Id = dbModel.Id,
                GuestName = dbModel.GuestName,
                GuestEmail = dbModel.GuestEmail,
                RoomNumber = dbModel.RoomNumber,
                CheckInDate = dbModel.CheckInDate,
                CheckOutDate = dbModel.CheckOutDate,
                Status = Enum.Parse<ReservationStatus>(dbModel.Status),
                NumberOfGuests = dbModel.NumberOfGuests,
                CreatedAt = dbModel.CreatedAt,
                UpdatedAt = dbModel.UpdatedAt
            };
        }

        public static IEnumerable<ReservationDto> ToDto(this IList<ReservationDBModel> dbModel)
        => dbModel.Select(r => r.ToDto());

        public static ReservationDBModel ToNewDbModel(this ReservationDto dto)
        {
            return new ReservationDBModel
            {
                Id = dto.Id,
                GuestName = dto.GuestName,
                GuestEmail = dto.GuestEmail,
                RoomNumber = dto.RoomNumber,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                Status = dto.Status.ToString(),
                NumberOfGuests = dto.NumberOfGuests,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static ReservationDBModel ToUpdateDbModel(this ReservationDto reservation, ReservationDBModel reservationDbModel)
        {
            reservationDbModel.CheckOutDate = reservation.CheckOutDate;
            reservationDbModel.CheckInDate = reservation.CheckInDate;
            reservationDbModel.RoomNumber = reservation.RoomNumber;
            reservationDbModel.Status = reservation.Status.ToString();
            reservationDbModel.GuestEmail = reservation.GuestEmail;
            reservationDbModel.GuestName = reservation.GuestName;
            reservationDbModel.NumberOfGuests = reservation.NumberOfGuests;
            reservationDbModel.UpdatedAt = DateTime.UtcNow;
            return reservationDbModel;
        }

    }
}
