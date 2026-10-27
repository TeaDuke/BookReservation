namespace RoomBooking.Api.Application.Reports;

public record RoomUsageReport(DateTime From, DateTime To, List<RoomUsageDto> Rooms);