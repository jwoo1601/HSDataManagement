import ApiServiceUtils from "@/services/apiServiceUtils";
import { AxiosResponse } from "axios";
import {
  HSMErrorResponse,
  HSMGeneralErrorResponse,
  HSMValidationErrorResponse,
} from "./Response";
import HSMValidationErrorCollection from "./ValidationError";

export default class HSMResponseModel<TOutputModel = null> {
  private _statusCode: number;
  private _data: TOutputModel | HSMErrorResponse;
  private _validationErrors?: HSMValidationErrorCollection;

  constructor(response: AxiosResponse<TOutputModel | HSMErrorResponse>) {
    this._statusCode = response.status;
    this._data = response.data;

    if (this.isValidationError) {
      this._validationErrors = new HSMValidationErrorCollection(
        this._data as HSMValidationErrorResponse
      );
    }
  }

  get isSuccess() {
    return ApiServiceUtils.isSuccessResponse(this._statusCode);
  }

  get isError() {
    return ApiServiceUtils.isErrorResponse(this._statusCode);
  }

  get isGeneralError() {
    return (
      this.isError && (this._data as HSMErrorResponse).error_type === "general"
    );
  }

  get isValidationError() {
    return (
      this.isError &&
      (this._data as HSMErrorResponse).error_type === "validation"
    );
  }

  get model() {
    if (this.isSuccess) {
      return this._data as TOutputModel;
    }

    return null;
  }

  get errorMessage() {
    if (this.isGeneralError) {
      return (this._data as HSMGeneralErrorResponse).message;
    }

    return null;
  }

  get validationErrors() {
    return this._validationErrors ?? null;
  }
}
