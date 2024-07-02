using FloralFusion.Application.Models.ModelsForUserAndAdminPanel;
using Microsoft.AspNetCore.Identity;

namespace FloralFusion.Application.Interfaces.User
{
    public interface IUserService
    {
        Task<UserModel> Info(string userId);
        Task<IdentityResult> RegisterUserAsync(UserModel user, string password);
        Task<SignInResult> SignInAsync(SignInModel mod);
        Task<bool> SignOutAsync();
    }
}
