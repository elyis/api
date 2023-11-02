using api.src.App.IService;
using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Utility;
using Microsoft.AspNetCore.Mvc;

namespace api.src.App.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IInvitationLinkRepository invitationLinkRepository;

        public AuthService(
            IUserRepository userRepository,
            IInvitationLinkRepository invitationLinkRepository
        )
        {
            this.userRepository = userRepository;
            this.invitationLinkRepository = invitationLinkRepository;
        }

        public async Task<IActionResult> RestoreToken(string refreshToken)
        {
            var oldUser = await userRepository.GetByTokenAsync(refreshToken);
            if (oldUser == null)
                return new NotFoundResult();

            var tokenPair = JwtManager.GenerateTokenPair(oldUser.Id, oldUser.RoleName);
            await userRepository.UpdateTokenAsync(tokenPair.RefreshToken, oldUser.Id);

            return new OkObjectResult(tokenPair);
        }

        public async Task<IActionResult> SignIn(SignInBody body)
        {
            var oldUser = await userRepository.GetAsync(body.Email);
            if (oldUser == null)
                return new NotFoundResult();

            var inputPasswordHash = Hmac512Provider.Compute(body.PasswordHash);
            if (oldUser.PasswordHash != inputPasswordHash)
                return new BadRequestResult();

            var tokenPair = JwtManager.GenerateTokenPair(oldUser.Id, oldUser.RoleName);
            await userRepository.UpdateTokenAsync(tokenPair.RefreshToken, oldUser.Id);
            return new OkObjectResult(tokenPair);
        }

        public async Task<IActionResult> SignUp(SignUpBody body, string rolename, DepartmentModel department)
        {
            var invocationLink = await invitationLinkRepository.GetAsync(body.OrganizationCode);
            if (invocationLink == null)
                return new BadRequestResult();

            department = invocationLink.Department;
            await invitationLinkRepository.ActivateLink(body.OrganizationCode);

            var oldUser = await userRepository.AddAsync(body, rolename, department);
            if (oldUser == null)
                return new ConflictResult();

            var tokenPair = JwtManager.GenerateTokenPair(oldUser.Id, rolename);
            await userRepository.UpdateTokenAsync(tokenPair.RefreshToken, oldUser.Id);
            return new OkObjectResult(tokenPair);
        }
    }
}