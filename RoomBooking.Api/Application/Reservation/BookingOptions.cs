namespace RoomBooking.Api.Application.Reservation;

public class BookingOptions
{
    public int MaxDurationDays { get; set; } = 7;
    public int MaxAdvanceDays { get; set; } = 90;
}