namespace APIContracts.DTOs.Event;

public record EventDto(int Id,string Name, string Tag, bool Recursive, int CreatorId, DateTime? Duration,string TypeOfRecursive);