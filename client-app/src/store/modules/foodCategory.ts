import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import { HSMFoodCategory, HSMFoodCategoryInputModel } from "@/models/api/Food";
import IFoodService from "@/services/foodService";

export interface FoodCategoryState {
  foodCategories: HSMFoodCategory[];
  editedFoodCategory: HSMFoodCategory | null;
  foodCategoryDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "foodCategory" })
class FoodCategory extends VuexModule implements FoodCategoryState {
  @$inject()
  private foodService!: IFoodService;

  public foodCategories: HSMFoodCategory[] = [];
  public editedFoodCategory: HSMFoodCategory | null = null;
  public foodCategoryDetailDialog = false;

  @Mutation
  private SET_FOOD_CATEGORIES(foodCategories: HSMFoodCategory[]) {
    this.foodCategories = foodCategories;
  }

  @Mutation
  private SET_EDITED_FOOD_CATEGORY(foodCategory: HSMFoodCategory | null) {
    this.editedFoodCategory = foodCategory;
  }

  @Mutation
  private SET_FOOD_CATEGORY_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.foodCategoryDetailDialog = visible;
  }

  @Action
  public async fetchFoodCategoriesAsync() {
    this.SET_FOOD_CATEGORIES(await this.foodService.getFoodCategoriesAsync());
  }

  @Action
  public async addFoodCategoryAsync(inputModel: HSMFoodCategoryInputModel) {
    const response = await this.foodService.createFoodCategoryAsync(inputModel);
    if (response.isSuccess) {
      await this.fetchFoodCategoriesAsync();
    }

    return response;
  }

  @Action
  public async editFoodCategoryAsync(data: {
    id: number;
    inputModel: HSMFoodCategoryInputModel;
  }) {
    const response = await this.foodService.updateFoodCategoryAsync(
      data.id,
      data.inputModel
    );
    if (response.isSuccess) {
      await this.fetchFoodCategoriesAsync();
    }

    return response;
  }

  @Action
  public async deleteFoodCategoryAsync(id: number) {
    const result = await this.foodService.deleteFoodCategoryAsync(id);
    if (result) {
      await this.fetchFoodCategoriesAsync();
    }

    return result;
  }

  @Action
  public ClearFoodCategories() {
    this.SET_FOOD_CATEGORIES([]);
  }

  @Action
  public showEditFoodCategoryDialogFor(foodCategory: HSMFoodCategory) {
    this.SET_EDITED_FOOD_CATEGORY(foodCategory);
    this.SET_FOOD_CATEGORY_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeEditFoodCategoryDialog() {
    this.SET_FOOD_CATEGORY_DETAIL_DIALOG_VISIBLE(false);
  }

  @Action
  public showAddFoodCategoryDialog() {
    this.SET_EDITED_FOOD_CATEGORY(null);
    this.SET_FOOD_CATEGORY_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeAddFoodCategoryDialog() {
    this.SET_FOOD_CATEGORY_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(FoodCategory);
