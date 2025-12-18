namespace WebAPI.Controllers;
using APIContracts.DTOs.User;
using Entities;
using APIContracts;
using Microsoft.AspNetCore.Mvc;
using Services.User;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
   
    public UserController(IUserService userService)
    {
        _userService = userService;
    } 

    // GET 
    [HttpGet]
    public async Task<ActionResult> GetCompanies()
    {
        var usersList = _userService.GetManyAsync();
        var usersDto = usersList
            .Select(u => new UserDto(u.Id, u.UserName, u.Password, u.Email, u.FirstName, u.LastName, u.GroupId))
            .ToList();

        return Ok(usersDto);
    }

    // POST 
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        var userEntity = new User.Builder()
            .SetId(dto.Id)
            .SetUsername(dto.UserName)
            .SetPassword(dto.Password)
            .SetEmail(dto.Email)
            .SetFirstName(dto.FirstName)
            .SetLastName(dto.LastName)
            .SetGroupId(dto.GroupId)
            .Build();

        await _userService.CreateAsync(userEntity);
        return CreatedAtAction(nameof(CreateUser), new { nameof=dto.UserName, dto.Password, dto.Email, dto.FirstName, dto.LastName }, dto);
    }

    // PUT 
    [HttpPut]
    public async Task<ActionResult> UpdateUser([FromBody] CreateUserDto dto)
    {
        var userEntity = new User.Builder()
            .SetId(dto.Id)
            .SetUsername(dto.UserName)
            .SetPassword(dto.Password)
            .SetEmail(dto.Email)
            .SetFirstName(dto.FirstName)
            .SetLastName(dto.LastName)
            .SetGroupId(dto.GroupId)
            .Build();

        await _userService.UpdateAsync(userEntity);
        return NoContent();
    }
    // GET 
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateUserDto>> GetSingleUserAsync(int id)
    {
        var _user = await _userService.GetSingleAsync(id);

        return Ok(new CreateUserDto(_user.Id, _user.UserName, _user.Password, _user.Email, _user.FirstName, _user.LastName, _user.GroupId));
    }
    // DELETE 
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompanyAsync(int id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}