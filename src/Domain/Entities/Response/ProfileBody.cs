using api.src.Domain.Enums;

namespace api.src.Domain.Entities.Response
{
    public class ProfileBody
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string OrganizationName { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public UserRole Role { get; set; }
        public string? UrlIcon { get; set; }
    }
}