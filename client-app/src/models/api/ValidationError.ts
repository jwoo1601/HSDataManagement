import { HSMValidationErrorResponse } from "./Response";

export interface HSMValidationErrorEntry {
  key: string;
  value: string;
}

export default class HSMValidationErrorCollection {
  private errorMap: Map<string, HSMValidationErrorEntry[]>;

  constructor(response?: HSMValidationErrorResponse) {
    this.errorMap = new Map<string, HSMValidationErrorEntry[]>();

    if (response) {
      Object.entries(response.errors).forEach(([name, errors]) => {
        this.errorMap.set(
          name,
          errors.map((error) => ({
            key: "default",
            value: error,
          }))
        );
      });
    }
  }

  get errors() {
    return this.errorMap.entries();
  }

  hasAnyDistinctErrorFor(name: string, key = "default") {
    return this.getErrorFor(name, key) !== undefined;
  }

  hasAnyErrorsFor(name: string) {
    return (this.getAllErrorsFor(name)?.length ?? 0) > 0;
  }

  getErrorFor(name: string, key = "default") {
    return this.errorMap.get(name)?.filter((entry) => entry.key === key)?.[0];
  }

  getAllErrorsFor(name: string) {
    return this.errorMap.get(name);
  }

  getAllErrors() {
    return Array.from(this.errorMap.entries()).flatMap(
      ([key, entries]) => entries
    );
  }

  addErrorsFor(name: string, messages: string[], key = "default") {
    this.clearErrorsFor(name, key);

    const existingErrors = this.getAllErrorsFor(name);
    // if (!existingErrors) {
    //   this.errorMap.set(name, []);
    //   existingErrors = this.getAllErrorsFor(name);
    // }

    messages.forEach((msg) => {
      existingErrors?.push({
        key,
        value: msg,
      });
    });
  }

  addErrorFor(name: string, message: string, key = "default") {
    this.addErrorsFor(name, [message], key);
  }

  clearAllErrors() {
    this.errorMap.clear();
  }

  clearErrorsFor(name: string, key = "default") {
    this.errorMap.set(
      name,
      this.errorMap.get(name)?.filter((entry) => entry.key !== key) ?? []
    );
  }

  clearAllErrorsFor(name: string) {
    this.errorMap.delete(name);
  }
}
