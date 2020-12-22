import { inject, injectable } from "inversify";
import "reflect-metadata";
import serviceTypes from "./serviceTypes";
import IConfiguration from "./configuration";
import IQueryService from "./queryService";
import HSMResponseModel from "@/models/api/responseModel";
import HSMFood, {
  HSMFoodCategory,
  HSMFoodIngredient,
  HSMFoodIngredientCategory,
  HSMFoodInputModel,
  HSMFoodCategoryInputModel,
  HSMFoodIngredientInputModel,
  HSMFoodIngredientCategoryInputModel,
} from "@/models/api/Food";

export default interface IFoodService {
  getFoodsAsync(): Promise<HSMFood[]>;
  getFoodByIdAsync(id: number): Promise<HSMFood | null>;
  createFoodAsync(
    inputModel: HSMFoodInputModel
  ): Promise<HSMResponseModel<HSMFood>>;
  updateFoodAsync(
    id: number,
    inputModel: HSMFoodInputModel
  ): Promise<HSMResponseModel<HSMFood>>;
  deleteFoodAsync(id: number): Promise<boolean>;

  getFoodCategoriesAsync(): Promise<HSMFoodCategory[]>;
  getFoodCategoryByIdAsync(id: number): Promise<HSMFoodCategory | null>;
  createFoodCategoryAsync(
    inputModel: HSMFoodCategoryInputModel
  ): Promise<HSMResponseModel<HSMFoodCategory>>;
  updateFoodCategoryAsync(
    id: number,
    inputModel: HSMFoodCategoryInputModel
  ): Promise<HSMResponseModel<HSMFoodCategory>>;
  deleteFoodCategoryAsync(id: number): Promise<boolean>;

  getFoodIngredientsAsync(): Promise<HSMFoodIngredient[]>;
  getFoodIngredientByIdAsync(id: number): Promise<HSMFoodIngredient | null>;
  createFoodIngredientAsync(
    inputModel: HSMFoodIngredientInputModel
  ): Promise<HSMResponseModel<HSMFoodIngredient>>;
  updateFoodIngredientAsync(
    id: number,
    inputModel: HSMFoodIngredientInputModel
  ): Promise<HSMResponseModel<HSMFoodIngredient>>;
  deleteFoodIngredientAsync(id: number): Promise<boolean>;

  getFoodIngredientCategoriesAsync(): Promise<HSMFoodIngredientCategory[]>;
  getFoodIngredientCategoryByIdAsync(
    id: number
  ): Promise<HSMFoodIngredientCategory | null>;
  createFoodIngredientCategoryAsync(
    inputModel: HSMFoodIngredientCategoryInputModel
  ): Promise<HSMResponseModel<HSMFoodIngredientCategory>>;
  updateFoodIngredientCategoryAsync(
    id: number,
    inputModel: HSMFoodIngredientCategoryInputModel
  ): Promise<HSMResponseModel<HSMFoodIngredientCategory>>;
  deleteFoodIngredientCategoryAsync(id: number): Promise<boolean>;
}

@injectable()
export class DefaultFoodService implements IFoodService {
  public readonly foodRouteName: string;
  public readonly foodCategoryRouteName: string;
  public readonly foodIngredientRouteName: string;
  public readonly foodIngredientCategoryRouteName: string;

  constructor(
    @inject(serviceTypes.Configuration)
    private configuration: IConfiguration,
    @inject(serviceTypes.QueryService)
    private queryService: IQueryService
  ) {
    this.foodRouteName = "/api/nutrition-support/foods";
    this.foodCategoryRouteName = `${this.foodRouteName}/categories`;
    this.foodIngredientRouteName = `${this.foodRouteName}/ingredients`;
    this.foodIngredientCategoryRouteName = `${this.foodIngredientRouteName}/categories`;
  }

  async getFoodsAsync(): Promise<HSMFood[]> {
    return await this.queryService.getCollectionAsync<HSMFood>(
      this.foodRouteName,
      HSMFood
    );
  }

  async getFoodByIdAsync(id: number): Promise<HSMFood | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMFood>(
      `${this.foodRouteName}/${id}`,
      HSMFood
    );
  }

  async createFoodAsync(
    inputModel: HSMFoodInputModel
  ): Promise<HSMResponseModel<HSMFood>> {
    return await this.queryService.createAsync<HSMFood>(
      `${this.foodRouteName}`,
      inputModel,
      HSMFood
    );
  }

  async updateFoodAsync(
    id: number,
    inputModel: HSMFoodInputModel
  ): Promise<HSMResponseModel<HSMFood>> {
    return await this.queryService.updateAsync<HSMFood>(
      `${this.foodRouteName}/${id}`,
      inputModel,
      HSMFood
    );
  }

  async deleteFoodAsync(id: number): Promise<boolean> {
    const response = await this.queryService.deleteAsync(
      `${this.foodRouteName}/${id}`
    );

    return response.isSuccess;
  }

  async getFoodCategoriesAsync(): Promise<HSMFoodCategory[]> {
    return await this.queryService.getCollectionAsync<HSMFoodCategory>(
      this.foodCategoryRouteName,
      HSMFoodCategory
    );
  }

  async getFoodCategoryByIdAsync(id: number): Promise<HSMFoodCategory | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMFoodCategory>(
      `${this.foodCategoryRouteName}/${id}`,
      HSMFoodCategory
    );
  }

  async createFoodCategoryAsync(
    inputModel: HSMFoodCategoryInputModel
  ): Promise<HSMResponseModel<HSMFoodCategory>> {
    return await this.queryService.createAsync<HSMFoodCategory>(
      `${this.foodCategoryRouteName}`,
      inputModel,
      HSMFoodCategory
    );
  }

  async updateFoodCategoryAsync(
    id: number,
    inputModel: HSMFoodCategoryInputModel
  ): Promise<HSMResponseModel<HSMFoodCategory>> {
    return await this.queryService.updateAsync<HSMFoodCategory>(
      `${this.foodCategoryRouteName}/${id}`,
      inputModel,
      HSMFoodCategory
    );
  }

  async deleteFoodCategoryAsync(id: number): Promise<boolean> {
    const response = await this.queryService.deleteAsync(
      `${this.foodCategoryRouteName}/${id}`
    );

    return response.isSuccess;
  }

  async getFoodIngredientsAsync(): Promise<HSMFoodIngredient[]> {
    return await this.queryService.getCollectionAsync<HSMFoodIngredient>(
      this.foodIngredientRouteName,
      HSMFoodIngredient
    );
  }

  async getFoodIngredientByIdAsync(
    id: number
  ): Promise<HSMFoodIngredient | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMFoodIngredient>(
      `${this.foodIngredientRouteName}/${id}`,
      HSMFoodIngredient
    );
  }

  async createFoodIngredientAsync(
    inputModel: HSMFoodIngredientInputModel
  ): Promise<HSMResponseModel<HSMFoodIngredient>> {
    return await this.queryService.createAsync<HSMFoodIngredient>(
      `${this.foodIngredientRouteName}`,
      inputModel,
      HSMFoodIngredient
    );
  }

  async updateFoodIngredientAsync(
    id: number,
    inputModel: HSMFoodIngredientInputModel
  ): Promise<HSMResponseModel<HSMFoodIngredient>> {
    return await this.queryService.updateAsync<HSMFoodIngredient>(
      `${this.foodIngredientRouteName}/${id}`,
      inputModel,
      HSMFoodIngredient
    );
  }

  async deleteFoodIngredientAsync(id: number): Promise<boolean> {
    const response = await this.queryService.deleteAsync(
      `${this.foodIngredientRouteName}/${id}`
    );

    return response.isSuccess;
  }

  async getFoodIngredientCategoriesAsync(): Promise<
    HSMFoodIngredientCategory[]
  > {
    return await this.queryService.getCollectionAsync<
      HSMFoodIngredientCategory
    >(this.foodIngredientCategoryRouteName, HSMFoodIngredientCategory);
  }

  async getFoodIngredientCategoryByIdAsync(
    id: number
  ): Promise<HSMFoodIngredientCategory | null> {
    return this.queryService.getSingleOrDefaultAsync<HSMFoodIngredientCategory>(
      `${this.foodIngredientCategoryRouteName}/${id}`,
      HSMFoodIngredientCategory
    );
  }

  async createFoodIngredientCategoryAsync(
    inputModel: HSMFoodIngredientCategoryInputModel
  ): Promise<HSMResponseModel<HSMFoodIngredientCategory>> {
    return await this.queryService.createAsync<HSMFoodIngredientCategory>(
      `${this.foodIngredientCategoryRouteName}`,
      inputModel,
      HSMFoodIngredientCategory
    );
  }

  async updateFoodIngredientCategoryAsync(
    id: number,
    inputModel: HSMFoodIngredientCategoryInputModel
  ): Promise<HSMResponseModel<HSMFoodIngredientCategory>> {
    return await this.queryService.updateAsync<HSMFoodIngredientCategory>(
      `${this.foodIngredientCategoryRouteName}/${id}`,
      inputModel,
      HSMFoodIngredientCategory
    );
  }

  async deleteFoodIngredientCategoryAsync(id: number): Promise<boolean> {
    const response = await this.queryService.deleteAsync(
      `${this.foodIngredientCategoryRouteName}/${id}`
    );

    return response.isSuccess;
  }
}
