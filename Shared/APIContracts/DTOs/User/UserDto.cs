namespace APIContracts.DTOs.User;

public record UserDto(int Id,string UserName, string Password, string Email, string FirstName, string LastName, int GroupId = 0);