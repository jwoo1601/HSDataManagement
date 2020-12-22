import { injectable } from "inversify";
import "reflect-metadata";
import {
  OAuthRefreshTokenRequest,
  OAuthRefreshTokenResponse,
  OAuthRSOPGrantRequest,
  OAuthTokenErrorResponse,
  OAuthTokenResponse,
} from "@/models/api/OAuthToken";
import httpClient from "./httpClient";
import { AxiosRequestConfig, AxiosResponse } from "axios";
import queryString from "query-string";
import ApiServiceUtils from "./apiServiceUtils";

export interface TokenResult<TSuccessResponse> {
  success: boolean;
  response: TSuccessResponse | OAuthTokenErrorResponse;
}

export default interface IOAuthService {
  requestAccessTokenAsync(
    request: OAuthRSOPGrantRequest
  ): Promise<TokenResult<OAuthTokenResponse>>;
  requestRefreshTokenAsync(
    request: OAuthRefreshTokenRequest
  ): Promise<TokenResult<OAuthRefreshTokenResponse>>;
}

@injectable()
export class DefaultOAuthService implements IOAuthService {
  async postFormAsync<TResponse>(
    url: string,
    data: any,
    config?: AxiosRequestConfig | undefined
  ): Promise<AxiosResponse<TResponse>> {
    return await httpClient.post<TResponse>(url, queryString.stringify(data), {
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
      ...config,
    });
  }

  async requestAccessTokenAsync(
    request: OAuthRSOPGrantRequest
  ): Promise<TokenResult<OAuthTokenResponse>> {
    const response = await this.postFormAsync<OAuthTokenResponse>(
      "/connect/token",
      request
    );

    if (ApiServiceUtils.isSuccessResponse(response.status)) {
      return {
        success: true,
        response: response.data,
      };
    }

    return {
      success: false,
      response: response.data,
    };
  }

  async requestRefreshTokenAsync(
    request: OAuthRefreshTokenRequest
  ): Promise<TokenResult<OAuthRefreshTokenResponse>> {
    const response = await this.postFormAsync<OAuthRefreshTokenResponse>(
      "/connect/token",
      request
    );

    if (ApiServiceUtils.isSuccessResponse(response.status)) {
      return {
        success: true,
        response: response.data,
      };
    }

    return {
      success: false,
      response: response.data,
    };
  }
}
