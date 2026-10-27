namespace RoomBooking.Api.Application.Reservation.Rules;

public record BookingContext(
    Domain.Room Room, long OrganizerId, DateTime Start, DateTime End, int Attendees,
    DateTime Now, 
    IReadOnlyList<Domain.Reservation> RoomReservations,
    IReadOnlyList<Domain.Reservation> OrganizerReservations
    );