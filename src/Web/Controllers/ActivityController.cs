using api.src.Domain.Entities.Response;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository activityRepository;
        private readonly IUserRepository userRepository;

        public ActivityController(
            IActivityRepository activityRepository,
            IUserRepository userRepository
        )
        {
            this.activityRepository = activityRepository;
            this.userRepository = userRepository;
        }

        // [HttpPost("activities")]
        // [SwaggerOperation("Выбрать активности")]
        // [SwaggerResponse(200, Description = "Успешно", Type = typeof(ProfileBody))]
        // [SwaggerResponse(400, Description = "Неверный идентификатор")]

        // public async Task<IActionResult> SelectActivitiesAsync(
        //     [FromHeader(Name = "Authorization")] string token,
        //     List<Guid> activityIds)
        // {
        //     var userId = JwtManager.GetUserId(token);
        //     var user = await userRepository.GetAsync(userId);
        //     if (user == null)
        //         return Unauthorized();

        //     var activities = new List<ActivityModel>();
        //     foreach (var activityId in activityIds)
        //     {
        //         var activity = await activityRepository.GetAsync(activityId);
        //         if (activity == null)
        //             return BadRequest();

        //         activities.Add(activity);
        //     }

        //     var result = await userRepository.AddActivitiesAsync(userId, activities);
        //     return result == null ? NotFound() : Ok(result.ToProfileBody());
        // }

        // [HttpDelete("activities/{activityId}")]
        // [SwaggerOperation("Удалить выбранную активность")]
        // [SwaggerResponse(204)]
        // [SwaggerResponse(404, Description = "Неверный идентификатор")]

        // public async Task<IActionResult> RemoveActivityAsync(
        //     [FromHeader(Name = "Authorization")] string token,
        //     Guid activityId)
        // {
        //     var userId = JwtManager.GetUserId(token);
        //     var user = await userRepository.GetAsync(userId);
        //     if (user == null)
        //         return Unauthorized();

        //     var activity = await activityRepository.GetAsync(activityId);
        //     if (activity == null)
        //         return NotFound();

        //     var result = await userRepository.RemoveActivityAsync(userId, activity);
        //     return NoContent();
        // }


        [HttpPatch("activities/{achivitiesId}"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Изменить значение коэффициента оплаты")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400, Description = "Неверный идентификатор или значение коэффициента")]

        public async Task<IActionResult> ChangePaymentRatio(Guid achivitiesId, float ratio)
        {
            var result = await activityRepository.UpdatePaymentRatio(achivitiesId, ratio);
            return result == null ? BadRequest() : Ok();
        }

        [HttpGet("activities")]
        [SwaggerOperation("Получить все активности")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(IEnumerable<ActivityRatioBody>))]

        public async Task<IActionResult> GetAllActivitiesAsync()
        {
            var result = await activityRepository.GetAllAsync();
            return Ok(result.Select(e => e.ToActivityRatioBody()));
        }
    }
}