namespace RoomBooking.Api.Application.Reservation.Rules;

public class ValidTimeRangeRule : IBookingRule
{
    public BookingViolation? Validate(BookingContext context)
    {
        return context.End <= context.Start
            ? new BookingViolation("INVALID_TIME_RANGE", "The reservation must end after start")
            : null;
    }
}