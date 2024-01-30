using EHR.Journey.BasicManagement.Roles.Dtos;

namespace EHR.Journey.BasicManagement.Roles
{
    public interface IRolePermissionAppService : IApplicationService
    {
        
        Task<PermissionOutput> GetPermissionAsync(GetPermissionInput input);

        Task UpdatePermissionAsync(UpdateRolePermissionsInput input);
    }
}