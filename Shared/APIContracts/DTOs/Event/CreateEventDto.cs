namespace APIContracts.DTOs.Event;

public record CreateEventDto(
    string Name,
    string Tag,
    bool Recursive,
    int CreatorId,
    DateTime Start,
    DateTime End,
    string TypeOfRecursive
);