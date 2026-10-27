using RoomBooking.Api.Application.Room;

namespace RoomBooking.Api.Application.Reservation;

public interface IReservationService
{
    Task<List<ReservationDto>> GetAllAsync(ReservationFilter filter); 
    
    Task<ReservationDto> GetByIdAsync(long id);
    
    Task<ReservationDto> CreateAsync(CreateReservationRequest request);
    
    Task<ReservationDto> CancelAsync(long id);
    
    Task<ReservationDto> EndEarlyAsync(long id);
    
    // TODO: add create, patch (cancel, end a meeting early) 
}