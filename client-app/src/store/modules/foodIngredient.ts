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
  HSMFoodIngredient,
  HSMFoodIngredientInputModel,
} from "@/models/api/Food";
import IFoodService from "@/services/foodService";

export interface FoodIngredientState {
  foodIngredients: HSMFoodIngredient[];
  editedFoodIngredient: HSMFoodIngredient | null;
  foodIngredientDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "foodIngredient" })
class FoodIngredient extends VuexModule implements FoodIngredientState {
  @$inject()
  private foodService!: IFoodService;

  public foodIngredients: HSMFoodIngredient[] = [];
  public editedFoodIngredient: HSMFoodIngredient | null = null;
  public foodIngredientDetailDialog = false;

  @Mutation
  private SET_FOOD_INGREDIENTS(foodIngredients: HSMFoodIngredient[]) {
    this.foodIngredients = foodIngredients;
  }

  @Mutation
  private SET_EDITED_FOOD_INGREDIENT(foodIngredient: HSMFoodIngredient | null) {
    this.editedFoodIngredient = foodIngredient;
  }

  @Mutation
  private SET_FOOD_INGREDIENT_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.foodIngredientDetailDialog = visible;
  }

  @Action
  public async fetchFoodIngredientsAsync() {
    this.SET_FOOD_INGREDIENTS(await this.foodService.getFoodIngredientsAsync());
  }

  @Action
  public async addFoodIngredientAsync(inputModel: HSMFoodIngredientInputModel) {
    const response = await this.foodService.createFoodIngredientAsync(
      inputModel
    );
    if (response.isSuccess) {
      await this.fetchFoodIngredientsAsync();
    }

    return response;
  }

  @Action
  public async editFoodIngredientAsync(data: {
    id: number;
    inputModel: HSMFoodIngredientInputModel;
  }) {
    const response = await this.foodService.updateFoodIngredientAsync(
      data.id,
      data.inputModel
    );
    if (response.isSuccess) {
      await this.fetchFoodIngredientsAsync();
    }

    return response;
  }

  @Action
  public async deleteFoodIngredientAsync(id: number) {
    const result = await this.foodService.deleteFoodIngredientAsync(id);
    if (result) {
      await this.fetchFoodIngredientsAsync();
    }

    return result;
  }

  @Action
  public ClearFoodIngredients() {
    this.SET_FOOD_INGREDIENTS([]);
  }

  @Action
  public showEditFoodIngredientDialogFor(foodIngredient: HSMFoodIngredient) {
    this.SET_EDITED_FOOD_INGREDIENT(foodIngredient);
    this.SET_FOOD_INGREDIENT_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeEditFoodIngredientDialog() {
    this.SET_FOOD_INGREDIENT_DETAIL_DIALOG_VISIBLE(false);
  }

  @Action
  public showAddFoodIngredientDialog() {
    this.SET_EDITED_FOOD_INGREDIENT(null);
    this.SET_FOOD_INGREDIENT_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeAddFoodIngredientDialog() {
    this.SET_FOOD_INGREDIENT_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(FoodIngredient);
