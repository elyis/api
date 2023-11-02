using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{
    public class FundRepository : IFundRepository
    {
        private readonly AppDbContext context;

        public FundRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<FundModel?> AddAsync(CreateFundBody body, List<FundSubgroupsModel> fundSubgroups)
        {
            var fund = await GetAsync(body.Name);
            if (fund != null)
                return null;

            var newFund = new FundModel
            {
                Name = body.Name,
                FundSubgroups = fundSubgroups
            };

            var result = await context.Funds.AddAsync(newFund);
            await context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<FundModel>> GetAllAsync()
            => await context.Funds
                .Include(e => e.FundSubgroups)
                .ToListAsync();

        public async Task<FundModel?> GetAsync(Guid id)
            => await context.Funds
                .Include(e => e.FundSubgroups)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<FundModel?> GetAsync(string name)
            => await context.Funds.FirstOrDefaultAsync(e => e.Name == name);
    }
}