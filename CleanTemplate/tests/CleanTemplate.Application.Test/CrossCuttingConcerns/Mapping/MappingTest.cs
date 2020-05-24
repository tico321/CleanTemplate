using System;
using System.Linq;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Mapping;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Queries;
using CleanTemplate.Domain.Todos;
using Xunit;

namespace CleanTemplate.Application.Test.CrossCuttingConcerns.Mapping
{
    public class MappingTest : IClassFixture<MappingTestFixture>
    {
        public MappingTest(MappingTestFixture fixture)
        {
            _configuration = fixture.ConfigurationProvider;
            _mapper = fixture.Mapper;
        }

        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        [Theory, InlineData(typeof(TodoItem), typeof(TodoVm))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }

        /// <summary>
        ///     If some type needs to be filtered in this test it's advisable to create a separate test for that type.
        /// </summary>
        [Fact]
        public void AllIMapFromTypes_ShouldSupportMappingFromSourceToDestination()
        {
            // Local function to identify types that implement IMapFrom
            bool implementsIMapFrom(Type i)
            {
                return i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>);
            }

            // We get all the types that Implement IMapFrom
            var allDestinationTypes = typeof(IMapFrom<>).Assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(implementsIMapFrom))
                .ToList();

            foreach (var destinationType in allDestinationTypes)
            {
                // We get all the Generic types from IMapFrom<T>
                var sourceTypes = destinationType.GetInterfaces()
                    .Where(implementsIMapFrom)
                    .Select(i => i.GetGenericArguments()[0]);
                // Foreach target type we test that we can map from source to destination
                foreach (var sourceType in sourceTypes)
                {
                    var sourceInstance = Activator.CreateInstance(sourceType);
                    _mapper.Map(sourceInstance, sourceType, destinationType);
                }
            }
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
    }
}
