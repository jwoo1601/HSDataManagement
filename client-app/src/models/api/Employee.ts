export interface HSMEmployee {
  id: number;
  role: HSMEmployeeRole;
  licensedDate: Date;
  licenseRenewalDate?: Date;
}

export interface HSMEmployeeRole {
  id: number;
  name: string;
  displayName: string;
}
