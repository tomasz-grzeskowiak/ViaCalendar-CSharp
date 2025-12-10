namespace APIContracts.DTOs.Calendar;

public record CalendarDto(int Id, int UserId, List<int> EventIds);