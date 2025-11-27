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
    //Service used in constructor
    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    } 

    // GET /eventEntity
    [HttpGet]
    public async Task<ActionResult> GetCompanies()
    {
        var groupsList = _groupService.GetManyAsync();
        var groupsDto = groupsList
            .Select(g => new GroupDto(g.Id, g.Name))
            .ToList();

        return Ok(groupsDto);
    }

    // POST /eventEntity
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

    // PUT /eventEntity
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
    // GET /event/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateGroupDto>> GetSingleEventAsync(int id)
    {
        var _group = await _groupService.GetSingleAsync(id);

        return Ok(new CreateGroupDto(_group.Id, _group.Name));
    }
    // DELETE /event/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompanyAsync(int id)
    {
        await _groupService.DeleteAsync(id);
        return NoContent();
    }
}