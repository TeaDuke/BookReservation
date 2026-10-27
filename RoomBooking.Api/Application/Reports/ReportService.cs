using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RoomBooking.Api.Application.Reservation.Rules;
using RoomBooking.Api.Domain;
using RoomBooking.Api.Infrastructure;

namespace RoomBooking.Api.Application.Reports;

public class ReportService(AppDbContext db, TimeProvider time, IOptions<ReportOptions> options) : IReportService
{
    public async Task<RoomUsageReport> GetRoomUsageAsync(ReportFilter filter)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var to = filter.To ?? now;
        var from = filter.From ?? to.AddDays(-30);
        if (from >= to)
            throw new BookingRuleException(new BookingViolation("INVALID_PERIOD", "'from' must be earlier than 'to'."));

        var rooms = await db.Rooms.AsNoTracking().ToListAsync();

        var reservations = await db.Reservations.AsNoTracking()
            .Where(r => r.StartDate >= from && r.StartDate < to)
            .Where(r => r.Status != ReservationStatus.Booked || r.EndDate <= now)
            .ToListAsync();

        var byRoom = reservations.ToLookup(r => r.RoomId);
        var opt = options.Value;

        var rows = rooms.Select(room =>
            {
                var all = byRoom[room.Id].ToList();
                var held = all.Where(r => r.Status != ReservationStatus.Cancelled).ToList();

                var booked = all.Sum(r => (r.EndDate - r.StartDate).TotalHours);
                var used = held.Sum(r => ((r.EndedAt ?? r.EndDate) - r.StartDate).TotalHours);
                var utilization = booked > 0 ? used / booked * 100 : 0;

                return new RoomUsageDto(
                    room.Id, room.Name, room.Floor,
                    all.Count, held.Count, all.Count - held.Count, held.Count(r => r.EndedAt != null),
                    Math.Round(booked, 1), Math.Round(used, 1), Math.Round(booked - used, 1),
                    Math.Round(utilization, 1),
                    all.Count >= opt.MinReservationsForFlag && utilization < opt.LowUtilizationPercent);
            })
            .OrderByDescending(x => x.UnusedHours)
            .ToList();

        return new RoomUsageReport(from, to, rows);
    }
}