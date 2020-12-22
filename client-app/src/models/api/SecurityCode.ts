export interface HSMSecurityCode {
  id: number;
  codeType: HSMSecurityCode;
  value: string;
  isValid: boolean;
  generatedAt: Date;
  expiresAt?: Date;
}

export enum HSMSecurityCodeType {
  Normal,
  Transient,
  Persistent,
}
