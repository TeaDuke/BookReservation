using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Application.Reservation.Rules;
using RoomBooking.Api.Application.Room;
using RoomBooking.Api.Application.User;
using RoomBooking.Api.Domain;
using RoomBooking.Api.Infrastructure;

namespace RoomBooking.Api.Application.Reservation;

public class ReservationService(AppDbContext db, BookingValidator validator, TimeProvider time) : IReservationService
{
    
    private DateTime Now() => time.GetUtcNow().UtcDateTime;
    
    private static BookingRuleException Error(string code, string message, int status) =>
        new(new BookingViolation(code, message, status));
    
    private IQueryable<Domain.Reservation> ReservationsWithRelations() =>
        db.Reservations.AsNoTracking()
            .Include(r => r.Room).ThenInclude(room => room.Equipments)
            .Include(r => r.Organizer);
    
    public async Task<List<ReservationDto>> GetAllAsync(ReservationFilter filter)
    {
        var query = ReservationsWithRelations().AsQueryable();

        if (filter.RoomId is not null)
            query = query.Where(r => r.RoomId == filter.RoomId);
        if (filter.OrganizerId is not null)
            query = query.Where(r => r.OrganizerId == filter.OrganizerId);
        if (filter.Status is not null)
            query = query.WithStatus(filter.Status.Value, Now());
        if (filter.FromDate is not null)
            query = query.Where(r => r.StartDate >= filter.FromDate);
        if (filter.ToDate is not null)
            query = query.Where(r => r.EndDate <= filter.ToDate);

        return await query
            .OrderByDescending(r => r.StartDate)
            .ThenByDescending(r => r.EndDate)
            .Select(r => ToDto(r, Now()))
            .ToListAsync();
    }

    public async Task<ReservationDto> GetByIdAsync(long id)
    {
        var reservation = await ReservationsWithRelations().FirstOrDefaultAsync(r => r.Id == id)
                          ?? throw Error("RESERVATION_NOT_FOUND", "Reservation not found.", 404);

        return ToDto(reservation, Now());
    }

    public async Task<ReservationDto> CreateAsync(CreateReservationRequest request)
    {
        var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == request.RoomId)
                   ?? throw Error("ROOM_NOT_FOUND", "Room not found.", 404);

        if (!await db.Users.AnyAsync(u => u.Id == request.OrganizerId))
            throw Error("USER_NOT_FOUND", "Organizer not found.", 404);

        var booked = await db.Reservations.AsNoTracking()
            .Where(r => r.Status == ReservationStatus.Booked
                        && (r.RoomId == request.RoomId || r.OrganizerId == request.OrganizerId))
            .ToListAsync();

        var context = new BookingContext(
            room, request.OrganizerId, request.StartDate, request.EndDate, request.Attendees, Now(),
            booked.Where(r => r.RoomId == request.RoomId).ToList(),
            booked.Where(r => r.OrganizerId == request.OrganizerId).ToList());

        validator.EnsureValid(context);

        var reservation = new Domain.Reservation
        {
            RoomId = request.RoomId, OrganizerId = request.OrganizerId, Title = request.Title,
            StartDate = request.StartDate, EndDate = request.EndDate, Attendees = request.Attendees,
            Status = ReservationStatus.Booked
        };
        db.Reservations.Add(reservation);
        await db.SaveChangesAsync();
    
        return (await GetByIdAsync(reservation.Id));
    }

    public async Task<ReservationDto> CancelAsync(long id)
    {
        var reservation = await db.Reservations.FirstOrDefaultAsync(r => r.Id == id)
                          ?? throw Error("RESERVATION_NOT_FOUND", "Reservation not found.", 404);

        if (reservation.Status != ReservationStatus.Booked || reservation.StartDate <= Now())
            throw Error("CANNOT_CANCEL", "Only a booked reservation that has not started yet can be cancelled.", 409);

        reservation.Status = ReservationStatus.Cancelled;
        await db.SaveChangesAsync();
        return (await GetByIdAsync(id));
    }

    public async Task<ReservationDto> EndEarlyAsync(long id)
    {
        var reservation = await db.Reservations.FirstOrDefaultAsync(r => r.Id == id) 
                          ?? throw Error("RESERVATION_NOT_FOUND", "Reservation not found.", 404);

        var now = Now();
        if (reservation.Status != ReservationStatus.Booked || reservation.StartDate > now || reservation.EndDate <= now)
            throw Error("CANNOT_END", "Only a meeting that is in progress can be ended early.", 409);

        reservation.EndedAt = now;
        reservation.Status = ReservationStatus.Completed;
        await db.SaveChangesAsync();
        return (await GetByIdAsync(id));
    }

    private RoomDto DtoFromRoom(Domain.Room room)
    {
        return new RoomDto(room.Id, room.Name,  room.Capacity, room.Floor, room.IsActive, room.Equipments.Select(e => e.Name).ToList());
    }

    private UserDto DtoFromUser(Domain.User user)
    {
        return new UserDto(user.Id, user.FirstName, user.LastName, 0); // active reservations do not matter here
    }
    
    private ReservationDto ToDto(Domain.Reservation r, DateTime now) => new(
        r.Id, DtoFromRoom(r.Room), DtoFromUser(r.Organizer), r.Title,
        r.StartDate, r.EndDate, r.EndedAt, r.Attendees,
        r.Status == ReservationStatus.Booked && r.EndDate <= now
            ? ReservationStatus.Completed
            : r.Status);
}