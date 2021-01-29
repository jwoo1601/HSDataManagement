import { inject, injectable } from "inversify";
import "reflect-metadata";
import serviceTypes from "./serviceTypes";
import IConfiguration from "./configuration";
import IQueryService from "./queryService";
import HSMResponseModel from "@/models/api/responseModel";
import HSMRole from "@/models/api/Role";

export default interface IRoleService {
  getRolesAsync(): Promise<HSMRole[]>;
  getRoleByIdAsync(id: string): Promise<HSMRole | null>;
}

@injectable()
export class DefaultRoleService implements IRoleService {
  public readonly routeName: string;

  constructor(
    @inject(serviceTypes.Configuration)
    private configuration: IConfiguration,
    @inject(serviceTypes.QueryService)
    private queryService: IQueryService
  ) {
    this.routeName = "/api/roles";
  }

  async getRolesAsync(): Promise<HSMRole[]> {
    return await this.queryService.getCollectionAsync<HSMRole>(
      this.routeName,
      HSMRole
    );
  }

  async getRoleByIdAsync(id: string): Promise<HSMRole | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMRole>(
      `${this.routeName}/${id}`,
      HSMRole
    );
  }
}
