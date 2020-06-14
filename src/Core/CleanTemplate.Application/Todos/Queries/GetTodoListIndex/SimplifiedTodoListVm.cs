using System.Linq;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Mapping;
using CleanTemplate.Application.Todos.Model;

namespace CleanTemplate.Application.Todos.Queries.GetTodoListIndex
{
    public class SimplifiedTodoListVm : IMapFrom<TodoList>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int Count { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TodoList, SimplifiedTodoListVm>()
                .ForMember(d => d.Count, opt => opt.MapFrom(t => t.Todos.Count()));
        }
    }
}
