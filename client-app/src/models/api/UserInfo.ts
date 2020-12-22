export default interface HSMUserInfoBase {
  email: string;
  email_verified: boolean;
  locale: string;
  name: string;
  role: string;
  sub: string;
  registered_at: Date;
  updated_at: Date;
  username: string;
  access_failed_count: string;
}
