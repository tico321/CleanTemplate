namespace CleanTemplate.Auth.Application.Model
{
    public class Policy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
