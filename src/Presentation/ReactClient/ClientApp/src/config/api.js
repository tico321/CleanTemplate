export const todoServiceConfig = {
  baseURL: 'http://localhost:5000',
};

export const identityConfig = {
  authority: 'http://localhost:5002',
  client_id: 'ReactClient',
  redirect_uri: 'http://localhost:3000/callback',
  response_type: 'code',
  scope: 'openid profile email todo_api todo_graphql',
  post_logout_redirect_uri: 'http://localhost:3000/',
  // TODO add silent renew https://github.com/maxmantz/redux-oidc-example/blob/master/src/utils/userManager.js
  // https://github.com/AxaGuilDEv/react-oidc/blob/master/examples/redux/src/configuration.js
  // https://github.com/maxmantz/redux-oidc/blob/master/docs/API.md
  // silent_redirect_uri: true,
};
