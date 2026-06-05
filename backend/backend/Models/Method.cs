namespace backend.Models
{
    public class Method
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string LuaCode { get; set; } = null!;
    }
}
