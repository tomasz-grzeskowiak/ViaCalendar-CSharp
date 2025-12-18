namespace WebAPI.Controllers;
using APIContracts.DTOs.Group;
using Entities;
using APIContracts;
using Microsoft.AspNetCore.Mvc;
using Services.Group;

[ApiController]
[Route("group")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    
    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    } 

    // GET 
    [HttpGet]
    public async Task<ActionResult> GetCompanies()
    {
        var groupsList = _groupService.GetManyAsync();
        var groupsDto = groupsList
            .Select(g => new GroupDto(g.Id, g.Name))
            .ToList();

        return Ok(groupsDto);
    }

    // POST 
    [HttpPost]
    public async Task<ActionResult> CreateGroup([FromBody] CreateGroupDto dto)
    {
        var groupEntity = new Group.Builder()
            .SetId(dto.Id)
            .SetName(dto.Name)
            .Build();

        await _groupService.CreateAsync(groupEntity);
        return CreatedAtAction(nameof(CreateGroup), new { nameof=dto.Name }, dto);
    }

    // PUT 
    [HttpPut]
    public async Task<ActionResult> UpdateGroup([FromBody] CreateGroupDto dto)
    {
        var groupEntity = new Group.Builder()
            .SetId(dto.Id)
            .SetName(dto.Name)
            .Build();

        await _groupService.UpdateAsync(groupEntity);
        return NoContent();
    }
    // GET 
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateGroupDto>> GetSingleGroupAsync(int id)
    {
        var _group = await _groupService.GetSingleAsync(id);

        return Ok(new CreateGroupDto(_group.Id, _group.Name));
    }
    // DELETE 
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompanyAsync(int id)
    {
        await _groupService.DeleteAsync(id);
        return NoContent();
    }
}