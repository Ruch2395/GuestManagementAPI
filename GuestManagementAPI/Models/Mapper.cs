using GuestManagementAPI.DTOs;

namespace GuestManagementAPI.Models
{
    public static class Mapper
    {
        public static ReservationResponse ToModel(this ReservationDto reservationDto)
        {
            return new ReservationResponse
            {
                Id = reservationDto.Id,
                GuestName = reservationDto.GuestName,
                GuestEmail = reservationDto.GuestEmail,
                RoomNumber = reservationDto.RoomNumber,
                CheckInDate = reservationDto.CheckInDate,
                CheckOutDate = reservationDto.CheckOutDate,
                NumberOfGuests = reservationDto.NumberOfGuests,
                Status = reservationDto.Status.ToString()
            };
        }


        public static ReservationDto ToDomain(this ReservationRequest reservationRequest)
        {
            return new ReservationDto {
                GuestName = reservationRequest.GuestName,
                GuestEmail = reservationRequest.GuestEmail,
                RoomNumber = reservationRequest.RoomNumber,
                CheckInDate = reservationRequest.CheckInDate,
                CheckOutDate = reservationRequest.CheckOutDate,
                NumberOfGuests = reservationRequest.NumberOfGuests,
            };
        }

        public static ReservationDto ToDomain(this UpdateReservation reservationRequest)
        {
            return new ReservationDto
            {
                GuestName = reservationRequest.GuestName,
                GuestEmail = reservationRequest.GuestEmail,
                RoomNumber = reservationRequest.RoomNumber,
                CheckInDate = reservationRequest.CheckInDate,
                CheckOutDate = reservationRequest.CheckOutDate,
                NumberOfGuests = reservationRequest.NumberOfGuests
            };
        }

        public static IList<ReservationResponse> ToModel(this IEnumerable<ReservationDto> reservationDtos)
            => reservationDtos.Select(dto => dto.ToModel()).ToList();
    }
}
