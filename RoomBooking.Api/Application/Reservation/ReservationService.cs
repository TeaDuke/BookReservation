using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Application.Room;
using RoomBooking.Api.Application.User;
using RoomBooking.Api.Infrastructure;

namespace RoomBooking.Api.Application.Reservation;

public class ReservationService(AppDbContext db) : IReservationService
{
    public async Task<List<ReservationDto>> GetAsync(ReservationFilter filter)
    {
        var query = db.Reservations.AsNoTracking().AsQueryable();

        if (filter.RoomId is not null)
            query = query.Where(r => r.RoomId == filter.RoomId);
        if (filter.OrganizerId is not null)
            query = query.Where(r => r.OrganizerId == filter.OrganizerId);
        if (filter.Status is not null)
            query = query.Where(r => r.Status == filter.Status);
        if (filter.FromDate is not null)
            query = query.Where(r => r.StartDate >= filter.FromDate);
        if (filter.ToDate is not null)
            query = query.Where(r => r.EndDate <= filter.ToDate);

        return await query
            .OrderByDescending(r => r.StartDate)
            .ThenByDescending(r => r.EndDate)
            .Select(r => new ReservationDto(r.Id, dtoFromRoom(r.Room),
                dtoFromUser(r.Organizer), r.Title, r.StartDate, r.EndDate, 
                r.EndedAt, r.Attendees, r.Status))
            .ToListAsync();
    }

    private RoomDto dtoFromRoom(Domain.Room room)
    {
        return new RoomDto(room.Id, room.Name,  room.Capacity, room.Floor, room.IsActive, room.Equipments.Select(e => e.Name).ToList());
    }

    private UserDto dtoFromUser(Domain.User user)
    {
        return new UserDto(user.Id, user.FirstName, user.LastName, 0); // active reservations do not matter here
    }
}