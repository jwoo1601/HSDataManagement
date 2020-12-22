import HSMResponseModel from "@/models/api/ResponseModel";
import { AxiosRequestConfig } from "axios";
import { injectable } from "inversify";
import httpClient from "@/services/httpClient";
import ApiServiceUtils from "./apiServiceUtils";
import "reflect-metadata";
import { plainToClass } from "class-transformer";

export interface QueryHeaders {
  [key: string]: string;
}

export type Constructible = { new (): any };

export default interface IQueryService {
  getCollectionAsync<TItem>(
    url: string,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<TItem[]>;
  getSingleAsync<TModel>(
    url: string,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel<TModel>>;
  getSingleOrDefaultAsync<TModel>(
    url: string,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<TModel | null>;
  createAsync<TOutputModel>(
    url: string,
    inputModel: any,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel<TOutputModel>>;
  updateAsync<TOutputModel>(
    url: string,
    inputModel: any,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel<TOutputModel>>;
  updatePartialAsync<TOutputModel>(
    url: string,
    inputModel: any,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel<TOutputModel>>;
  deleteAsync(
    url: string,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel>;
}

@injectable()
export class AxiosQueryService implements IQueryService {
  async getCollectionAsync<TItem>(
    url: string,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<TItem[]> {
    const response = await httpClient.get(url, config);

    if (ApiServiceUtils.isSuccessResponse(response.status)) {
      if (transformTo) {
        const arr = plainToClass(transformTo, response.data);

        const e = arr[0];
        Object.entries(e).forEach(([k, v]) => {
          console.log(typeof v);
        });
        console.log(JSON.stringify(arr, null, "\t"));

        return arr;
      } else {
        return response.data as TItem[];
      }
    }

    return [];
  }

  async getSingleAsync<TModel>(
    url: string,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel<TModel>> {
    const response = await httpClient.get(url, config);

    if (transformTo) {
      return new HSMResponseModel({
        ...response,
        data: plainToClass(transformTo, response.data),
      });
    } else {
      return new HSMResponseModel(response);
    }
  }

  async getSingleOrDefaultAsync<TModel>(
    url: string,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<TModel | null> {
    const resModel = await this.getSingleAsync<TModel>(
      url,
      transformTo,
      config
    );

    return resModel.model;
  }

  async createAsync<TOutputModel>(
    url: string,
    inputModel: any,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel<TOutputModel>> {
    const response = await httpClient.post(url, inputModel, config);

    if (transformTo) {
      return new HSMResponseModel({
        ...response,
        data: plainToClass(transformTo, response.data),
      });
    } else {
      return new HSMResponseModel(response);
    }
  }

  async updateAsync<TOutputModel>(
    url: string,
    inputModel: any,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel<TOutputModel>> {
    const response = await httpClient.put(url, inputModel, config);

    if (transformTo) {
      return new HSMResponseModel({
        ...response,
        data: plainToClass(transformTo, response.data),
      });
    } else {
      return new HSMResponseModel(response);
    }
  }

  async updatePartialAsync<TOutputModel>(
    url: string,
    inputModel: any,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel<TOutputModel>> {
    const response = await httpClient.patch(url, inputModel, config);

    if (transformTo) {
      return new HSMResponseModel({
        ...response,
        data: plainToClass(transformTo, response.data),
      });
    } else {
      return new HSMResponseModel(response);
    }
  }

  async deleteAsync(
    url: string,
    transformTo?: Constructible,
    config?: AxiosRequestConfig
  ): Promise<HSMResponseModel> {
    const response = await httpClient.delete(url, config);

    if (transformTo) {
      return new HSMResponseModel({
        ...response,
        data: plainToClass(transformTo, response.data),
      });
    } else {
      return new HSMResponseModel(response);
    }
  }
}
