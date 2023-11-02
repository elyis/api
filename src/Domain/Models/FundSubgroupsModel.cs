using api.src.Domain.Entities.Response;

namespace api.src.Domain.Models
{
    public class FundSubgroupsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //Relations
        public FundSpecializationsModel FundSpecialization { get; set; }
        public Guid FundSpecializationId { get; set; }
        public List<FundModel> Funds { get; set; }

        public FundSubgroupBody ToFundSubgroupBody()
        {
            return new FundSubgroupBody
            {
                Id = Id.ToString(),
                Name = Name
            };
        }
    }
}