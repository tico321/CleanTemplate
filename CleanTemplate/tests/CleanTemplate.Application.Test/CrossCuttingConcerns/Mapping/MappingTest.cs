using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Mapping;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.UpdateTodoItem;
using CleanTemplate.Application.Todos.Commands.UpdateTodoList;
using CleanTemplate.Application.Todos.Model;
using CleanTemplate.Application.Todos.Queries.GetTodoListById;
using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
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

        /// <summary>
        ///     If some type needs to be filtered in this test it's advisable to create a separate test for that type.
        /// </summary>
        [Fact]
        public void AllIMapFromTypes_ShouldSupportMappingFromSourceToDestination()
        {
            // Ignored types have separate unit tests
            var ignoredTypes = new HashSet<Type>
            {
                typeof(SimplifiedTodoListVm), typeof(TodoListVm), typeof(TodoItemVm), typeof(TodoListMappingProfile)
            };

            // Local function to identify types that implement IMapFrom
            bool implementsIMapFrom(Type i)
            {
                return i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>);
            }

            // We get all the types that Implement IMapFrom
            var allDestinationTypes = typeof(IMapFrom<>).Assembly.GetExportedTypes()
                .Where(t => !ignoredTypes.Contains(t))
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

        [Fact]
        public void ShouldSupportMappingFromSourceToDestination()
        {
            var tests = new (object, Type, Type)[]
            {
                (new TodoList("userId", "description", displayOrder: 0),
                    typeof(TodoList),
                    typeof(SimplifiedTodoListVm)),
                (new TodoItem("desc", displayOrder: 1),
                    typeof(TodoItem),
                    typeof(TodoItemVm)),
                (new TodoList("userId", "description", displayOrder: 0).SequenceAddTodo("desc"),
                    typeof(TodoList),
                    typeof(TodoListVm))
            };

            foreach (var (source, sourceType, destinationType) in tests)
            {
                _mapper.Map(source, sourceType, destinationType);
            }
        }

        [Fact]
        public void ShouldSupportMappingFromSourceToDestination_ForTypesWithConstructors()
        {
            var tests = new (object, object)[]
            {
                (new UpdateTodoListCommand(), new TodoList("userId", "desc", displayOrder: 1)),
                (new UpdateTodoItemCommand(), new TodoItem("des", displayOrder: 1))
            };
            foreach (var (source, destination) in tests)
            {
                _mapper.Map(source, destination);
            }
        }
    }
}
