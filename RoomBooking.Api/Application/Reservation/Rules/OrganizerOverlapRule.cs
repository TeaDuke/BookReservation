namespace RoomBooking.Api.Application.Reservation.Rules;

public class OrganizerOverlapRule : IBookingRule
{
    public BookingViolation? Validate(BookingContext context)
    {
        return context.OrganizerReservations.Any(r => r.StartDate < context.End && r.EndDate > context.Start)
            ? new BookingViolation("ORGANIZER_OVERLAP", "The organizer is already booked another room for this time", 409)
            : null;
    }
}