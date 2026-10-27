namespace RoomBooking.Api.Application.Reservation.Rules;

public interface IBookingRule
{
    BookingViolation? Validate(BookingContext context);
}