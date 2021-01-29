import { Transform, Type } from "class-transformer";
import HSMRole, { RawHSMRole } from "./Role";

export interface RawHSMUser {
  id: string;
  username: string;
  email: string;
  email_confirmed: boolean;
  name: string;
  security_code: string;
  locale: string;
  access_failed_count: number;
  lockout_end: string | null;
  registered_at: string;
  last_updated_at: string;
  is_active: boolean;
  roles: RawHSMRole[];
}

export default class HSMUser {
  id!: string;
  username!: string;
  email!: string;
  email_confirmed!: boolean;
  name!: string;
  security_code!: string;
  locale!: string;
  access_failed_count!: number;
  @Type(() => Date)
  @Transform((value) => (value !== null ? new Date(value) : null), {
    toClassOnly: true,
  })
  lockout_end!: Date | null;
  @Type(() => Date)
  registered_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
  is_active!: boolean;
  @Type(() => HSMRole)
  roles!: HSMRole[];
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

export interface HSMUserSetRoleInputModel {
  role: string;
}
