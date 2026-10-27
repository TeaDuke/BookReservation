namespace RoomBooking.Api.Application.User;

public record UserDto(long Id, string FirstName, string LastName, int ActiveReservations);