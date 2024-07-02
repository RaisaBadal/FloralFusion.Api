using FloralFusion.Application.Models.ModelsForUserAndAdminPanel;
using Microsoft.AspNetCore.Identity;

namespace FloralFusion.Application.Interfaces.Admin
{
    public interface IAdminPanelService
    {
        Task<IdentityResult> DeleteRole(string role);
     
        Task<IdentityResult> AddRolesAsync(string roleName);

        Task<IdentityResult> AssignRoleToUserAsync(string userId, string role);

        Task<IdentityResult> DeleteUser(string id);

        Task<IEnumerable<RoleModel>> GetAllRoles();

        Task<IEnumerable<UserModel>> GetAllUser();
    }
}
