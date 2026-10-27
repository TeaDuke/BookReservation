using Microsoft.Extensions.Options;

namespace RoomBooking.Api.Application.Reservation.Rules;

public class MaxDurationRule(IOptions<BookingOptions> options) : IBookingRule
{
    public BookingViolation? Validate(BookingContext context)
    {
        var maxDays = options.Value.MaxDurationDays;
        return context.End - context.Start > TimeSpan.FromDays(maxDays)
            ? new BookingViolation("MAX_DURATION_EXCEEDED",
                $"A reservation cannot last longer than {maxDays} days.")
            : null;
    }
}