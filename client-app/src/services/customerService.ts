import { inject, injectable } from "inversify";
import "reflect-metadata";
import HSMCustomer, {
  RawHSMCustomer,
  HSMCustomerInputModel,
  HSMCustomerOptionsInputModel,
  HSMCustomerServiceAssignmentInput,
  HSMCustomerServiceInputModel,
} from "@/models/api/Customer";
import serviceTypes from "./serviceTypes";
import IConfiguration from "./configuration";
import IQueryService from "./queryService";
import HSMResponseModel from "@/models/api/responseModel";

export default interface ICustomerService {
  getCustomersAsync(): Promise<HSMCustomer[]>;
  getCustomerByIdAsync(id: number): Promise<HSMCustomer | null>;
  createCustomerAsync(
    inputModel: HSMCustomerInputModel
  ): Promise<HSMResponseModel<HSMCustomer>>;
  updateCustomerAsync(
    id: number,
    inputModel: HSMCustomerInputModel
  ): Promise<HSMResponseModel<HSMCustomer>>;
  deleteCustomerAsync(id: number): Promise<boolean>;
  setCustomerVisibilityAsync(
    id: number,
    visible: boolean
  ): Promise<HSMResponseModel<HSMCustomer>>;
  setCustomerDischargedAsync(
    id: number,
    discharged: boolean,
    discharged_date?: Date
  ): Promise<HSMResponseModel<HSMCustomer>>;
  assignServiceAsync(
    id: number,
    serviceAssignment: HSMCustomerServiceAssignmentInput
  ): Promise<HSMResponseModel<HSMCustomer>>;
}

@injectable()
export class DefaultCustomerService implements ICustomerService {
  public readonly routeName: string;

  constructor(
    @inject(serviceTypes.Configuration)
    private configuration: IConfiguration,
    @inject(serviceTypes.QueryService)
    private queryService: IQueryService
  ) {
    this.routeName = "/api/customers";
  }

  async getCustomersAsync(): Promise<HSMCustomer[]> {
    return await this.queryService.getCollectionAsync<HSMCustomer>(
      this.routeName,
      HSMCustomer
    );
  }

  async getCustomerByIdAsync(id: number): Promise<HSMCustomer | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMCustomer>(
      `${this.routeName}/${id}`,
      HSMCustomer
    );
  }

  async createCustomerAsync(
    inputModel: HSMCustomerInputModel
  ): Promise<HSMResponseModel<HSMCustomer>> {
    return await this.queryService.createAsync<HSMCustomer>(
      `${this.routeName}`,
      inputModel,
      HSMCustomer
    );
  }

  async updateCustomerAsync(
    id: number,
    inputModel: HSMCustomerInputModel
  ): Promise<HSMResponseModel<HSMCustomer>> {
    return await this.queryService.updateAsync<HSMCustomer>(
      `${this.routeName}/${id}`,
      inputModel,
      HSMCustomer
    );
  }

  async deleteCustomerAsync(id: number): Promise<boolean> {
    const response = await this.queryService.deleteAsync(
      `${this.routeName}/${id}`
    );

    return response.isSuccess;
  }

  async setCustomerVisibilityAsync(
    id: number,
    visible: boolean
  ): Promise<HSMResponseModel<HSMCustomer>> {
    return await this.queryService.updatePartialAsync<HSMCustomer>(
      `${this.routeName}/${id}`,
      {
        visible,
      } as HSMCustomerOptionsInputModel,
      HSMCustomer
    );
  }

  async setCustomerDischargedAsync(
    id: number,
    discharged: boolean,
    discharged_date?: Date
  ): Promise<HSMResponseModel<HSMCustomer>> {
    return await this.queryService.updatePartialAsync<HSMCustomer>(
      `${this.routeName}/${id}`,
      {
        discharged,
        discharged_date,
      } as HSMCustomerOptionsInputModel,
      HSMCustomer
    );
  }

  async assignServiceAsync(
    id: number,
    serviceAssignment: HSMCustomerServiceAssignmentInput
  ): Promise<HSMResponseModel<HSMCustomer>> {
    return await this.queryService.updatePartialAsync<HSMCustomer>(
      `${this.routeName}/${id}/services`,
      {
        services: serviceAssignment,
      } as HSMCustomerServiceInputModel,
      HSMCustomer
    );
  }
}
