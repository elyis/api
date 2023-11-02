using System.ComponentModel.DataAnnotations;
using api.src.Domain.Entities.Response;
using api.src.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace api.src.Domain.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Token), IsUnique = true)]
    public class UserModel
    {
        public Guid Id { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string PasswordHash { get; set; }
        public string? RestoreCode { get; set; }
        public string RoleName { get; set; }
        public float Weight { get; set; } = 0.0f;
        public float Height { get; set; } = 0.0f;
        public DateTime? RestoreCodeValidBefore { get; set; }
        public bool WasPasswordResetRequest { get; set; }
        public string? Token { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        //Relations
        public OrganizationModel? Organization { get; set; }
        public Guid? OrganizationId { get; set; }

        public DepartmentModel Department { get; set; }
        public Guid DepartmentId { get; set; }

        //selected funds for donations
        public List<FundModel> EndowmentFunds { get; set; } = new();

        //selected activities
        public List<ActivityModel> SelectedActivities { get; set; } = new();
        public List<TrainingResultModel> TrainingResults { get; set; } = new();

        public ProfileBody ToProfileBody()
        {

            var fullname = FirstName;
            if (!string.IsNullOrEmpty(LastName))
                fullname += " " + LastName;
            if (!string.IsNullOrEmpty(Patronymic))
                fullname += " " + Patronymic;

            return new ProfileBody
            {
                Email = Email,
                Fullname = fullname,
                Role = Enum.Parse<UserRole>(RoleName),
                UrlIcon = string.IsNullOrEmpty(Image) ? null : $"{Constants.webPathToProfileIcons}{Image}",
                Weight = Weight,
                Height = Height,
                OrganizationName = Department.Organization.Name,
            };
        }
    }
}