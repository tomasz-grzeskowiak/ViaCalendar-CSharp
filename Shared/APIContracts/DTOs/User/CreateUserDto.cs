namespace APIContracts.DTOs.User;

public record CreateUserDto(int Id,string UserName, string Password, string Email, string FirstName, string LastName);