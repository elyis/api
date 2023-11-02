using api.src.Domain.Entities.Response;
using Microsoft.EntityFrameworkCore;

namespace api.src.Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class OrganizationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<UserModel> Workers { get; set; } = new();
        public List<InvitationLinkModel> InvitationLinks { get; set; } = new();
        public List<DepartmentModel> Departments { get; set; } = new();

        public OrganizationBody ToOrganizationBody()
        {
            return new OrganizationBody
            {
                Id = Id.ToString(),
                Name = Name
            };
        }
    }
}