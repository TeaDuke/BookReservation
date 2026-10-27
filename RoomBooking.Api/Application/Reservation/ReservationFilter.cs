using RoomBooking.Api.Domain;

namespace RoomBooking.Api.Application.Reservation;

public class ReservationFilter
{
    public long? RoomId { get; set; }
    public long? OrganizerId { get; set; }
    public ReservationStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}