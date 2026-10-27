using RoomBooking.Api.Application.Reservation.Rules;

namespace RoomBooking.Tests.Application.Reservation.Rules;

public class ValidStartTimeRuleTest
{
    [Fact]
    public void Reservation_with_valid_start_time_success()
    {
        var now = TestData.Now;
        var context = TestData.Context(start: now.AddHours(1), end: now.AddHours(2));

        var result = new ValidStartTimeRule().Validate(context);

        Assert.Null(result);
    }
    
    [Fact]
    public void Reservation_with_invalid_start_time_fail()
    {
        var now = TestData.Now;
        var context = TestData.Context(start: now.AddHours(-1), end: now.AddHours(2));

        var result = new ValidStartTimeRule().Validate(context);

        Assert.NotNull(result);
        Assert.Equal("START_IN_PAST", result.Code);
    }
}