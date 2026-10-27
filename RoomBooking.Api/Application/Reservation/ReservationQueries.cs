using RoomBooking.Api.Domain;

namespace RoomBooking.Api.Application.Reservation;

public static class ReservationQueries
{
    public static IQueryable<Domain.Reservation> WithStatus(
        this IQueryable<Domain.Reservation> query, ReservationStatus status, DateTime now) => status switch
    {
        ReservationStatus.Booked => query.Where(r => r.Status == ReservationStatus.Booked && r.EndDate > now),
        ReservationStatus.Completed => query.Where(r => r.Status == ReservationStatus.Completed
                                                        || (r.Status == ReservationStatus.Booked && r.EndDate <= now)),
        _ => query.Where(r => r.Status == status)
    };

}