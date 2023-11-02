using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{
    public class TrainingResultRepository : ITrainingResultRepository
    {
        private readonly AppDbContext context;

        public TrainingResultRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<TrainingResultModel> AddAsync(CreateTrainingResultBody body, ActivityModel activity, UserModel user)
        {
            var newResult = new TrainingResultModel
            {
                Count = body.Count,
                DurationSeconds = body.DurationSeconds,
                Activity = activity,
                User = user
            };

            var result = await context.TrainingResults.AddAsync(newResult);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<TrainingResultModel?> GetAsync(Guid id)
            => await context.TrainingResults
                .Include(e => e.Activity)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<TrainingResultModel>> GetByDate(DateOnly date)
            => await context.TrainingResults
                .Include(e => e.Activity)
                .Where(e => e.Date == date)
                .ToListAsync();

        public async Task<IEnumerable<TrainingResultModel>> GetByDate(DateOnly date, Guid userId)
            => await context.TrainingResults
                .Include(e => e.Activity)
                .Where(e => e.Date == date && e.UserId == userId)
                .ToListAsync();

        public async Task<IEnumerable<TrainingResultModel>> GetByMonth(int month)
            => await context.TrainingResults
                .Include(e => e.Activity)
                .Where(e => e.Date.Month == month)
                .ToListAsync();

        public async Task<IEnumerable<TrainingResultModel>> GetFullByMonth(int month, Guid userId)
        => await context.TrainingResults
                .Include(e => e.Activity)
                .Include(e => e.User)
                    .ThenInclude(e => e.Organization)
                .Where(e => e.Date.Month == month && e.User.Id == userId)
                .ToListAsync();
    }
}