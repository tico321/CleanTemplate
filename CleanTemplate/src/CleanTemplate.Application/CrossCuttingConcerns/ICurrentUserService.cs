namespace CleanTemplate.Application.CrossCuttingConcerns
{
    public interface ICurrentUserService
    {
        public string UserId { get; }
        public string UserName { get; }
    }
}