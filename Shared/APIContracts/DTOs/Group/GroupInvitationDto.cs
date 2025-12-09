namespace APIContracts.DTOs.Group;

public class GroupInvitationDto
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; } = "";
    public int InvitedUserId { get; set; }
    public int InvitedByUserId { get; set; }
    public DateTime InvitedDate { get; set; }
    public string Status { get; set; } = "Pending";
}