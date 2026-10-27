using RoomBooking.Api.Application.Room;

namespace RoomBooking.Api.Application.Reservation;

public interface IReservationService
{
    Task<List<ReservationDto>> GetAsync(ReservationFilter filter); 
    
    // TODO: add create, patch (cancel, end a meeting early) 
}