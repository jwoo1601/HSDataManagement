import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import {
  HSMFoodIngredientCategory,
  HSMFoodIngredientCategoryInputModel,
} from "@/models/api/Food";
import IFoodService from "@/services/foodService";

export interface FoodIngredientCategoryState {
  foodIngredientCategories: HSMFoodIngredientCategory[];
  editedFoodIngredientCategory: HSMFoodIngredientCategory | null;
  foodIngredientCategoryDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "foodIngredientCategory" })
class FoodIngredientCategory
  extends VuexModule
  implements FoodIngredientCategoryState {
  @$inject()
  private foodService!: IFoodService;

  public foodIngredientCategories: HSMFoodIngredientCategory[] = [];
  public editedFoodIngredientCategory: HSMFoodIngredientCategory | null = null;
  public foodIngredientCategoryDetailDialog = false;

  @Mutation
  private SET_FOOD_INGREDIENT_CATEGORIES(
    foodIngredientCategories: HSMFoodIngredientCategory[]
  ) {
    this.foodIngredientCategories = foodIngredientCategories;
  }

  @Mutation
  private SET_EDITED_FOOD_INGREDIENT_CATEGORY(
    foodIngredientCategory: HSMFoodIngredientCategory | null
  ) {
    this.editedFoodIngredientCategory = foodIngredientCategory;
  }

  @Mutation
  private SET_FOOD_INGREDIENT_CATEGORY_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.foodIngredientCategoryDetailDialog = visible;
  }

  @Action
  public async fetchFoodIngredientCategoriesAsync() {
    this.SET_FOOD_INGREDIENT_CATEGORIES(
      await this.foodService.getFoodIngredientCategoriesAsync()
    );
  }

  @Action
  public async addFoodIngredientCategoryAsync(
    inputModel: HSMFoodIngredientCategoryInputModel
  ) {
    const response = await this.foodService.createFoodIngredientCategoryAsync(
      inputModel
    );
    if (response.isSuccess) {
      await this.fetchFoodIngredientCategoriesAsync();
    }

    return response;
  }

  @Action
  public async editFoodIngredientCategoryAsync(data: {
    id: number;
    inputModel: HSMFoodIngredientCategoryInputModel;
  }) {
    const response = await this.foodService.updateFoodIngredientCategoryAsync(
      data.id,
      data.inputModel
    );
    if (response.isSuccess) {
      await this.fetchFoodIngredientCategoriesAsync();
    }

    return response;
  }

  @Action
  public async deleteFoodIngredientCategoryAsync(id: number) {
    const result = await this.foodService.deleteFoodIngredientCategoryAsync(id);
    if (result) {
      await this.fetchFoodIngredientCategoriesAsync();
    }

    return result;
  }

  @Action
  public ClearFoodIngredientCategories() {
    this.SET_FOOD_INGREDIENT_CATEGORIES([]);
  }

  @Action
  public showEditFoodIngredientCategoryDialogFor(
    foodIngredientCategory: HSMFoodIngredientCategory
  ) {
    this.SET_EDITED_FOOD_INGREDIENT_CATEGORY(foodIngredientCategory);
    this.SET_FOOD_INGREDIENT_CATEGORY_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeEditFoodIngredientCategoryDialog() {
    this.SET_FOOD_INGREDIENT_CATEGORY_DETAIL_DIALOG_VISIBLE(false);
  }

  @Action
  public showAddFoodIngredientCategoryDialog() {
    this.SET_EDITED_FOOD_INGREDIENT_CATEGORY(null);
    this.SET_FOOD_INGREDIENT_CATEGORY_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeAddFoodIngredientCategoryDialog() {
    this.SET_FOOD_INGREDIENT_CATEGORY_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(FoodIngredientCategory);
