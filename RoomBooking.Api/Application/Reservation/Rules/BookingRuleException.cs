namespace RoomBooking.Api.Application.Reservation.Rules;

public class BookingRuleException(BookingViolation violation) : Exception(violation.Message)
{
    public BookingViolation Violation { get; } = violation;
}