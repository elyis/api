using api.src.Domain.Entities.Request;
using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface IFundRepository
    {
        Task<FundModel?> AddAsync(CreateFundBody body, List<FundSubgroupsModel> fundSpecializations);
        Task<FundModel?> GetAsync(Guid id);
        Task<FundModel?> GetAsync(string name);
        Task<IEnumerable<FundModel>> GetAllAsync();
    }
}