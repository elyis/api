using api.src.Domain.Entities.Response;
using Microsoft.EntityFrameworkCore;

namespace api.src.Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ActivityModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float PaymentRation { get; set; } = 0.65f;

        //Relations
        public List<UserModel> Users { get; set; } = new();
        public List<TrainingResultModel> TrainingResults { get; set; } = new();

        public ActivityBody ToActivityBody()
        {
            return new ActivityBody
            {
                Id = Id.ToString(),
                Name = Name,
            };
        }

        public ActivityRatioBody ToActivityRatioBody()
        {
            return new ActivityRatioBody
            {
                Id = Id.ToString(),
                Name = Name,
                PaymentRation = PaymentRation
            };
        }
    }
}