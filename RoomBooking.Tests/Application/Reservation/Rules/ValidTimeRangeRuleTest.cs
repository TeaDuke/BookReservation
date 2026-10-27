using RoomBooking.Api.Application.Reservation.Rules;

namespace RoomBooking.Tests.Application.Reservation.Rules;

public class ValidTimeRangeRuleTest
{
    [Fact]
    public void Reservation_with_valid_start_time_success()
    {
        var start = TestData.Now.AddHours(1);
        var context = TestData.Context(start: start, end: start.AddMinutes(1));

        var result = new ValidTimeRangeRule().Validate(context);

        Assert.Null(result);
    }
    
    [Fact]
    public void Reservation_with_invalid_start_time_fail()
    {
        var start = TestData.Now.AddHours(1);
        var context = TestData.Context(start: start, end: start.AddMinutes(-1));

        var result = new ValidTimeRangeRule().Validate(context);

        Assert.NotNull(result);
        Assert.Equal("INVALID_TIME_RANGE", result.Code);
    }
}