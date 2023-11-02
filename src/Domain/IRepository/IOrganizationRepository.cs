using api.src.Domain.Entities.Request;
using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface IOrganizationRepository
    {
        Task<OrganizationModel?> GetAsync(Guid id);
        Task<OrganizationModel?> AddAsync(CreateOrganizationBody body);
        Task<OrganizationModel?> GetAsync(string organizationName);
        Task<IEnumerable<OrganizationModel>> GetAllAsync();
    }
}