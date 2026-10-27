namespace RoomBooking.Api.Application.Reservation.Rules;

public class RoomOverlapRule : IBookingRule
{
    public BookingViolation? Validate(BookingContext context)
    {
        return context.RoomReservations.Any(r => r.StartDate < context.End && r.EndDate > context.Start)
            ? new BookingViolation("ROOM_OVERLAP", "The room is already booked for this time", 409)
            : null;
    }
}