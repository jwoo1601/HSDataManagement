export interface OAuthTokenRequest {
  grant_type: string;
  client_id: string;
  client_secret?: string;
  scope?: string;
}

export interface OAuthRSOPGrantRequest extends OAuthTokenRequest {
  grant_type: "password";
  username: string;
  password: string;
}

export interface OAuthRefreshTokenRequest {
  grant_type: "refresh_token";
  client_id: string;
  client_secret?: string;
  refresh_token: string;
}

export interface OAuthTokenResponse {
  access_token: string;
  refresh_token: string;
  scope: string;
  token_type: string;
  expires_in: number;
}

export interface OAuthTokenErrorResponse {
  error: string;
  error_description?: string;
}

export interface OAuthRefreshTokenResponse extends OAuthTokenResponse {
  id_token: string;
}

export interface OAuthData {
  access_token: string;
  refresh_token?: string;
}
