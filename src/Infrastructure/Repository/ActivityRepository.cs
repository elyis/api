using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext context;

        public ActivityRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ActivityModel?> AddAsync(CreateActivityBody body)
        {
            var activity = await GetAsync(body.Name);
            if (activity != null)
                return null;

            var newActivity = new ActivityModel
            {
                Name = body.Name
            };

            var result = await context.Activities.AddAsync(newActivity);
            await context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<ActivityModel>> GetAllAsync()
            => await context.Activities.ToListAsync();

        public async Task<ActivityModel?> GetAsync(string name)
            => await context.Activities
                .FirstOrDefaultAsync(e => e.Name == name);

        public async Task<ActivityModel?> GetAsync(Guid id)
            => await context.Activities.FindAsync(id);

        public async Task<ActivityModel?> UpdatePaymentRatio(Guid id, float paymentRatio)
        {
            var activity = await GetAsync(id);
            if (activity == null || paymentRatio <= 0)
                return null;

            activity.PaymentRation = paymentRatio;
            await context.SaveChangesAsync();
            return activity;
        }
    }
}