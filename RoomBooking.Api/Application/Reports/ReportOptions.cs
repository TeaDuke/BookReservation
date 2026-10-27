namespace RoomBooking.Api.Application.Reports;

public class ReportOptions
{
    public double LowUtilizationPercent { get; set; } = 50;
    public int MinReservationsForFlag { get; set; } = 3;
}