import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import HSMFood, { HSMFoodInputModel } from "@/models/api/Food";
import IFoodService from "@/services/foodService";

export interface FoodState {
  foods: HSMFood[];
  editedFood: HSMFood | null;
  foodDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "food" })
class Food extends VuexModule implements FoodState {
  @$inject()
  private foodService!: IFoodService;

  public foods: HSMFood[] = [];
  public editedFood: HSMFood | null = null;
  public foodDetailDialog = false;

  @Mutation
  private SET_FOODS(foods: HSMFood[]) {
    this.foods = foods;
  }

  @Mutation
  private SET_EDITED_FOOD(food: HSMFood | null) {
    this.editedFood = food;
  }

  @Mutation
  private SET_FOOD_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.foodDetailDialog = visible;
  }

  @Action
  public async fetchFoodsAsync() {
    this.SET_FOODS(await this.foodService.getFoodsAsync());
  }

  @Action
  public async addFoodAsync(inputModel: HSMFoodInputModel) {
    const response = await this.foodService.createFoodAsync(inputModel);
    if (response.isSuccess) {
      await this.fetchFoodsAsync();
    }

    return response;
  }

  @Action
  public async editFoodAsync(data: {
    id: number;
    inputModel: HSMFoodInputModel;
  }) {
    const response = await this.foodService.updateFoodAsync(
      data.id,
      data.inputModel
    );
    if (response.isSuccess) {
      await this.fetchFoodsAsync();
    }

    return response;
  }

  @Action
  public async deleteFoodAsync(id: number) {
    const result = await this.foodService.deleteFoodAsync(id);
    if (result) {
      await this.fetchFoodsAsync();
    }

    return result;
  }

  @Action
  public ClearFoods() {
    this.SET_FOODS([]);
  }

  @Action
  public showEditFoodDialogFor(food: HSMFood) {
    this.SET_EDITED_FOOD(food);
    this.SET_FOOD_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeEditFoodDialog() {
    this.SET_FOOD_DETAIL_DIALOG_VISIBLE(false);
  }

  @Action
  public showAddFoodDialog() {
    this.SET_EDITED_FOOD(null);
    this.SET_FOOD_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeAddFoodDialog() {
    this.SET_FOOD_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(Food);
