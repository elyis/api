using api.src.Domain.Entities.Request;
using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface IFundSpecializationRepository
    {
        Task<FundSpecializationsModel?> AddAsync(CreateFundSpecializationBody body);
        Task<FundSpecializationsModel?> GetAsync(Guid id);
        Task<FundSpecializationsModel?> GetAsync(string name);
        Task<IEnumerable<FundSpecializationsModel>> GetAllAsync();
    }
}