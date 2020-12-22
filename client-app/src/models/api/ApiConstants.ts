export enum OidcApiScopes {
  OpenId = "openid",
  Email = "email",
  Profile = "profile",
}

export enum OAuthApiScopes {
  OfflineAccess = "offline_access",
}

export enum HSMApiScopes {
  Role = "role",
  SecurityCode = "security_code",
  User = "user",
  UserRole = "user_role",
  UserSecurityCode = "user_security_code",
  Customer = "customer",
  Service = "service",
  Food = "food",
  Meal = "meal",
  WeeklyMenu = "weekly_menu",
}

export class ApiScopes {
  static from(...scopes: string[]): string {
    return scopes.join(" ");
  }
}
