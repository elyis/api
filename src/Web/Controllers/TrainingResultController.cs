using api.src.Domain.Entities.Request;
using api.src.Domain.Entities.Shared;
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
    public class TrainingResultController : ControllerBase
    {
        private readonly ITrainingResultRepository trainingResultRepository;
        private readonly IActivityRepository activityRepository;
        private readonly IUserRepository userRepository;
        private readonly IDepartmentRepository departmentRepository;

        public TrainingResultController(
            ITrainingResultRepository trainingResultRepository,
            IActivityRepository activityRepository,
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository
        )
        {
            this.trainingResultRepository = trainingResultRepository;
            this.activityRepository = activityRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
        }

        [HttpPost("training")]
        [SwaggerOperation("Добавить результат тренировки")]
        [SwaggerResponse(200)]

        public async Task<IActionResult> CreateTrainingResult(
            [FromHeader(Name = "Authorization")] string token,
            CreateTrainingResultBody body
            )
        {
            var userId = JwtManager.GetUserId(token);
            var user = await userRepository.GetAsync(userId);

            var activityName = Enum.GetName(body.Activity)!;
            var activity = await activityRepository.GetAsync(activityName);
            activity ??= await activityRepository.AddAsync(new CreateActivityBody
            {
                Name = activityName
            });

            var result = await trainingResultRepository.AddAsync(body, activity, user);
            return Ok();
        }

        [HttpGet("training/analytics/{date}"), Authorize]
        [SwaggerOperation("Получить аналитику по активностям за дату")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<TrainingAnalyticsByDate>))]

        public async Task<IActionResult> GetAnalyticsByDate(
            [FromHeader(Name = "Authorization")] string token,
            DateOnly date
        )
        {
            var userId = JwtManager.GetUserId(token);
            var result = await trainingResultRepository.GetByDate(date, userId);

            var analyticsByDate = result
                .Select(e => e.ToActivityEfficiencyCoefficient())
                .GroupBy(e => e.ActivityName)
                .Select(group => new TrainingAnalyticsByDate
                {
                    ActivityName = group.Key,
                    AverageRatio = group.Average(e => e.EfficiencyRatio)
                })
                .ToList();

            return Ok(analyticsByDate);
        }

        [HttpGet("trainings/analytics/{monthIndex}")]
        [SwaggerOperation("Получить ведомость за месяц")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<TrainingAnalyticsByDate>))]

        public async Task<IActionResult> GetAnatylicsByMonth(
            [FromHeader(Name = "Authorization")] string token,
            int monthIndex)
        {
            var userId = JwtManager.GetUserId(token);
            var result = await trainingResultRepository.GetByMonth(monthIndex);

            var analyticsByMonth = result
                .Select(e => e.ToActivityEfficiencyCoefficient())
                .GroupBy(e => e.ActivityName)
                .Select(group => new TrainingAnalyticsByDate
                {
                    ActivityName = group.Key,
                    AverageRatio = group.Average(e => e.EfficiencyRatio)
                })
                .ToList();

            return Ok(analyticsByMonth);
        }
    }
}