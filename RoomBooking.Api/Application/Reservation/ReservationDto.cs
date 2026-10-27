using RoomBooking.Api.Application.Room;
using RoomBooking.Api.Application.User;
using RoomBooking.Api.Domain;

namespace RoomBooking.Api.Application.Reservation;

public record ReservationDto(
    long Id,
    RoomDto Room, 
    UserDto Organizer,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    DateTime? EndedAt,
    int Attendees,
    ReservationStatus Status
);