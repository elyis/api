namespace api.src.Domain.Entities.Request
{
    public class CreateOrganizationBody
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}