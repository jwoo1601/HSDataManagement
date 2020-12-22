import { HSMRole } from "./Role";

export interface HSMUser {
  id: number;
  username: string;
  email: string;
  email_confirmed: string;
  name: string;
  security_code: string;
  locale: string;
  access_failed_count: number;
  lockout_end: Date;
  last_updated_time: Date;
  is_active: boolean;
  roles: HSMRole[];
}

export interface HSMUserRegisterInputModel {
  username?: string;
  password?: string;
  confirm_password?: string;
  name?: string;
  email?: string;
  email_domain?: string;
  security_code?: string;
}
