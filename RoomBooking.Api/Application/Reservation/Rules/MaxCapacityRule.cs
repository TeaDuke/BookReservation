namespace RoomBooking.Api.Application.Reservation.Rules;

public class MaxCapacityRule : IBookingRule
{
    public BookingViolation? Validate(BookingContext context)
    {
        return context.Attendees > context.room.Capacity
            ? new BookingViolation("EXCEEDED_CAPACITY", "The room capacity is exceeded")
            : null;
    }
}