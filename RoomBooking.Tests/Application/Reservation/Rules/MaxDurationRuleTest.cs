using Microsoft.Extensions.Options;
using RoomBooking.Api.Application.Reservation;
using RoomBooking.Api.Application.Reservation.Rules;

namespace RoomBooking.Tests.Application.Reservation.Rules;

public class MaxDurationRuleTest
{
    private static MaxDurationRule CreateRule(int maxDays = 7) =>
        new(Options.Create(new BookingOptions { MaxDurationDays = maxDays }));

    [Fact]
    public void Allows_exactly_seven_days()
    {
        var start = TestData.Now.AddDays(1);
        var context = TestData.Context(start: start, end: start.AddDays(7));

        var result = CreateRule().Validate(context);

        Assert.Null(result);
    }

    [Fact]
    public void Rejects_seven_days_and_one_second()
    {
        var start = TestData.Now.AddDays(1);
        var context = TestData.Context(start: start, end: start.AddDays(7).AddSeconds(1));

        var result = CreateRule().Validate(context);

        Assert.NotNull(result);
        Assert.Equal("MAX_DURATION_EXCEEDED", result.Code);
    }

    [Fact]
    public void Uses_the_configured_limit()
    {
        var start = TestData.Now.AddDays(1);
        var context = TestData.Context(start: start, end: start.AddDays(3));

        Assert.NotNull(CreateRule(maxDays: 2).Validate(context));
        Assert.Null(CreateRule(maxDays: 3).Validate(context));
    }
}