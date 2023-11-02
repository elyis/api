using System.Text.RegularExpressions;
using api.src.Domain.Entities.Request;
using api.src.Domain.Entities.Response;
using api.src.Domain.Enums;
using api.src.Domain.IRepository;
using api.src.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.src.Web.Controllers
{
    [ApiController]
    [Route("api/")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IUserRepository userRepository;
        private readonly ITrainingResultRepository trainingResultRepository;
        private readonly IActivityRepository activityRepository;

        public DepartmentController(
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository,
            ITrainingResultRepository trainingResultRepository,
            IActivityRepository activityRepository
        )
        {
            this.departmentRepository = departmentRepository;
            this.userRepository = userRepository;
            this.trainingResultRepository = trainingResultRepository;
            this.activityRepository = activityRepository;
        }

        [HttpGet("departments")]
        [SwaggerOperation("Получить подразделения организации")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<DepartmentBody>))]

        public async Task<IActionResult> GetDepartments([FromHeader(Name = "Authorization")] string token)
        {
            var userId = JwtManager.GetUserId(token);
            var user = await userRepository.GetAsync(userId);
            var result = await departmentRepository.GetAllDepartmentsByOrganization((Guid)user.OrganizationId);
            return Ok(result.Select(e => e.ToDepartmentBody()));
        }


        [HttpGet("department/analytics/{departmentId}"), Authorize]
        [SwaggerOperation("Получить аналитику по отделу")]
        [SwaggerResponse(200, Type = typeof(DepartmentAnalytics))]
        [SwaggerResponse(404, Description = "Неверныей идентификатор")]


        public async Task<IActionResult> GetAnalyticsDepartmentByMonthAsync(
            [FromHeader(Name = "Authorization")] string token,
            Guid departmentId
        )
        {
            var userId = JwtManager.GetUserId(token);
            var department = await departmentRepository.GetWithWorkers(departmentId);
            if (department == null)
                return NotFound();

            var userInfos = new List<UserInfo>();
            var activityInfo = new List<ActivityInfo>();
            var monthIndex = DateTime.UtcNow.Month;

            foreach (var worker in department.Workers)
            {
                var query = await trainingResultRepository.GetFullByMonth(monthIndex, worker.Id);
                var groupedResults = query
                    .Select(e => e.ToActivityEfficiencyCoefficient())
                    .GroupBy(e => e.ActivityName)
                    .ToList();

                var analyticsByMonth = groupedResults.Select(group =>
                {
                    var averageEfficiencyRatio = group.Average(s => s.EfficiencyRatio);
                    return new UserActivityInfo
                    {
                        ActivityName = group.Key,
                        AverageCountPoints = averageEfficiencyRatio,
                        PointsByMonth = group.Sum(s => s.EfficiencyRatio)
                    };
                }).ToList();

                userInfos.Add(new UserInfo
                {
                    Activities = analyticsByMonth,
                    Fullname = $"{worker.FirstName}{(!string.IsNullOrEmpty(worker.LastName) ? $" {worker.LastName}" : "")}{(!string.IsNullOrEmpty(worker.Patronymic) ? $" {worker.Patronymic}" : "")}"
                });
            }

            var names = Enum.GetNames<Activity>();
            var map = new Dictionary<string, ActivityInfo>();


            foreach (var name in names)
            {
                var activity = await activityRepository.GetAsync(name);
                activity ??= await activityRepository.AddAsync(new CreateActivityBody
                {
                    Name = name
                });

                map.Add(name, new ActivityInfo
                {
                    ActivityName = activity.Name,
                    AverageCountPoint = 0f,
                    PaymentRatio = activity.PaymentRation
                });
            }

            foreach (var userInfo in userInfos)
            {
                if (userInfo.Activities.Count == 0)
                    continue;
                foreach (var activity in userInfo.Activities)
                {
                    var activityName = activity.ActivityName;
                    if (map[activityName].AverageCountPoint == 0)
                        map[activityName].AverageCountPoint = activity.AverageCountPoints;
                    else
                        map[activityName].AverageCountPoint = (map[activityName].AverageCountPoint + activity.AverageCountPoints) / 2f;
                }
            }
            var departmentAnalytics = new DepartmentAnalytics
            {
                ActivityInfos = map.Select(e => e.Value).ToList(),
                UserInfos = userInfos
            };

            return Ok(departmentAnalytics);
        }

        [HttpGet("departments/analytics/{organizationId}"), Authorize]
        [SwaggerOperation("Получить аналитику по отделам организации")]
        [SwaggerResponse(200, Type = typeof(OrganizationAnalytics))]
        [SwaggerResponse(404, Description = "Неверныей идентификатор")]


        public async Task<IActionResult> GetAnalyticsDepartmentsByDate(
            [FromHeader(Name = "Authorization")] string token,
            Guid organizationId
        )
        {
            var departments = await departmentRepository.GetAllDepartmentsByOrganization(organizationId);
            if (!departments.Any())
                return BadRequest();

            var departmentDictionary = new Dictionary<string, DepartmentAnalytics>();

            foreach (var department in departments)
            {
                var userInfos = new List<UserInfo>();
                var activityInfo = new List<ActivityInfo>();
                var monthIndex = DateTime.UtcNow.Month;

                foreach (var worker in department.Workers)
                {
                    var query = await trainingResultRepository.GetFullByMonth(monthIndex, worker.Id);
                    var groupedResults = query
                        .Select(e => e.ToActivityEfficiencyCoefficient())
                        .GroupBy(e => e.ActivityName)
                        .ToList();

                    var analyticsByMonth = groupedResults.Select(group =>
                    {
                        var averageEfficiencyRatio = group.Average(s => s.EfficiencyRatio);
                        return new UserActivityInfo
                        {
                            ActivityName = group.Key,
                            AverageCountPoints = averageEfficiencyRatio,
                            PointsByMonth = group.Sum(s => s.EfficiencyRatio)
                        };
                    }).ToList();

                    userInfos.Add(new UserInfo
                    {
                        Activities = analyticsByMonth,
                        Fullname = $"{worker.FirstName}{(!string.IsNullOrEmpty(worker.LastName) ? $" {worker.LastName}" : "")}{(!string.IsNullOrEmpty(worker.Patronymic) ? $" {worker.Patronymic}" : "")}"
                    });
                }

                var names = Enum.GetNames<Activity>();
                var map = new Dictionary<string, ActivityInfo>();


                foreach (var name in names)
                {
                    var activity = await activityRepository.GetAsync(name);
                    activity ??= await activityRepository.AddAsync(new CreateActivityBody
                    {
                        Name = name
                    });

                    map.Add(name, new ActivityInfo
                    {
                        ActivityName = activity.Name,
                        AverageCountPoint = 0f,
                        PaymentRatio = activity.PaymentRation
                    });
                }

                foreach (var userInfo in userInfos)
                {
                    if (userInfo.Activities.Count == 0)
                        continue;
                    foreach (var activity in userInfo.Activities)
                    {
                        var activityName = activity.ActivityName;
                        if (map[activityName].AverageCountPoint == 0)
                            map[activityName].AverageCountPoint = activity.AverageCountPoints;
                        else
                            map[activityName].AverageCountPoint = (map[activityName].AverageCountPoint + activity.AverageCountPoints) / 2f;
                    }
                }
                var departmentAnalytics = new DepartmentAnalytics
                {
                    ActivityInfos = map.Select(e => e.Value).ToList(),
                    UserInfos = userInfos
                };

                departmentDictionary.Add(department.Name, departmentAnalytics);
            }

            var activityNames = Enum.GetNames<Activity>();
            var activityMapInfo = new Dictionary<string, ActivityInfo>();

            foreach (var name in activityNames)
            {
                var activity = await activityRepository.GetAsync(name);
                activity ??= await activityRepository.AddAsync(new CreateActivityBody
                {
                    Name = name
                });

                activityMapInfo.Add(name, new ActivityInfo
                {
                    ActivityName = activity.Name,
                    AverageCountPoint = 0f,
                    PaymentRatio = activity.PaymentRation
                });
            }

            foreach (var departmentAverages in departmentDictionary.Values)
            {
                if (departmentAverages.ActivityInfos.Count == 0)
                    continue;
                foreach (var averageCountPoint in departmentAverages.ActivityInfos)
                {
                    var activityName = averageCountPoint.ActivityName;
                    if (activityMapInfo[activityName].AverageCountPoint == 0)
                        activityMapInfo[activityName].AverageCountPoint = averageCountPoint.AverageCountPoint;
                    else
                        activityMapInfo[activityName].AverageCountPoint = (activityMapInfo[activityName].AverageCountPoint + averageCountPoint.AverageCountPoint) / 2f;
                }
            }


            var organizationAnalytics = new OrganizationAnalytics
            {
                ActivityInfos = activityMapInfo.Select(e => e.Value).ToList(),
                DepartmentAnalytics = departmentDictionary
            };
            return Ok(organizationAnalytics);
        }
    }
}