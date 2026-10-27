namespace RoomBooking.Api.Application.Reports;

public interface IReportService
{
    Task<RoomUsageReport> GetRoomUsageAsync(ReportFilter filter);
}