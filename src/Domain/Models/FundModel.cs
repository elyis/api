using api.src.Domain.Entities.Response;
using Microsoft.EntityFrameworkCore;

namespace api.src.Domain.Models
{
    [Index(nameof(Name))]
    public class FundModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }


        //Relations
        public List<UserModel> Benefactors { get; set; } = new();
        public List<FundSubgroupsModel> FundSubgroups { get; set; } = new();

        public FundBody ToFundBody()
        {
            return new FundBody
            {
                Id = Id.ToString(),
                Name = Name,
                SubgroupIds = FundSubgroups.Select(e => e.Id).ToList()
            };
        }
    }
}