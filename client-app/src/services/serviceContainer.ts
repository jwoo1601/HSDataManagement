import { Container } from "inversify";
import "reflect-metadata";
import ServiceTypes from "@/services/serviceTypes";
import IOidcService, { DefaultOidcService } from "@/services/oidcService";
import ICustomerService, {
  DefaultCustomerService,
} from "@/services/customerService";
import IConfiguration, { DefaultConfigurationProvider } from "./configuration";
import IOAuthService, { DefaultOAuthService } from "./oauthService";
import IQueryService, { AxiosQueryService } from "./queryService";
import IServiceService, { DefaultServiceService } from "./serviceService";
import IFoodService, { DefaultFoodService } from "./foodService";
import IAccountService, { DefaultAccountService } from "./accountService";
import IUserService, { DefaultUserService } from "./userService";
import IRoleService, { DefaultRoleService } from "./roleService";

const container = new Container({
  defaultScope: "Transient",
});

container
  .bind<IConfiguration>(ServiceTypes.Configuration)
  .to(DefaultConfigurationProvider)
  .inSingletonScope();
container.bind<IQueryService>(ServiceTypes.QueryService).to(AxiosQueryService);
container
  .bind<IOidcService>(ServiceTypes.OidcService)
  .to(DefaultOidcService)
  .inSingletonScope();
container
  .bind<IOAuthService>(ServiceTypes.OAuthService)
  .to(DefaultOAuthService);
container
  .bind<ICustomerService>(ServiceTypes.CustomerService)
  .to(DefaultCustomerService);
container
  .bind<IServiceService>(ServiceTypes.ServiceService)
  .to(DefaultServiceService);
container.bind<IFoodService>(ServiceTypes.FoodService).to(DefaultFoodService);
container
  .bind<IAccountService>(ServiceTypes.AccountService)
  .to(DefaultAccountService);
container.bind<IUserService>(ServiceTypes.UserService).to(DefaultUserService);
container.bind<IRoleService>(ServiceTypes.RoleService).to(DefaultRoleService);

export default container;
