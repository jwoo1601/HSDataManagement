import Vue from "vue";
import Vuex from "vuex";
import createPersistedState from "vuex-persistedstate";
import Cookies from "js-cookie";
import secureStorage from "@/store/secureStorage";

import { AppState } from "@/store/modules/app";
import { AccountState } from "@/store/modules/account";
import { CustomerState } from "@/store/modules/customer";
import { FoodState } from "@/store/modules/food";
import { FoodCategoryState } from "@/store/modules/foodCategory";
import { FoodIngredientState } from "@/store/modules/foodIngredient";
import { FoodIngredientCategoryState } from "@/store/modules/foodIngredientCategory";
import { ServiceState } from "@/store/modules/service";
import { ServiceGroupState } from "@/store/modules/serviceGroup";

Vue.use(Vuex);

export interface RootState {
  app: AppState;
  account: AccountState;
  customer: CustomerState;
  food: FoodState;
  foodCategory: FoodCategoryState;
  foodIngredient: FoodIngredientState;
  foodIngredientCategory: FoodIngredientCategoryState;
  service: ServiceState;
  serviceGroup: ServiceGroupState;
}

const store = new Vuex.Store<RootState>({
  plugins: [
    createPersistedState({
      key: "hsm_store_cache",
      storage: {
        getItem: (key) => secureStorage.get(key),
        setItem: (key, value) => secureStorage.set(key, value),
        removeItem: (key) => secureStorage.remove(key),
      },
    }),
  ],
});

export default store;
