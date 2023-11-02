using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{
    public class FundSubgroupRepository : IFundSubgroupRepository
    {
        private readonly AppDbContext context;

        public FundSubgroupRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<FundSubgroupsModel?> AddAsync(CreateFundSubgroupBody body, FundSpecializationsModel fundSpecialization)
        {
            var oldSubgroup = await GetAsync(body.Name, fundSpecialization.Id);
            if (oldSubgroup != null)
                return null;

            var fundSubgroup = new FundSubgroupsModel
            {
                Name = body.Name,
                FundSpecialization = fundSpecialization
            };

            var result = await context.FundSubgroups.AddAsync(fundSubgroup);
            await context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<FundSubgroupsModel>> GetAllBySpecializationidAsync(Guid specializationId)
            => await context.FundSubgroups
                .Where(e => e.FundSpecializationId == specializationId).ToListAsync();

        public async Task<FundSubgroupsModel?> GetAsync(string name, Guid specializationId)
            => await context.FundSubgroups
                .FirstOrDefaultAsync(e => e.Name == name && e.FundSpecializationId == specializationId);

        public async Task<FundSubgroupsModel?> GetAsync(Guid id)
            => await context.FundSubgroups.FindAsync(id);

        public async Task<FundSubgroupsModel?> GetWithFundsAsync(Guid subgroupId)
            => await context.FundSubgroups
                .Include(e => e.Funds)
                .FirstOrDefaultAsync(e => e.Id == subgroupId);
    }
}