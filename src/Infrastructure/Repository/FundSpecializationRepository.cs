using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{

    public class FundSpecializationRepository : IFundSpecializationRepository
    {
        private readonly AppDbContext context;

        public FundSpecializationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<FundSpecializationsModel?> AddAsync(CreateFundSpecializationBody body)
        {
            var oldSpecialization = await GetAsync(body.Name);
            if (oldSpecialization != null)
                return null;

            var newSpecialization = new FundSpecializationsModel
            {
                Name = body.Name
            };

            var result = await context.FundSpecializations.AddAsync(newSpecialization);
            await context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<FundSpecializationsModel>> GetAllAsync()
            => await context.FundSpecializations
                .ToListAsync();

        public async Task<FundSpecializationsModel?> GetAsync(Guid id)
            => await context.FundSpecializations
                .FindAsync(id);

        public async Task<FundSpecializationsModel?> GetAsync(string name)
            => await context.FundSpecializations
                .FirstOrDefaultAsync(e =>
                    e.Name == name);
    }
}