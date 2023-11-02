using api.src.Domain.Entities.Request;
using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using api.src.Utility;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<UserModel?> AddAsync(SignUpBody body, string role, DepartmentModel department)
        {
            var oldUser = await GetAsync(body.Email);
            if (oldUser != null)
                return null;

            var nameParts = body.Fullname.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var newUser = new UserModel
            {
                Email = body.Email,
                FirstName = nameParts.First(),
                LastName = nameParts.Length >= 2 ? nameParts[1] : null,
                Patronymic = nameParts.Length == 3 ? nameParts[2] : null,
                Organization = department.Organization,
                Department = department,
                PasswordHash = Hmac512Provider.Compute(body.PasswordHash),
                RoleName = role,
            };

            var result = await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<UserModel?> GetAsync(Guid id)
            => await context.Users
                .Include(e => e.Organization)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<UserModel?> GetAsync(string email)
            => await context.Users
                .Include(e => e.Organization)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Email == email);

        public async Task<UserModel?> GetByTokenAsync(string refreshTokenHash)
            => await context.Users
            .Include(e => e.Department)
            .Include(e => e.Organization)
            .FirstOrDefaultAsync(e => e.Token == refreshTokenHash);

        public async Task<UserModel?> GetWithFundsAsync(Guid id)
            => await context.Users
            .Include(e => e.EndowmentFunds)
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<UserModel?> GetWithDepartmentAsync(Guid id)
            => await context.Users
                .Include(e => e.Department)
                .Include(e => e.Organization)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<UserModel?> GetWithDepartmentWorkersAsync(Guid id)
            => await context.Users
                .Include(e => e.Department)
                    .ThenInclude(e => e.Workers)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<UserModel?> AddFundsAsync(Guid id, List<FundModel> funds)
        {
            var user = await GetAsync(id);
            if (user == null)
                return null;

            user.EndowmentFunds.AddRange(funds);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> RemoveFundAsync(Guid id, FundModel fund)
        {
            var user = await GetAsync(id);
            if (user == null)
                return false;

            var result = user.EndowmentFunds.Remove(fund);
            await context.SaveChangesAsync();
            return result;
        }

        public async Task<UserModel?> UpdateProfileIconAsync(Guid userId, string filename)
        {
            var user = await GetAsync(userId);
            if (user == null)
                return null;

            user.Image = filename;
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateTokenAsync(string newRefreshToken, Guid id)
        {
            var user = await GetAsync(id);
            if (user == null)
                return false;

            user.Token = newRefreshToken;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<UserModel?> UpdateWeightAndHeight(Guid id, float weight, float height)
        {
            var user = await GetAsync(id);
            if (user == null || weight <= 0 || height <= 0)
                return null;

            user.Weight = weight;
            user.Height = height;
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel?> AddActivitiesAsync(Guid id, List<ActivityModel> activities)
        {
            var user = await GetAsync(id);
            if (user == null)
                return null;

            user.SelectedActivities.AddRange(activities);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> RemoveActivityAsync(Guid id, ActivityModel activity)
        {
            var user = await GetAsync(id);
            if (user == null)
                return false;

            var result = user.SelectedActivities.Remove(activity);
            await context.SaveChangesAsync();
            return result;
        }
    }
}