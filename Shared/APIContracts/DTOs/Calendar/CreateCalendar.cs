namespace APIContracts.DTOs.Calendar;

public record CreateCalendarDto(int Id, int UserId, List<int> EventIds);