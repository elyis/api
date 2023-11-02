namespace api.src.Domain.Entities.Response
{
    public class FundBody
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Guid> SubgroupIds { get; set; } = new();
    }
}