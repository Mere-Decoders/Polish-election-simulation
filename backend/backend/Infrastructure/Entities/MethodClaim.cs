namespace backend.Infrastructure.Entities;
public class MethodClaim
{
    public Guid UserId { get; set; }
    public string Label { get; set; }
    public Guid MethodId { get; set; }
}

