using api.src.Domain.Entities.Response;
using Microsoft.EntityFrameworkCore;

namespace api.src.Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class FundSpecializationsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //Relations
        //Subgroups of specialization
        public List<FundSubgroupsModel> SpecializationSubgroups { get; set; } = new();

        public FundSpecializationBody ToFundSpecializationBody()
        {
            return new FundSpecializationBody
            {
                Id = Id.ToString(),
                Name = Name
            };
        }
    }
}