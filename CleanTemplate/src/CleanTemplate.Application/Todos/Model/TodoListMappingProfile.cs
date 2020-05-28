using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Mapping;
using CleanTemplate.Application.Todos.Commands.UpdateTodoItem;
using CleanTemplate.Application.Todos.Commands.UpdateTodoList;
using CleanTemplate.Domain.Common;

namespace CleanTemplate.Application.Todos.Model
{
    public class TodoListMappingProfile : IMapFrom<UpdateTodoListCommand>
    {
        public void Mapping(Profile profile)
        {
            // This mapping is not directly in TodoList because TodoList doesn't have a constructor without parameters.
            profile.CreateMap<UpdateTodoListCommand, TodoList>(MemberList.None)
                .ForMember(d => d.Id, opt => opt.Ignore());

            profile.CreateMap<UpdateTodoItemCommand, TodoItem>(MemberList.None)
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.State, opt => opt.MapFrom(s => Enumeration.FromDisplayName<TodoItemState>(s.State)));
        }
    }
}
