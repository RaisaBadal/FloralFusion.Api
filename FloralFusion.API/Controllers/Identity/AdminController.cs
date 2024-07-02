using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.Admin;
using FloralFusion.Application.Models.ModelsForUserAndAdminPanel;
using FloralFusion.Application.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FloralFusion.API.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize(Roles ="ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminPanelService adminPanelService;

        public AdminController(IAdminPanelService adminPanelService)
        {
            this.adminPanelService = adminPanelService;
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<Response<IdentityResult>> DeleteRole(string role)
        {
            var res=await adminPanelService.DeleteRole(role);
            if (!res.Succeeded) return Response<IdentityResult>.Error(ErrorKeys.BadRequest);
            return Response<IdentityResult>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{roleName}")]
        public async Task<Response<IdentityResult>> AddRoles([FromRoute] string roleName)
        {
            var res=await adminPanelService.AddRolesAsync(roleName);

            if (!res.Succeeded) return Response<IdentityResult>.Error(ErrorKeys.BadRequest);
            return Response<IdentityResult>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{userId}/{role}")]
        public async Task<Response<IdentityResult>> AssignRoleToUser([FromRoute]string userId,[FromRoute] string role)
        {
            var res=await adminPanelService.AssignRoleToUserAsync(userId, role);
            if (!res.Succeeded) return Response<IdentityResult>.Error(ErrorKeys.BadRequest);
            return Response<IdentityResult>.Ok(res);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<Response<IdentityResult>> DeleteUser([FromRoute] string id)
        {
            var res=await adminPanelService.DeleteUser(id);
            if (!res.Succeeded) return Response<IdentityResult>.Error(ErrorKeys.BadRequest);
            return Response<IdentityResult>.Ok(res);
        }

        [HttpGet]
        [Route(nameof(GetAllRoles))]
        public async Task<Response<IEnumerable<RoleModel>>> GetAllRoles()
        {
            var res=await adminPanelService.GetAllRoles();
            if(!res.Any()) return Response<IEnumerable<RoleModel>>.Error(ErrorKeys.BadRequest);
            return Response<IEnumerable<RoleModel>>.Ok(res);
        }

        [HttpGet]
        [Route(nameof(GetAllUser))]
        public async Task<Response<IEnumerable<UserModel>>> GetAllUser()
        {
            var res = await adminPanelService.GetAllUser();
            if (!res.Any()) return Response<IEnumerable<UserModel>>.Error(ErrorKeys.BadRequest);
            return Response<IEnumerable<UserModel>>.Ok(res);
        }
    }
}
