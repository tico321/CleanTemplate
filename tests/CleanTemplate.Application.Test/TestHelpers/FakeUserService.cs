using CleanTemplate.Application.CrossCuttingConcerns;

namespace CleanTemplate.Application.Test.TestHelpers
{
    public class FakeUserService : ICurrentUserService
    {
        public string UserId => "UserId";
        public string UserName => "UserName";
    }
}
