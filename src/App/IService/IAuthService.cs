using api.src.Domain.Entities.Request;
using api.src.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.src.App.IService
{
    public interface IAuthService
    {
        Task<IActionResult> SignUp(SignUpBody body, string rolename, DepartmentModel organization);
        Task<IActionResult> SignIn(SignInBody body);
        Task<IActionResult> RestoreToken(string refreshToken);
    }
}