using api.src.Domain.Entities.Request;
using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface IFundSubgroupRepository
    {
        Task<FundSubgroupsModel?> AddAsync(CreateFundSubgroupBody body, FundSpecializationsModel fundSpecialization);
        Task<FundSubgroupsModel?> GetAsync(string name, Guid specializationId);
        Task<FundSubgroupsModel?> GetAsync(Guid id);
        Task<IEnumerable<FundSubgroupsModel>> GetAllBySpecializationidAsync(Guid specializationId);
        Task<FundSubgroupsModel?> GetWithFundsAsync(Guid subgroupId);
    }
}