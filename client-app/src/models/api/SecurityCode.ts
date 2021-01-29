import { Type } from "class-transformer";

export type HSMSecurityCodeType = "normal" | "transient" | "persistent";
export enum HSMSecurityCodeTypes {
  Normal = "normal",
  Transient = "transient",
  Persistent = "persistent",
}

export interface RawHSMSecurityCode {
  id: number;
  code_type: HSMSecurityCodeType;
  value: string;
  is_valid: boolean;
  generated_at: Date;
  expires_at?: Date;
}

export default class HSMSecurityCode {
  id!: number;
  code_type!: HSMSecurityCodeType;
  value!: string;
  is_valid!: boolean;
  @Type(() => Date)
  generated_at!: Date;
  @Type(() => Date)
  expires_at?: Date;
}

export interface HSMSecurityCodeGenerateInputModel {
  code_type?: HSMSecurityCodeType;
  age?: number; // in seconds
}

export interface HSMSecurityCodeInvalidateInputModel {
  code: string;
}
