namespace EHR.Journey.BasicManagement.OrganizationUnits.Dto;

public class GetUnAddRoleInput : PagingBase
{
    public Guid OrganizationUnitId { get; set; }

    public string Filter { get; set; }
}