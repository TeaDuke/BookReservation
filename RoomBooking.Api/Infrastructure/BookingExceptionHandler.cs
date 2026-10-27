using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Application.Reservation.Rules;

namespace RoomBooking.Api.Infrastructure;

public class BookingExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken ct)
    {
        if (ex is not BookingRuleException e ) return false;

        var problem = new ProblemDetails
        {
            Status = e.Violation.Status, Title = "Booking rule violated", Detail = e.Violation.Message
        };
        problem.Extensions["code"] = e.Violation.Code;
        httpContext.Response.StatusCode = e.Violation.Status;
        await httpContext.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}