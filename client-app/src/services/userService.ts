import { inject, injectable } from "inversify";
import "reflect-metadata";
import HSMUser, { HSMUserSetRoleInputModel } from "@/models/api/User";
import serviceTypes from "./serviceTypes";
import IConfiguration from "./configuration";
import IQueryService from "./queryService";
import HSMResponseModel from "@/models/api/responseModel";

export default interface IUserService {
  getUsersAsync(): Promise<HSMUser[]>;
  getUserByIdAsync(id: string): Promise<HSMUser | null>;
  deleteUserAsync(id: string): Promise<boolean>;
  activateUserAsync(id: string): Promise<HSMResponseModel<HSMUser>>;
  inactivateUserAsync(id: string): Promise<HSMResponseModel<HSMUser>>;
  markEmailConfirmedAsync(id: string): Promise<HSMResponseModel<HSMUser>>;
  endLockoutAsync(id: string): Promise<HSMResponseModel<HSMUser>>;
  setUserRoleAsync(
    id: string,
    inputModel: HSMUserSetRoleInputModel
  ): Promise<HSMResponseModel<HSMUser>>;
}

@injectable()
export class DefaultUserService implements IUserService {
  public readonly routeName: string;

  constructor(
    @inject(serviceTypes.Configuration)
    private configuration: IConfiguration,
    @inject(serviceTypes.QueryService)
    private queryService: IQueryService
  ) {
    this.routeName = "/api/users";
  }

  async getUsersAsync(): Promise<HSMUser[]> {
    return await this.queryService.getCollectionAsync<HSMUser>(
      this.routeName,
      HSMUser
    );
  }

  async getUserByIdAsync(id: string): Promise<HSMUser | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMUser>(
      `${this.routeName}/${id}`,
      HSMUser
    );
  }

  async deleteUserAsync(id: string): Promise<boolean> {
    const response = await this.queryService.deleteAsync(
      `${this.routeName}/${id}`
    );

    return response.isSuccess;
  }

  async activateUserAsync(id: string): Promise<HSMResponseModel<HSMUser>> {
    return await this.queryService.updatePartialAsync<HSMUser>(
      `${this.routeName}/${id}/activate`,
      {},
      HSMUser
    );
  }

  async inactivateUserAsync(id: string): Promise<HSMResponseModel<HSMUser>> {
    return await this.queryService.updatePartialAsync<HSMUser>(
      `${this.routeName}/${id}/inactivate`,
      {},
      HSMUser
    );
  }

  async markEmailConfirmedAsync(
    id: string
  ): Promise<HSMResponseModel<HSMUser>> {
    return await this.queryService.updatePartialAsync<HSMUser>(
      `${this.routeName}/${id}/mark-email-confirmed`,
      {},
      HSMUser
    );
  }

  async endLockoutAsync(id: string): Promise<HSMResponseModel<HSMUser>> {
    return await this.queryService.updatePartialAsync<HSMUser>(
      `${this.routeName}/${id}/end-lockout`,
      {},
      HSMUser
    );
  }

  async setUserRoleAsync(
    id: string,
    inputModel: HSMUserSetRoleInputModel
  ): Promise<HSMResponseModel<HSMUser>> {
    console.log(JSON.stringify(inputModel, null, "\t"));

    return await this.queryService.updatePartialAsync<HSMUser>(
      `${this.routeName}/${id}/set-role`,
      inputModel,
      HSMUser
    );
  }
}
