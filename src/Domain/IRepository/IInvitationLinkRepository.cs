using api.src.Domain.Models;

namespace api.src.Domain.IRepository
{
    public interface IInvitationLinkRepository
    {
        Task<InvitationLinkModel?> AddAsync(DepartmentModel department);
        Task<InvitationLinkModel?> GetAsync(string invitationLinkValue);
        Task<bool> ActivateLink(string invitationLink);
    }
}