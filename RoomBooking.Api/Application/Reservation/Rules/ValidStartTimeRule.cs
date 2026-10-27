namespace RoomBooking.Api.Application.Reservation.Rules;

public class ValidStartTimeRule : IBookingRule
{
    public BookingViolation? Validate(BookingContext context)
    {
        return context.Start <= context.Now
            ? new BookingViolation("START_IN_PAST", "The reservation start must be in the future")
            : null;
    }
}