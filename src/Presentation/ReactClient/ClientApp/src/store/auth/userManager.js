import { Log, UserManager } from 'oidc-client';
import { identityConfig } from '../../config/api';

Log.logger = console;
Log.level = Log.INFO;

// sample usage https://github.com/skoruba/react-oidc-client-js/blob/master/src/src/services/AuthService.ts
const userManager = new UserManager(identityConfig);
export default userManager;
