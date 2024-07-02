using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.Admin;
using FloralFusion.Application.Models.ModelsForUserAndAdminPanel;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Application.Services.IdentityServices.AdminPanel
{
    public class AdminPanelService : AbstractClass, IAdminPanelService
    {
        public readonly UserManager<User> userManager;
        public readonly SignInManager<User> signInManager;
        public readonly RoleManager<IdentityRole> roleManager;

        public AdminPanelService(IUniteOfWork uniteOfWork, IMapper mapper,
            SmtpService smtpService, UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager) : base(uniteOfWork, mapper, smtpService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        #region AddRolesAsync
        public async Task<IdentityResult> AddRolesAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException(ErrorKeys.ArgumentNull);
            if (await roleManager.RoleExistsAsync(roleName.ToUpper())) throw new ArgumentException("Such role is already exists");

            var res=await roleManager.CreateAsync(new IdentityRole(roleName.ToUpper()));
            if (!res.Succeeded) throw new InvalidOperationException(ErrorKeys.BadRequest);
            return res;
            
        }

        #endregion

        #region AssignRoleToUserAsync
        public async Task<IdentityResult> AssignRoleToUserAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentException(ErrorKeys.ArgumentNull);
            if (!await roleManager.RoleExistsAsync(role.ToUpper())) throw new ArgumentException("Such role is not exists");

            var user = await userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException(ErrorKeys.BadRequest);

            var res=await userManager.AddToRoleAsync(user, role);
            if (!res.Succeeded) throw new InvalidOperationException(ErrorKeys.BadRequest);
            return res;
        }
        #endregion

        #region DeleteRole

        public async Task<IdentityResult> DeleteRole(string role)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentException(ErrorKeys.ArgumentNull);
            if (!await roleManager.RoleExistsAsync(role.ToUpper())) throw new ArgumentException("Such role is not exists");

            var res=await roleManager.DeleteAsync(new IdentityRole(role.ToUpper()));
            if (!res.Succeeded) throw new InvalidOperationException(ErrorKeys.BadRequest);
            return res;
        }
        #endregion

        #region DeleteUser

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id)
                ?? throw new ArgumentException(ErrorKeys.NotFound);

            var res=await userManager.DeleteAsync(user);
            if (!res.Succeeded) throw new InvalidOperationException(ErrorKeys.BadRequest);
            return res;

        }
        #endregion

        #region GetAllRoles
        public async Task<IEnumerable<RoleModel>> GetAllRoles()
        {
            return  await roleManager.Roles.Select(i=>new RoleModel()
            { 
                Name=i.Name
            }).ToListAsync();
        }
        #endregion

        #region GetAllUser
        public async Task<IEnumerable<UserModel>> GetAllUser()
        {
            var res = await userManager.Users.ToListAsync();
            var mapped = mapper.Map<IEnumerable<UserModel>>(res)
                ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;

        }
        #endregion

    }
}
