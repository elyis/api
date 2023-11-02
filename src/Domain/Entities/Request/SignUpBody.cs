using System.ComponentModel.DataAnnotations;

namespace api.src.Domain.Entities.Request
{
    public class SignUpBody
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string OrganizationCode { get; set; }
    }
}