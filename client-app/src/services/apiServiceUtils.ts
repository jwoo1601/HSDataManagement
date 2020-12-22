import { AxiosResponse } from "axios";
import httpClient from "./httpClient";

export default class ApiServiceUtils {
  static getRequestHeader(key: string): string | undefined {
    return httpClient.defaults.headers.common[key];
  }

  static getRequestHeaders() {
    return httpClient.defaults.headers.common;
  }

  static setRequestHeaders(headers: any) {
    httpClient.defaults.headers.common = headers;
  }

  static addRequestHeader(key: string, value: string) {
    httpClient.defaults.headers.common = {
      ...httpClient.defaults.headers.common,
      [key]: value,
    };
  }

  static clearAuthorization() {
    ApiServiceUtils.clearRequestHeader("Authorization");
  }

  static clearRequestHeader(key: string) {
    httpClient.defaults.headers.common = {
      [key]: undefined,
    };
  }

  static clearRequestHeaders() {
    httpClient.defaults.headers.common = {};
  }

  static isSuccessResponse(statusCode: number) {
    return statusCode >= 200 && statusCode < 300;
  }

  static isErrorResponse(statusCode: number) {
    return (
      this.isClientErrorResponse(statusCode) ||
      this.isServerErrorResponse(statusCode)
    );
  }

  static isClientErrorResponse(statusCode: number) {
    return statusCode >= 400 && statusCode < 500;
  }

  static isServerErrorResponse(statusCode: number) {
    return statusCode >= 500;
  }
}
