namespace RoomBooking.Api.Application.Reservation.Rules;

public class ActiveRoomRule : IBookingRule
{
    public BookingViolation? Validate(BookingContext context)
    {
        return !context.Room.IsActive
            ? new BookingViolation("ROOM_INACTIVE", "The room for reservation must be active")
            : null;
    }
}