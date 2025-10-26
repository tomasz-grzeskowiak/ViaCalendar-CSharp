using APIContracts.ENUMs;

namespace APIContracts.DTOs;

public record Request(HandlerType Handler, ActionType Action, object Payload);
