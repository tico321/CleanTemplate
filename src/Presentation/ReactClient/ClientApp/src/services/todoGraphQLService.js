import ApolloClient, { gql, InMemoryCache } from 'apollo-boost';
import { todoServiceConfig } from '../config/api';

let client = {};

const getQuery = {
  query: gql`
    {
      todoLists(first: 10) {
        nodes {
          id
          description
          displayOrder
          count
        }
      }
    }
  `,
  fetchPolicy: 'network-only',
};

const getByIdQuery = (id) => ({
  query: gql`
    {
      todoListById(id: ${id}) {
        id
        userId
        description
        displayOrder
        todos {
          id
          description
          displayOrder
          state
        }
      }
    }
  `,
  fetchPolicy: 'network-only',
});

const getCreateMutation = (description) => ({
  mutation: gql`
    mutation {
      createTodoList(command: {
        description: "${description}"
      })
    }
  `,
});

const getDeleteMutation = (id) => ({
  mutation: gql`
    mutation {
      deleteTodoList(id: ${id})
    }
  `,
});

const getUpdateMutation = (updatedList) => ({
  mutation: gql`
    mutation {
      updateTodoList(command: {
        id: ${updatedList.id}
        description: "${updatedList.description}"
        displayOrder: ${updatedList.displayOrder}
      })
    }
  `,
});

const getCreatTodoItem = (item) => ({
  mutation: gql`
    mutation {
      createTodoItem(command: {
        description: "${item.description}"
        todoListId: ${item.todoListId}
      })
    }
  `,
});

const getUpdateTodoItem = (item) => ({
  mutation: gql`
    mutation {
      updateTodoItem(command: {
        description: "${item.description}"
        displayOrder: ${item.displayOrder}
        id: ${item.id}
        itemId: ${item.itemId}
        state: "${item.state}"
      })
    }
  `,
});

const getDeleteTodoItem = (id, itemId) => ({
  mutation: gql`
    mutation {
      deleteTodoItem(id: ${id}, itemId: ${itemId})
    }
  `,
});

// ideally we would use useQuery from @apollo/react-hooks, but in order to keep compatibility with
// the rest service we implement this service.
const service = {
  get: () => client.query(getQuery).then((response) => response.data.todoLists.nodes),
  create: (description) => client.mutate(getCreateMutation(description)),
  getById: (id) => client.query(getByIdQuery(id)).then((response) => response.data.todoListById),
  deleteById: (id) => client.mutate(getDeleteMutation(id)),
  update: (todoList) => client.mutate(getUpdateMutation(todoList)),
  addItem: (id, item) => client.mutate(getCreatTodoItem(item)),
  updateItem: (id, item) => client.mutate(getUpdateTodoItem(item)),
  deleteItem: (id, itemId) => client.mutate(getDeleteTodoItem(id, itemId)),
  setToken: (token) => {
    const cache = new InMemoryCache();
    client = new ApolloClient({
      cache,
      uri: todoServiceConfig.baseURL,
      request: (operation) => {
        operation.setContext({
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
      },
    });
  },
};

export default service;
