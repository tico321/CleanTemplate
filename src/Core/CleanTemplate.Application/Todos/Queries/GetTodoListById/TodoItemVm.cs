﻿using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Mapping;
using CleanTemplate.Application.Todos.Model;

namespace CleanTemplate.Application.Todos.Queries.GetTodoListById
{
    public class TodoItemVm : IMapFrom<TodoItem>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string State { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TodoItem, TodoItemVm>()
                .ForMember(d => d.State, opt => opt.MapFrom(s => s.State.Name));
        }
    }
}
