namespace RoomBooking.Api.Application.Reservation.Rules;

public record BookingViolation(string Code, string Message, int Status = 400);