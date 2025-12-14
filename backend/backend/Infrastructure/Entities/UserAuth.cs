namespace backend.Infrastructure.Entities
{
    public class UserAuth
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}
