import store from "@/store";
import {
  Action,
  getModule,
  Module,
  Mutation,
  VuexModule,
} from "vuex-module-decorators";
import HSMUserInfo from "@/models/api/UserInfo";
import { UserCredentials } from "@/models/api/Account";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import IOAuthService from "@/services/oauthService";
import {
  OAuthRefreshTokenResponse,
  OAuthTokenErrorResponse,
  OAuthTokenResponse,
} from "@/models/api/OAuthToken";
import IQueryService from "@/services/queryService";
import secureStorage from "@/store/secureStorage";

export interface AccountState {
  authenticated: boolean;
  userInfo: HSMUserInfo | null;
  accessToken: string | null;
  refreshToken: string | null;
}

export interface AccountActionResult {
  success: boolean;
  errorMessage?: string;
  payload?: string;
}

@Module({
  dynamic: true,
  store,
  name: "account",
  preserveState: secureStorage.getAllKeys().includes("hsm_store_cache"),
})
class Account extends VuexModule implements AccountState {
  public authenticated = false;
  public userInfo: HSMUserInfo | null = null;
  public accessToken: string | null = null;
  public refreshToken: string | null = null;

  @$inject()
  private oAuthService!: IOAuthService;
  @$inject()
  private queryService!: IQueryService;

  @Mutation
  private SET_AUTHENTICATED(authenticated: boolean) {
    this.authenticated = authenticated;
  }

  @Mutation
  private SET_USERINFO(userInfo: HSMUserInfo | null) {
    this.userInfo = userInfo;
  }

  @Mutation
  private SET_ACCESS_TOKEN(token: string | null) {
    this.accessToken = token;
  }

  @Mutation
  private SET_REFRESH_TOKEN(token: string | null) {
    this.refreshToken = token;
  }

  @Action
  public async loginAsync(
    credentials: UserCredentials
  ): Promise<AccountActionResult> {
    const result: AccountActionResult = { success: false };
    if (!credentials.username || !credentials.password) {
      return result;
    }

    const tokenResult = await this.oAuthService.requestAccessTokenAsync({
      grant_type: "password",
      client_id: "hsm-interactive",
      username: credentials.username,
      password: credentials.password,
      scope: "openid profile offline_access HSMApi",
    });

    if (tokenResult.success) {
      const successResponse = tokenResult.response as OAuthTokenResponse;
      this.SET_ACCESS_TOKEN(successResponse.access_token);
      this.SET_REFRESH_TOKEN(successResponse.refresh_token);

      const userInfo = await this.getUserInfoAsync();
      if (userInfo) {
        this.SET_USERINFO(userInfo);
        this.SET_AUTHENTICATED(true);

        result.success = true;
      } else {
        result.errorMessage = "error.userinfoUnavailable";

        this.logout();
      }
    } else {
      const errorResponse = tokenResult.response as OAuthTokenErrorResponse;
      if (errorResponse.error === "invalid_grant") {
        result.errorMessage = "error.incorrectCredentials";
      } else {
        result.errorMessage = "error.loginUnknown";
        result.payload = errorResponse.error;
      }

      this.logout();
    }

    return result;
  }

  @Action
  public async extendLoginSessionAsync(): Promise<AccountActionResult> {
    if (this.refreshToken) {
      const tokenResult = await this.oAuthService.requestRefreshTokenAsync({
        grant_type: "refresh_token",
        client_id: "hsm-interactive",
        refresh_token: this.refreshToken,
      });

      if (tokenResult.success) {
        const successResponse = tokenResult.response as OAuthRefreshTokenResponse;

        this.SET_ACCESS_TOKEN(successResponse.access_token);
        this.SET_REFRESH_TOKEN(successResponse.refresh_token);

        return {
          success: true,
        };
      }
    }

    this.logout();

    return {
      success: false,
    };
  }

  @Action
  public async getUserInfoAsync(): Promise<HSMUserInfo | null> {
    return await this.queryService.getSingleOrDefaultAsync<HSMUserInfo>(
      "/connect/userinfo"
    );
  }

  @Action
  public logout() {
    this.SET_AUTHENTICATED(false);
    this.SET_ACCESS_TOKEN(null);
    this.SET_REFRESH_TOKEN(null);
    this.SET_USERINFO(null);
  }

  @Action
  public clearUserInfo() {
    this.SET_USERINFO(null);
  }
}

export default getModule(Account);
