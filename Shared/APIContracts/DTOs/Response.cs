using APIContracts.ENUMs;

namespace APIContracts.DTOs;

public record Response(Status StatusType, object Payload);