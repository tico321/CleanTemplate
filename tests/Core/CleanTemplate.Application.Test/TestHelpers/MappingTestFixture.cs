using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Mapping;

namespace CleanTemplate.Application.Test.TestHelpers
{
    public class MappingTestFixture
    {
        public MappingTestFixture()
        {
            ConfigurationProvider = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });

            Mapper = ConfigurationProvider.CreateMapper();
        }

        public IConfigurationProvider ConfigurationProvider { get; }
        public IMapper Mapper { get; }
    }
}
