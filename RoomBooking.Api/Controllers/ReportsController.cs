using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Application.Reports;

namespace RoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    [HttpGet("room-usage")]
    public async Task<ActionResult<RoomUsageReport>> RoomUsage([FromQuery] ReportFilter filter)
        => Ok(await reportService.GetRoomUsageAsync(filter));
}