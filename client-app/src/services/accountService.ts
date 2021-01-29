import { inject, injectable } from "inversify";
import "reflect-metadata";
import serviceTypes from "./serviceTypes";
import IConfiguration from "./configuration";
import IQueryService from "./queryService";
import HSMResponseModel from "@/models/api/responseModel";
import HSMSecurityCode, {
  HSMSecurityCodeGenerateInputModel,
  HSMSecurityCodeInvalidateInputModel,
} from "@/models/api/SecurityCode";

export default interface IAccountService {
  getSecurityCodeListAsync(): Promise<HSMSecurityCode[]>;
  generateSecurityCodeAsync(
    inputModel: HSMSecurityCodeGenerateInputModel
  ): Promise<HSMResponseModel<HSMSecurityCode>>;
  invalidateSecurityCodeAsync(
    inputModel: HSMSecurityCodeInvalidateInputModel
  ): Promise<boolean>;
}

@injectable()
export class DefaultAccountService implements IAccountService {
  public readonly routeName: string;
  public readonly securityCodeRouteName: string;

  constructor(
    @inject(serviceTypes.Configuration)
    private configuration: IConfiguration,
    @inject(serviceTypes.QueryService)
    private queryService: IQueryService
  ) {
    this.routeName = "/api/accounts";
    this.securityCodeRouteName = `${this.routeName}/security-code`;
  }

  async getSecurityCodeListAsync(): Promise<HSMSecurityCode[]> {
    return await this.queryService.getCollectionAsync<HSMSecurityCode>(
      this.securityCodeRouteName,
      HSMSecurityCode
    );
  }

  async generateSecurityCodeAsync(
    inputModel: HSMSecurityCodeGenerateInputModel
  ): Promise<HSMResponseModel<HSMSecurityCode>> {
    return await this.queryService.createAsync<HSMSecurityCode>(
      `${this.securityCodeRouteName}/generate`,
      inputModel,
      HSMSecurityCode
    );
  }

  async invalidateSecurityCodeAsync(
    inputModel: HSMSecurityCodeInvalidateInputModel
  ): Promise<boolean> {
    const response = await this.queryService.updatePartialAsync(
      `${this.securityCodeRouteName}/invalidate`,
      inputModel
    );

    return response.isSuccess;
  }
}
