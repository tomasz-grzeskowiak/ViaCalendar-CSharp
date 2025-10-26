using APIContracts.DTOs;

namespace PersistenceContracts;

public interface ICalendarPersistenceHandler
{
    Task<object> HandleAsync(Request request);
}