using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Api.Application.Reservation;

public class CreateReservationRequest
{
    [Range(1, long.MaxValue)]
    public long RoomId { get; set; }
    [Range(1, long.MaxValue)]
    public long OrganizerId { get; set; }

    [Required, StringLength(200)] 
    public string Title { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Attendees { get; set; }
}