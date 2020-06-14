using CleanTemplate.Application.CrossCuttingConcerns;

namespace CleanTemplate.Infrastructure.CrossCuttingConcerns
{
    public class NullCurrentUserService : ICurrentUserService
    {
        public string UserId { get; } = string.Empty;
        public string UserName { get; } = string.Empty;
    }
}
