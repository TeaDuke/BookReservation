namespace RoomBooking.Api.Domain;

public class Reservation
{
    public long Id { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public int OrganizerId { get; set; }
    public User Organizer { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? EndedAt { get; set; }
    public int Attendees { get; set; }
    public ReservationStatus Status { get; set; }
}