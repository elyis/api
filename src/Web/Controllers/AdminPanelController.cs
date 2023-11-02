using api.src.App.IService;
using api.src.Domain.Entities.Request;
using api.src.Domain.Entities.Response;
using api.src.Domain.Entities.Shared;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using api.src.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.src.Web.Controllers
{
    [ApiController]
    [Route("api/adminpanel")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IInvitationLinkRepository invitationLinkRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IFundSpecializationRepository fundSpecializationRepository;
        private readonly IFundSubgroupRepository fundSubgroupRepository;
        private readonly IActivityRepository activityRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IFundRepository fundRepository;


        public AdminPanelController(
            IUserRepository userRepository,
            IOrganizationRepository organizationRepository,
            IInvitationLinkRepository invitationLinkRepository,
            IFundSpecializationRepository fundSpecializationRepository,
            IFundSubgroupRepository fundSubgroupRepository,
            IActivityRepository activityRepository,
            IDepartmentRepository departmentRepository,
            IFundRepository fundRepository
        )
        {
            this.userRepository = userRepository;
            this.organizationRepository = organizationRepository;
            this.fundSpecializationRepository = fundSpecializationRepository;
            this.fundSubgroupRepository = fundSubgroupRepository;
            this.invitationLinkRepository = invitationLinkRepository;
            this.departmentRepository = departmentRepository;
            this.fundRepository = fundRepository;
            this.activityRepository = activityRepository;

        }

        [HttpPost("signup"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Зарегистрировать компанию")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPair))]
        [SwaggerResponse(400, "Токен не валиден или активирован")]
        [SwaggerResponse(409, "Почта или название компании уже существует")]

        public async Task<IActionResult> SignUpAsync(CreateOrganizationBody organizationBody)
        {
            var organization = await organizationRepository.AddAsync(organizationBody);
            return organization == null ? Conflict() : Ok();
        }

        [HttpGet("inviteLink"), Authorize(Roles = "Organization,Admin")]
        [SwaggerOperation("Создать токен регистрации для сотрудника")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(RefreshTokenBody))]
        [SwaggerResponse(400, Description = "Не удалось создать токен")]

        public async Task<IActionResult> GenerateLink([FromHeader(Name = "Authorization")] string token)
        {
            var userId = JwtManager.GetUserId(token);
            var user = await userRepository.GetWithDepartmentAsync(userId);
            if (user == null)
                return Unauthorized();

            var result = await invitationLinkRepository.AddAsync(user.Department);


            return result == null ? BadRequest() : Ok(new RefreshTokenBody
            {
                RefreshToken = $"{Constants.serverUrl}/inviteLink?code={result.Value}"
            });
        }

        [HttpPost("specialization"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Создать специальность фонда")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(FundSpecializationBody))]
        [SwaggerResponse(409, "Специализация существует")]

        public async Task<IActionResult> CreateSpecializationAsync(CreateFundSpecializationBody body)
        {
            var result = await fundSpecializationRepository.AddAsync(body);
            return result == null ? Conflict() : Ok(result.ToFundSpecializationBody());
        }

        [HttpPost("subgroup/{specializationId}"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Создать подгруппу специальности")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(FundSubgroupBody))]
        [SwaggerResponse(409, "Специализация существует")]

        public async Task<IActionResult> CreateSubgroupAsync(CreateFundSubgroupBody body, Guid specializationId)
        {
            var specialization = await fundSpecializationRepository.GetAsync(specializationId);
            if (specialization == null)
                return NotFound();

            var result = await fundSubgroupRepository.AddAsync(body, specialization);
            return result == null ? Conflict() : Ok(result.ToFundSubgroupBody());
        }

        [HttpPost("activity"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Создать активность")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(ActivityBody))]
        [SwaggerResponse(409, Description = "Активность уже существует")]

        public async Task<IActionResult> CreateActivity(CreateActivityBody body)
        {
            var result = await activityRepository.AddAsync(body);
            return result == null ? Conflict() : Ok(result.ToActivityBody());
        }

        [HttpGet("organizations"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Получить все организации")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<OrganizationBody>))]

        public async Task<IActionResult> GetOrganizations()
        {
            var result = await organizationRepository.GetAllAsync();
            return Ok(result.Select(e => e.ToOrganizationBody()));
        }

        [HttpPost("department/{organizationId}"), Authorize(Roles = "Organization,Admin")]
        [SwaggerOperation("Создать отдел")]
        [SwaggerResponse(200, Type = typeof(DepartmentBody))]
        [SwaggerResponse(409, "Неверный идентификатор организации")]


        public async Task<IActionResult> CreateDepartment(CreateDepartmentBody body, Guid organizationId)
        {
            var organization = await organizationRepository.GetAsync(organizationId);
            if (organization == null)
                return NotFound();

            var result = await departmentRepository.AddAsync(body, organization);
            return result == null ? Conflict() : Ok(result.ToDepartmentBody());
        }

        [HttpPost("fund")]
        [SwaggerOperation("Создать фонд"), Authorize(Roles = "Admin")]
        [SwaggerResponse(200, Type = typeof(FundBody))]
        [SwaggerResponse(400, Description = "Неверный идентификатор")]

        public async Task<IActionResult> CreateFund(CreateFundBody body)
        {
            var subgroups = new List<FundSubgroupsModel>();
            var fund = await fundRepository.GetAsync(body.Name);
            if (fund != null)
                return Conflict();

            foreach (var subgroupId in body.FundSubgroupIds)
            {
                var subgroup = await fundSubgroupRepository.GetAsync(subgroupId);
                if (subgroup == null)
                    return BadRequest();
                subgroups.Add(subgroup);
            }

            var result = await fundRepository.AddAsync(body, subgroups);
            return result == null ? Conflict() : Ok(result.ToFundBody());
        }
    }
}