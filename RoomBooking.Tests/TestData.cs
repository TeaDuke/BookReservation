using RoomBooking.Api.Application.Reservation.Rules;
using RoomBooking.Api.Domain;

namespace RoomBooking.Tests;

public class TestData
{
    public static readonly DateTime Now = new(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    public static Room MakeRoom(int capacity = 10, bool isActive = true) =>
        new() { Id = 1, Name = "M1", Capacity = capacity, IsActive = isActive };

    public static Reservation MakeBooking(DateTime start, DateTime end) =>
        new() { StartDate = start, EndDate = end, Status = ReservationStatus.Booked };

    public static BookingContext Context(
        Room? room = null,
        long organizerId = 1,
        DateTime? start = null,
        DateTime? end = null,
        int attendees = 5,
        IReadOnlyList<Reservation>? roomReservations = null,
        IReadOnlyList<Reservation>? organizerReservations = null) =>
        new(room ?? MakeRoom(), organizerId,
            start ?? Now.AddHours(1), end ?? Now.AddHours(2),
            attendees, Now,
            roomReservations ?? [], organizerReservations ?? []);
}