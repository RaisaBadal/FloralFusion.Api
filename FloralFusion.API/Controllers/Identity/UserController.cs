using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.User;
using FloralFusion.Application.Models.ModelsForUserAndAdminPanel;
using FloralFusion.Application.response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace FloralFusion.API.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService _userService)
        {
            this._userService = _userService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<UserModel>> Info()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(user)) throw new ArgumentException(ErrorKeys.BadRequest);

            var res=await _userService.Info(user);

            if (res is null) return Response<UserModel>.Error(ErrorKeys.BadRequest);
            return Response<UserModel>.Ok(res);

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<IdentityResult>> RegisterUser([FromBody]UserRegisterModel model)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);

            var res=await _userService.RegisterUserAsync(model.user,model.password);
            if(!res.Succeeded) return Response<IdentityResult>.Error(ErrorKeys.BadRequest);
            return Response<IdentityResult>.Ok(res);

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<SignInResult>> SignIn([FromBody] SignInModel mod)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var res=await _userService.SignInAsync(mod);
            if(!res.Succeeded) return Response<SignInResult>.Error(ErrorKeys.BadRequest);
            return Response<SignInResult>.Ok(res);

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<bool>> SignOutNow()
        {
            var res=await _userService.SignOutAsync();
            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.BadRequest);
        }
    }
}
