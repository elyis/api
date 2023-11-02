using api.src.Domain.Entities.Request;
using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface IDepartmentRepository
    {
        Task<DepartmentModel?> AddAsync(CreateDepartmentBody body, OrganizationModel organization);
        Task<DepartmentModel?> GetAsync(string name, Guid organizationId);
        Task<DepartmentModel?> GetFullAsync(Guid id);
        Task<DepartmentModel?> GetWithWorkers(Guid id);
        Task<IEnumerable<DepartmentModel>> GetAllDepartmentsByOrganization(Guid organizationId);
    }
}