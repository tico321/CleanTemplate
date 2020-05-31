using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Mapping;
using CleanTemplate.Application.Todos.Model;

namespace CleanTemplate.Application.Todos.Queries.GetTodoListById
{
    public class TodoListVm : IMapFrom<TodoList>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int Count { get; set; }
        public List<TodoItemVm> Todos { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TodoList, TodoListVm>()
                .ForMember(d => d.Count, opt => opt.MapFrom(s => s.Todos.Count()));
        }
    }
}
