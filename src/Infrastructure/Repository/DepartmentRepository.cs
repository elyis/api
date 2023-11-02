using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext context;

        public DepartmentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<DepartmentModel?> AddAsync(CreateDepartmentBody body, OrganizationModel organization)
        {
            var department = await GetAsync(body.Name, organization.Id);
            if (department != null)
                return null;

            var newDepartment = new DepartmentModel
            {
                Name = body.Name,
                Organization = organization,
            };

            var result = await context.Departments.AddAsync(newDepartment);
            await context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartmentsByOrganization(Guid organizationId)
            => await context.Departments
                .Where(e => e.OrganizationId == organizationId).
                ToListAsync();

        public async Task<DepartmentModel?> GetAsync(string name, Guid organizationId)
            => await context.Departments
                .FirstOrDefaultAsync(e => e.Name == name &&
                                        e.OrganizationId == organizationId);

        public async Task<DepartmentModel?> GetFullAsync(Guid id)
            => await context.Departments
                .Include(e => e.Workers)
                    .ThenInclude(e => e.TrainingResults)
                        .ThenInclude(e => e.Activity)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<DepartmentModel?> GetWithWorkers(Guid id)
            => await context.Departments
                .Include(e => e.Workers)
                .FirstOrDefaultAsync(e => e.Id == id);
    }
}