using api.src.App.IService;
using api.src.Domain.Entities.Request;
using api.src.Domain.Entities.Shared;
using api.src.Domain.Enums;
using api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IInvitationLinkRepository invitationLinkRepository;


        public AuthController(IAuthService authService, IInvitationLinkRepository invitationLinkRepository)
        {
            this.authService = authService;
            this.invitationLinkRepository = invitationLinkRepository;
        }


        [SwaggerOperation("Регистрация")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPair))]
        [SwaggerResponse(400, "Токен не валиден или активирован")]
        [SwaggerResponse(409, "Почта уже существует")]


        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync(SignUpBody signUpBody)
        {
            string role = Enum.GetName(UserRole.Common)!;

            var link = await invitationLinkRepository.GetAsync(signUpBody.OrganizationCode);
            if (link == null)
                return BadRequest();

            var result = await authService.SignUp(signUpBody, role, link.Department);
            return result;
        }



        [SwaggerOperation("Авторизация")]
        [SwaggerResponse(200, "Успешно", Type = typeof(TokenPair))]
        [SwaggerResponse(400, "Пароли не совпадают")]
        [SwaggerResponse(404, "Email не зарегистрирован")]

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(SignInBody signInBody)
        {
            var result = await authService.SignIn(signInBody);
            return result;
        }

        [SwaggerOperation("Восстановление токена")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPair))]
        [SwaggerResponse(404, "Токен не используется")]

        [HttpPost("token")]
        public async Task<IActionResult> RestoreTokenAsync(RefreshTokenBody body)
        {
            var result = await authService.RestoreToken(body.RefreshToken);
            return result;
        }

    }
}