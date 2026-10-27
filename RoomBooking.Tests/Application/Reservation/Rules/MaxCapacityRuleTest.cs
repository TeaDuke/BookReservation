using RoomBooking.Api.Application.Reservation.Rules;

namespace RoomBooking.Tests.Application.Reservation.Rules;

public class MaxCapacityRuleTest
{
    [Fact]
    public void Reservation_room_anough_capacity_success()
    {
        var room = TestData.MakeRoom(capacity: 10);
        var context = TestData.Context(room: room, attendees: 5);

        var result = new MaxCapacityRule().Validate(context);

        Assert.Null(result);
    }
    
    [Fact]
    public void Reservation_with_inactive_room_fail()
    {
        var room = TestData.MakeRoom(capacity: 10);
        var context = TestData.Context(room: room, attendees: 15);

        var result = new MaxCapacityRule().Validate(context);

        Assert.NotNull(result);
        Assert.Equal("EXCEEDED_CAPACITY", result.Code);
    }
}