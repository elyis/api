using api.src.Domain.Entities.Request;
using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel?> AddAsync(SignUpBody body, string role, DepartmentModel organization);
        Task<UserModel?> GetAsync(Guid id);
        Task<UserModel?> GetAsync(string email);
        Task<UserModel?> GetWithFundsAsync(Guid id);
        Task<UserModel?> GetWithDepartmentAsync(Guid id);
        Task<UserModel?> GetWithDepartmentWorkersAsync(Guid id);
        Task<bool> UpdateTokenAsync(string newRefreshToken, Guid id);
        Task<UserModel?> GetByTokenAsync(string refreshTokenHash);
        Task<UserModel?> UpdateProfileIconAsync(Guid userId, string filename);
        Task<UserModel?> UpdateWeightAndHeight(Guid id, float weight, float height);
        Task<UserModel?> AddFundsAsync(Guid id, List<FundModel> funds);
        Task<bool> RemoveFundAsync(Guid id, FundModel fund);

        Task<UserModel?> AddActivitiesAsync(Guid id, List<ActivityModel> activities);
        Task<bool> RemoveActivityAsync(Guid id, ActivityModel activity);
    }
}