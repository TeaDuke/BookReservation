using Microsoft.Extensions.Options;
using RoomBooking.Api.Application.Reservation;
using RoomBooking.Api.Application.Reservation.Rules;

namespace RoomBooking.Tests.Application.Reservation.Rules;

public class BookingHorizonRuleTest
{
    private static BookingHorizonRule CreateRule(int maxAdvance = 90) =>
        new(Options.Create(new BookingOptions { MaxAdvanceDays = maxAdvance }));

    [Fact]
    public void Allows_exactly_ninety_days()
    {
        var start = TestData.Now.AddDays(90);
        var context = TestData.Context(start: start, end: start.AddHours(2));

        var result = CreateRule().Validate(context);

        Assert.Null(result);
    }

    [Fact]
    public void Rejects_ninty_days_and_one_second()
    {
        var start = TestData.Now.AddDays(90).AddSeconds(1);
        var context = TestData.Context(start: start, end: start.AddHours(2));

        var result = CreateRule().Validate(context);

        Assert.NotNull(result);
        Assert.Equal("MAX_ADVANCE_DAYS_EXCEEDED", result.Code);
    }

    [Fact]
    public void Uses_the_configured_limit()
    {
        var start = TestData.Now.AddDays(10);
        var context = TestData.Context(start: start, end: start.AddHours(2));

        Assert.NotNull(CreateRule(maxAdvance: 9).Validate(context));
        Assert.Null(CreateRule(maxAdvance: 11).Validate(context));
    }
}