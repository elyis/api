using api.src.Domain.IRepository;
using api.src.Domain.Models;
using api.src.Infrastructure.Data;
using api.src.Utility;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Repository
{
    public class InvitationLinkRepository : IInvitationLinkRepository
    {
        private readonly AppDbContext context;

        public InvitationLinkRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> ActivateLink(string invitationLink)
        {
            var invitationLinkModel = await GetAsync(invitationLink);
            if (invitationLinkModel == null || invitationLinkModel.ValidBefore < DateTime.UtcNow)
                return false;

            invitationLinkModel.IsActivated = true;
            await context.SaveChangesAsync();
            return true;
        }



        public async Task<InvitationLinkModel?> AddAsync(DepartmentModel department)
        {
            var token = InvitationLinkGenerator.Generate(department.Id);
            var newInvitationLink = new InvitationLinkModel
            {
                Value = token,
                Department = department,
                ValidBefore = DateTime.UtcNow.AddDays(1),
            };

            var result = await context.InvitationLinks.AddAsync(newInvitationLink);
            await context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<InvitationLinkModel?> GetAsync(string invitationLinkValue)
            => await context.InvitationLinks
                .Include(e => e.Department)
                    .ThenInclude(e => e.Organization)
                .FirstOrDefaultAsync(e => e.Value == invitationLinkValue && !e.IsActivated);
    }
}