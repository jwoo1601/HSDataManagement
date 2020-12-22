import Vue from "vue";
import Vuex from "vuex";
import createPersistedState from "vuex-persistedstate";
import SecureLS from "secure-ls";
import Cookies from "js-cookie";

import { AppState } from "@/store/modules/app";
import { AccountState } from "@/store/modules/account";
import { CustomerState } from "@/store/modules/customer";

Vue.use(Vuex);

export interface RootState {
  app: AppState;
  account: AccountState;
  customer: CustomerState;
}

const secureLocalStorage = new SecureLS({
  isCompression: false,
});

const store = new Vuex.Store<RootState>({
  plugins: [
    createPersistedState({
      storage: {
        getItem: (key) => secureLocalStorage.get(key),
        setItem: (key, value) => secureLocalStorage.set(key, value),
        removeItem: (key) => secureLocalStorage.remove(key),
      },
      paths: ["account"],
    }),
  ],
});

export default store;
