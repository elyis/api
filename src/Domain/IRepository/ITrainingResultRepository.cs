using api.src.Domain.Entities.Request;
using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface ITrainingResultRepository
    {
        Task<TrainingResultModel?> GetAsync(Guid id);
        Task<TrainingResultModel> AddAsync(CreateTrainingResultBody body, ActivityModel activity, UserModel user);
        Task<IEnumerable<TrainingResultModel>> GetByDate(DateOnly date);
        Task<IEnumerable<TrainingResultModel>> GetByDate(DateOnly date, Guid userId);
        Task<IEnumerable<TrainingResultModel>> GetByMonth(int month);
        Task<IEnumerable<TrainingResultModel>> GetFullByMonth(int month, Guid userId);
    }
}