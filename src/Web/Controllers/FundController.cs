using api.src.Domain.Entities.Response;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Utility;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class FundController : ControllerBase
    {
        private readonly IFundSpecializationRepository fundSpecializationRepository;
        private readonly IFundSubgroupRepository fundSubgroupRepository;
        private readonly IFundRepository fundRepository;
        private readonly IUserRepository userRepository;


        public FundController(
            IFundSpecializationRepository fundSpecializationRepository,
            IFundSubgroupRepository fundSubgroupRepository,
            IUserRepository userRepository,
            IFundRepository fundRepository
        )
        {
            this.fundSpecializationRepository = fundSpecializationRepository;
            this.fundSubgroupRepository = fundSubgroupRepository;
            this.userRepository = userRepository;
            this.fundRepository = fundRepository;
        }

        [HttpGet("funds/{subgroupId}")]
        [SwaggerOperation("Получить все фонды по подгруппе специализации")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<FundBody>))]
        [SwaggerResponse(404, Description = "Неверный идентификатор")]


        public async Task<IActionResult> GetFundsBySubgroupId(Guid subgroupId)
        {
            var subgroup = await fundSubgroupRepository.GetWithFundsAsync(subgroupId);
            if (subgroup == null)
                return NotFound();

            var result = subgroup.Funds.Select(e => e.ToFundBody());
            return Ok(result);
        }

        [HttpGet("funds")]
        [SwaggerOperation("Получить выбранные фонды")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<FundBody>))]

        public async Task<IActionResult> GetUserFundsAsync([FromHeader(Name = "Authorization")] string token)
        {
            var userId = JwtManager.GetUserId(token);
            var user = await userRepository.GetWithFundsAsync(userId);
            if (user == null)
                return NotFound();

            var result = user.EndowmentFunds.Select(e => e.ToFundBody());
            return Ok(result);
        }


        [HttpPost("funds")]
        [SwaggerOperation("Выбрать фонды")]
        [SwaggerResponse(200, Type = typeof(ProfileBody))]
        [SwaggerResponse(400, Description = "Неверный идентификатор")]

        public async Task<IActionResult> SelectFundsAsync(
            [FromHeader(Name = "Authorization")] string token,
            List<Guid> fundIds
        )
        {
            var userId = JwtManager.GetUserId(token);
            var user = await userRepository.GetAsync(userId);
            if (user == null)
                return Unauthorized();

            var funds = new List<FundModel>();
            foreach (var fundId in fundIds)
            {
                var fund = await fundRepository.GetAsync(fundId);
                if (fund == null)
                    return BadRequest();

                funds.Add(fund);
            }

            var result = await userRepository.AddFundsAsync(userId, funds);
            return result == null ? NotFound() : Ok(result.ToProfileBody());
        }


        [HttpDelete("funds/{fundId}")]
        [SwaggerOperation("Удалить выбранный фонд")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, Description = "Неверный идентификатор")]

        public async Task<IActionResult> RemoveActivityAsync(
            [FromHeader(Name = "Authorization")] string token,
            Guid fundId)
        {
            var userId = JwtManager.GetUserId(token);
            var user = await userRepository.GetAsync(userId);
            if (user == null)
                return Unauthorized();

            var fund = await fundRepository.GetAsync(fundId);
            if (fund == null)
                return NotFound();

            var result = await userRepository.RemoveFundAsync(userId, fund);
            return NoContent();
        }


        [HttpGet("fund/specializations")]
        [SwaggerOperation("Получить список специализаций фондов")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(IEnumerable<FundSpecializationBody>))]

        public async Task<IActionResult> GetSpecializations()
        {
            var result = await fundSpecializationRepository.GetAllAsync();
            return Ok(result.Select(e => e.ToFundSpecializationBody()));
        }

        [HttpGet("fund/specializations/{specializationId}")]
        [SwaggerOperation("Получить список подгрупп специализации")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(IEnumerable<FundSubgroupBody>))]

        public async Task<IActionResult> GetSubroups(Guid specializationId)
        {
            var result = await fundSubgroupRepository.GetAllBySpecializationidAsync(specializationId);
            return Ok(result.Select(e => e.ToFundSubgroupBody()));
        }
    }
}