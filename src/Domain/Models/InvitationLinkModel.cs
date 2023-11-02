namespace api.src.Domain.Models
{
    public class InvitationLinkModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public DateTime ValidBefore { get; set; }
        public bool IsActivated { get; set; }

        public DepartmentModel Department { get; set; }
        public Guid DepartmentId { get; set; }
    }
}