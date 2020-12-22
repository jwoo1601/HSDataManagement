import { inject, injectable } from "inversify";
import "reflect-metadata";
import serviceTypes from "./serviceTypes";
import IConfiguration from "./configuration";
import IQueryService from "./queryService";
import HSMResponseModel from "@/models/api/responseModel";
import HSMService, {
  HSMServiceGroup,
  HSMServiceInputModel,
  HSMServiceGroupInputModel,
} from "@/models/api/Service";

export default interface IServiceService {
  getServicesAsync(): Promise<HSMService[]>;
  getServiceByIdAsync(id: number): Promise<HSMService | null>;
  createServiceAsync(
    inputModel: HSMServiceInputModel
  ): Promise<HSMResponseModel<HSMService>>;
  updateServiceAsync(
    id: number,
    inputModel: HSMServiceInputModel
  ): Promise<HSMResponseModel<HSMService>>;
  deleteServiceAsync(id: number): Promise<boolean>;

  getServiceGroupsAsync(): Promise<HSMServiceGroup[]>;
  getServiceGroupByIdAsync(id: number): Promise<HSMServiceGroup | null>;
  createServiceGroupAsync(
    inputModel: HSMServiceGroupInputModel
  ): Promise<HSMResponseModel<HSMServiceGroup>>;
  updateServiceGroupAsync(
    id: number,
    inputModel: HSMServiceGroupInputModel
  ): Promise<HSMResponseModel<HSMServiceGroup>>;
  deleteServiceGroupAsync(id: number): Promise<boolean>;
}

@injectable()
export class DefaultServiceService implements IServiceService {
  public readonly serviceRouteName: string;
  public readonly serviceGroupRouteName: string;

  constructor(
    @inject(serviceTypes.Configuration)
    private configuration: IConfiguration,
    @inject(serviceTypes.QueryService)
    private queryService: IQueryService
  ) {
    this.serviceRouteName = "/api/nutrition-support/services";
    this.serviceGroupRouteName = `${this.serviceRouteName}/groups`;
  }

  async getServicesAsync(): Promise<HSMService[]> {
    return await this.queryService.getCollectionAsync<HSMService>(
      this.serviceRouteName,
      HSMService
    );
  }

  async getServiceByIdAsync(id: number): Promise<HSMService | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMService>(
      `${this.serviceRouteName}/${id}`,
      HSMService
    );
  }

  async createServiceAsync(
    inputModel: HSMServiceInputModel
  ): Promise<HSMResponseModel<HSMService>> {
    return await this.queryService.createAsync<HSMService>(
      `${this.serviceRouteName}`,
      inputModel,
      HSMService
    );
  }

  async updateServiceAsync(
    id: number,
    inputModel: HSMServiceInputModel
  ): Promise<HSMResponseModel<HSMService>> {
    return await this.queryService.updateAsync<HSMService>(
      `${this.serviceRouteName}/${id}`,
      inputModel,
      HSMService
    );
  }

  async deleteServiceAsync(id: number): Promise<boolean> {
    const response = await this.queryService.deleteAsync(
      `${this.serviceRouteName}/${id}`
    );

    return response.isSuccess;
  }

  async getServiceGroupsAsync(): Promise<HSMServiceGroup[]> {
    return await this.queryService.getCollectionAsync<HSMServiceGroup>(
      this.serviceGroupRouteName,
      HSMServiceGroup
    );
  }

  async getServiceGroupByIdAsync(id: number): Promise<HSMServiceGroup | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMServiceGroup>(
      `${this.serviceGroupRouteName}/${id}`,
      HSMServiceGroup
    );
  }

  async createServiceGroupAsync(
    inputModel: HSMServiceGroupInputModel
  ): Promise<HSMResponseModel<HSMServiceGroup>> {
    return await this.queryService.createAsync<HSMServiceGroup>(
      `${this.serviceGroupRouteName}`,
      inputModel,
      HSMServiceGroup
    );
  }

  async updateServiceGroupAsync(
    id: number,
    inputModel: HSMServiceGroupInputModel
  ): Promise<HSMResponseModel<HSMServiceGroup>> {
    return await this.queryService.updateAsync<HSMServiceGroup>(
      `${this.serviceGroupRouteName}/${id}`,
      inputModel,
      HSMServiceGroup
    );
  }

  async deleteServiceGroupAsync(id: number): Promise<boolean> {
    const response = await this.queryService.deleteAsync(
      `${this.serviceGroupRouteName}/${id}`
    );

    return response.isSuccess;
  }
}
