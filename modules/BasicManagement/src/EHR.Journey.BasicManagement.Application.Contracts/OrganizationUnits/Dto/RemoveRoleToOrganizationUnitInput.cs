namespace EHR.Journey.BasicManagement.OrganizationUnits.Dto;

public class RemoveRoleToOrganizationUnitInput
{
    public Guid RoleId { get; set; }
    
    public Guid OrganizationUnitId { get; set; }
}