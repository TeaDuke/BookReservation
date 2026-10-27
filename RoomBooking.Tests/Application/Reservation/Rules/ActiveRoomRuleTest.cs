using RoomBooking.Api.Application.Reservation.Rules;

namespace RoomBooking.Tests.Application.Reservation.Rules;

public class ActiveRoomRuleTest
{
    [Fact]
    public void Reservation_with_active_room_success()
    {
        var room = TestData.MakeRoom(isActive: true);
        var context = TestData.Context(room: room);

        var result = new ActiveRoomRule().Validate(context);

        Assert.Null(result);
    }
    
    [Fact]
    public void Reservation_with_inactive_room_fail()
    {
        var room = TestData.MakeRoom(isActive: false);
        var context = TestData.Context(room: room);

        var result = new ActiveRoomRule().Validate(context);

        Assert.NotNull(result);
        Assert.Equal("ROOM_INACTIVE", result.Code);
    }
}