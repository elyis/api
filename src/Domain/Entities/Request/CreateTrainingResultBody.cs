using api.src.Domain.Enums;

namespace api.src.Domain.Entities.Request
{
    public class CreateTrainingResultBody
    {
        public int Count { get; set; }
        public int DurationSeconds { get; set; }
        public Activity Activity { get; set; }
    }
}