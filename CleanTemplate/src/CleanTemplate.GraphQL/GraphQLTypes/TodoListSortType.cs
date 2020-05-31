using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
using HotChocolate.Types.Sorting;

namespace CleanTemplate.GraphQL.GraphQLTypes
{
    public class TodoListSortType : SortInputType<SimplifiedTodoListVm>
    {
        protected override void Configure(ISortInputTypeDescriptor<SimplifiedTodoListVm> descriptor)
        {
            descriptor.BindFieldsExplicitly().Sortable(t => t.DisplayOrder);
            descriptor.BindFieldsExplicitly().Sortable(t => t.Description);
        }
    }
}
