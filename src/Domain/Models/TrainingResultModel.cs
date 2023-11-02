using api.src.Domain.Entities.Response;

namespace api.src.Domain.Models
{
    public class TrainingResultModel
    {
        public Guid Id { get; set; }
        public int Count { get; set; }
        public int DurationSeconds { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        public UserModel User { get; set; }
        public Guid UserId { get; set; }
        public ActivityModel Activity { get; set; }
        public Guid ActivityId { get; set; }

        public ActivityEfficiencyBody ToActivityEfficiencyCoefficient()
        {
            return new ActivityEfficiencyBody
            {
                ActivityName = Activity.Name,
                EfficiencyRatio = (float)Count / (float)DurationSeconds
            };
        }
    }
}