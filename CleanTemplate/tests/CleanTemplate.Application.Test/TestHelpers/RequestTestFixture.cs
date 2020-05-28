using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.CrossCuttingConcerns.Mapping;

namespace CleanTemplate.Application.Test.TestHelpers
{
    public class RequestTestFixture
    {
        public RequestTestFixture()
        {
            ConfigurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = ConfigurationProvider.CreateMapper();

            Context = new ApplicationDbContextFactory().Create().Result;
        }

        public IConfigurationProvider ConfigurationProvider { get; }
        public IMapper Mapper { get; }
        public IApplicationDbContext Context { get; }
    }
}
