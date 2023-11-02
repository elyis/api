using api.src.Domain.Entities.Response;

namespace api.src.Domain.Models
{
    public class DepartmentModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<UserModel> Workers { get; set; } = new();
        public OrganizationModel Organization { get; set; }
        public Guid OrganizationId { get; set; }

        public DepartmentBody ToDepartmentBody()
        {
            return new DepartmentBody
            {
                Id = Id.ToString(),
                Name = Name,
            };
        }
    }
}