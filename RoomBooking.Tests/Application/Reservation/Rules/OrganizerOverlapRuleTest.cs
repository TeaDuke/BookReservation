using RoomBooking.Api.Application.Reservation.Rules;

namespace RoomBooking.Tests.Application.Reservation.Rules;

public class OrganizerOverlapRuleTest
{
    [Theory]
    [InlineData(0, 60, true)]
    [InlineData(30, 90, true)]
    [InlineData(-30, 30, true)]
    [InlineData(10, 20, true)]
    [InlineData(-30, 90, true)]
    [InlineData(60, 120, false)]
    [InlineData(-60, 0, false)]
    [InlineData(120, 180, false)]
    public void Detects_overlap(int existingFromMin, int existingToMin, bool expectedOverlap)
    {
        var start = TestData.Now.AddHours(1);
        var existing = TestData.MakeBooking(start.AddMinutes(existingFromMin), start.AddMinutes(existingToMin));
        var context = TestData.Context(start: start, end: start.AddHours(1), organizerReservations: [existing]);

        var result = new OrganizerOverlapRule().Validate(context);

        Assert.Equal(expectedOverlap, result is not null);
        if (expectedOverlap)
            Assert.Equal("ORGANIZER_OVERLAP", result!.Code);
    }
}