import authStore from './auth.store';

export default authStore;
export {
  signinRedirect, signinCallback, getIdentityUser, signoutRedirect,
} from './actions';
