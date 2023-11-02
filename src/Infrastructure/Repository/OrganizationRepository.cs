using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDbContext context;

        public OrganizationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<OrganizationModel?> AddAsync(CreateOrganizationBody body)
        {
            var oldOrganization = await GetAsync(body.Name);
            if (oldOrganization != null)
                return null;

            var newOrganization = new OrganizationModel
            {
                Name = body.Name,
            };

            var result = await context.Organizations.AddAsync(newOrganization);
            await context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<OrganizationModel>> GetAllAsync()
            => await context.Organizations
                .ToListAsync();

        public async Task<OrganizationModel?> GetAsync(Guid id)
            => await context.Organizations.FindAsync(id);

        public async Task<OrganizationModel?> GetAsync(string organizationName)
            => await context.Organizations
                .FirstOrDefaultAsync(e => e.Name.ToLower() == organizationName.ToLower());
    }
}