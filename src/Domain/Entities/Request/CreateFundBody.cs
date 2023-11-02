namespace api.src.Domain.Entities.Request
{
    public class CreateFundBody
    {
        public string Name { get; set; }
        public List<Guid> FundSubgroupIds { get; set; } = new();
    }
}