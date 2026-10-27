namespace RoomBooking.Api.Application.Reservation.Rules;

public class BookingValidator(IEnumerable<IBookingRule> rules)
{
    public void EnsureValid(BookingContext context)
    {
        foreach (var rule in rules)
            if (rule.Validate(context) is { } violation)
                throw new BookingRuleException(violation);
    }
}