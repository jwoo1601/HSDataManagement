import { inject, injectable } from "inversify";
import { User, UserManager, WebStorageStateStore } from "oidc-client";
import { ApiScopes, OidcApiScopes } from "@/models/api/ApiConstants";
import ServiceTypes from "@/services/serviceTypes";
import IConfiguration from "./configuration";
import "reflect-metadata";

export default interface IOidcService {
  signInAsync(): Promise<void>;
  signOutAsync(): Promise<void>;
  getUserAsync(): Promise<User | null>;
  getAccessTokenAsync(): Promise<string | null>;
}

@injectable()
export class DefaultOidcService implements IOidcService {
  private userManager: UserManager;

  constructor(
    @inject(ServiceTypes.Configuration) configuration: IConfiguration
  ) {
    this.userManager = new UserManager({
      userStore: new WebStorageStateStore({ store: window.localStorage }),
      authority:
        configuration.getValue("hsm.baseUrl") || "http://localhost:5000",
      client_id: configuration.getValue("hsm.client.id") || "hsm-interactive",
      automaticSilentRenew: true,
      response_type: "id_token token",
      scope: ApiScopes.from(OidcApiScopes.OpenId, OidcApiScopes.Profile),
      filterProtocolClaims: true,
    });
  }

  async signInAsync(): Promise<void> {
    await this.userManager.signinRedirect({});
  }

  async signOutAsync(): Promise<void> {
    await this.userManager.signoutRedirect();
  }

  async getUserAsync(): Promise<User | null> {
    return await this.userManager.getUser();
  }

  async getAccessTokenAsync(): Promise<string | null> {
    const user = await this.getUserAsync();
    if (user) {
      return user.access_token;
    }

    return null;
  }
}
