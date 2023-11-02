using api.src.Domain.Entities.Request;
using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface IActivityRepository
    {
        Task<ActivityModel?> AddAsync(CreateActivityBody body);
        Task<IEnumerable<ActivityModel>> GetAllAsync();
        Task<ActivityModel?> GetAsync(string name);
        Task<ActivityModel?> GetAsync(Guid id);
        Task<ActivityModel?> UpdatePaymentRatio(Guid id, float paymentRatio);
    }
}