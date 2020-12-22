export type HSMErrorType = "general" | "validation";

export interface HSMValidationErrorMap {
  [name: string]: string[];
}

export interface HSMErrorResponse {
  error_type: HSMErrorType;
}

export interface HSMGeneralErrorResponse extends HSMErrorResponse {
  message: string;
}

export interface HSMValidationErrorResponse extends HSMErrorResponse {
  errors: HSMValidationErrorMap;
}
