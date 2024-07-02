using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.User;
using FloralFusion.Application.Models.ModelsForUserAndAdminPanel;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;
using Microsoft.AspNetCore.Identity;

namespace FloralFusion.Application.Services.IdentityServices.UserServices
{
    public class UserService : AbstractClass, IUserService
    {
        public readonly UserManager<User> userManager;
        public readonly SignInManager<User> signInManager;
        public readonly RoleManager<IdentityRole> roleManager;

        public UserService(IUniteOfWork uniteOfWork, IMapper mapper,
            SmtpService smtpService, UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager) : base(uniteOfWork, mapper, smtpService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }


        #region RegisterUserAsync
        public async Task<IdentityResult> RegisterUserAsync(UserModel user, string password)
        {

            if (await userManager.FindByEmailAsync(user.Email) is not null) throw new ArgumentException("Such user is already exists");
            var mapped = mapper.Map<User>(user)
                ?? throw new GeneralException(ErrorKeys.Mapped);

            var res = await userManager.CreateAsync(mapped, password);
            if (!res.Succeeded) throw new InvalidOperationException(ErrorKeys.BadRequest);

            var body = @$"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Registration Successful</title>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h2 style='color: #333333;'>Welcome to Flower Fusion Online Store!</h2>
        <p>Dear {user.Name} {user.Surname},</p>
        <p>Thank you for registering with Flower Fusion Online Store. We are thrilled to have you with us.</p>
        <p>Your account has been successfully created and you can now explore our wide range of products.</p>
        <p>If you have any questions, feel free to contact our support team.</p>
        <p>Best regards,</p>
        <p>The Flower Fusion Team</p>
    </div>
</body>
</html>";
            if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("User email is not valid");
            try
            {
                smtpService.SendMessage(user.Email, $"FlowerFusion new account {DateTime.Now.ToShortTimeString()}", body);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ErrorKeys.EmailNotSend, ex);
            }
            return res;
        }
        #endregion

        #region SignInAsync
        public async Task<SignInResult> SignInAsync(SignInModel mod)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(mod, nameof(mod));
                var user = await userManager.FindByEmailAsync(mod.Email)
                    ?? throw new UserNotFoundException(ErrorKeys.NotFound);
                var res = await signInManager.PasswordSignInAsync(user, mod.Password, true, false);

                var body = @$"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Sign-In Successful</title>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h2 style='color: #333333;'>Sign-In Successful</h2>
        <p>Dear {user.Name} {user.Surname},</p>
        <p>We are pleased to inform you that you have successfully signed in to Flower Fusion Online Store.</p>
        <p>If this wasn't you, please contact our support team immediately to secure your account.</p>
        <p>We hope you enjoy your shopping experience with us.</p>
        <p>Best regards,</p>
        <p>The Flower Fusion Team</p>
    </div>
</body>
</html>";
                if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("Invalid Email");
                smtpService.SendMessage(user.Email,
                    "Sign-In Alert: Welcome Back to Flower Fusion Online Store", body);
                return res;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region SignOutAsync
        public async Task<bool> SignOutAsync()
        {
            try
            {
                await signInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region Info

        public async Task<UserModel> Info(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            var user = await userManager.FindByIdAsync(userId);
            var mapped=mapper.Map<UserModel>(user)
                ?? throw new GeneralException(ErrorKeys.Mapped);
         
            return mapped;
        }
        #endregion

    }
}

