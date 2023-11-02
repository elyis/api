using api.src.Domain.Entities.Response;
using api.src.Domain.IRepository;
using api.src.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public ProfileController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPatch("profile"), Authorize]
        [SwaggerOperation("Изменить вес и рост")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(ProfileBody))]
        [SwaggerResponse(404, Description = "Пользователь отсутсвует")]

        public async Task<IActionResult> ChangeWeightAndHeightAsync(
            [FromHeader(Name = "Authorization")] string token,
            float weight, float height
        )
        {
            var userId = JwtManager.GetUserId(token);
            var result = await userRepository.UpdateWeightAndHeight(userId, weight, height);
            return result == null ? NotFound() : Ok(result.ToProfileBody());
        }


        [HttpGet("profile"), Authorize]
        [SwaggerOperation("Получить профиль")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(ProfileBody))]
        public async Task<IActionResult> GetProfileAsync([FromHeader(Name = "Authorization")] string token)
        {
            var userId = JwtManager.GetUserId(token);
            var user = await userRepository.GetAsync(userId);
            return user == null ? NotFound() : Ok(user.ToProfileBody());
        }
    }
}