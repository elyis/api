using api.src.Domain.IRepository;
using api.src.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.src.Web.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UploadController(
            IUserRepository userRepository
            )
        {
            this.userRepository = userRepository;
        }

        [HttpPost("profileIcon"), Authorize]
        [SwaggerOperation("Загрузить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(string))]

        public async Task<IActionResult> UploadProfileIcon([FromHeader(Name = "Authorization")] string token)
        {
            if (Request.Form.Files.Count == 0)
                return BadRequest();

            var file = Request.Form.Files[0];
            var userId = JwtManager.GetUserId(token);
            var filename = await FileUploader.UploadImageAsync(Constants.localPathToProfileIcons, file.OpenReadStream());
            await userRepository.UpdateProfileIconAsync(userId, filename);
            return Ok(new { filename });
        }

        [HttpGet("profileIcon/{filename}")]
        [SwaggerOperation("Получить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetProfileIcon(string filename)
        {
            var bytes = await FileUploader.GetStreamImageAsync(Constants.localPathToProfileIcons, filename);
            if (bytes == null)
                return NotFound();

            return File(bytes, $"image/jpeg", filename);
        }


    }
}