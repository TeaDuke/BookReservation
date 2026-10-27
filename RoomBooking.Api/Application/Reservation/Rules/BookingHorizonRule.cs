using Microsoft.Extensions.Options;

namespace RoomBooking.Api.Application.Reservation.Rules;

public class BookingHorizonRule(IOptions<BookingOptions> options, TimeProvider timeProvider) : IBookingRule
{
    public BookingViolation? Validate(BookingContext context)
    {
        int maxAdvanceDays = options.Value.MaxAdvanceDays;
        return context.Start > timeProvider.GetUtcNow().AddDays(maxAdvanceDays).UtcDateTime
            ? new BookingViolation("MAX_ADVANCE_DAYS_EXCEEDED", 
                $"The reservation must starts not later than {maxAdvanceDays} from today")
            : null;
    }
}