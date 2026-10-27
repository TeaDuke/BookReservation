namespace RoomBooking.Api.Application.Reports;

public record RoomUsageDto(
    long RoomId, string RoomName, int Floor,
    int TotalReservations, int CompletedCount, int CancelledCount, int EndedEarlyCount,
    double BookedHours, double UsedHours, double UnusedHours,
    double UtilizationPercent, bool IsUnderused);