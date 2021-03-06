import Vue from "vue";
import "@/custom/style.hsm.scss";
import BootstrapVue, { BootstrapVueIcons } from "bootstrap-vue";
import { AxiosInstance } from "axios";
import HttpClient from "@/services/httpClient";
import { vueInversifyPlugin } from "@vanroeybe/vue-inversify-plugin";
import container from "./services/serviceContainer";
import i18n from "./i18n";
import Clipboard from "v-clipboard";

import App from "./App.vue";
import router from "./router";
import store from "./store";

import HSMNavbar from "@/components/HSMNavbar.vue";
import HSMButton from "@/components/HSMButton.vue";
import HSMDropdown from "@/components/HSMDropdown.vue";
import HSMLoadingDialog from "@/components/HSMLoadingDialog.vue";
import HSMSuccess from "@/components/HSMSuccess.vue";
import HSMError from "@/components/HSMError.vue";
import HSMItemDetail from "@/components/HSMItemDetail.vue";
import HSMCustomerDetail from "@/components/HSMCustomerDetail.vue";
import HSMDialog from "@/components/HSMDialog.vue";
import HSMDataTable from "@/components/HSMDataTable.vue";
import HSMDataTableLite from "@/components/HSMDataTableLite.vue";
import HSMDataViewer from "@/components/HSMDataViewer.vue";
import HSMDataSelectionDialog from "@/components/HSMDataSelectionDialog.vue";
import HSMPostList from "@/components/HSMPostList.vue";

import { HSMConfirmDialogPlugin } from "@/plugins/HSMConfirmDialogPlugin";
import { HSMMessageDialogPlugin } from "@/plugins/HSMMessageDialogPlugin";
import Component from "vue-class-component";

Vue.prototype.$http = HttpClient;

declare module "vue/types/vue" {
  interface Vue {
    $http: AxiosInstance;
    $hsmConfirmDialog(
      title: string,
      message: string,
      options?: { icon: string; titleVariant: string }
    ): Promise<boolean>;
    $hsmMessageDialog(
      title: string,
      message: string,
      options?: { icon: string; titleVariant: string }
    ): void;
    $clipboard(value: string): boolean;
  }
}

Vue.config.productionTip = false;

Vue.use(BootstrapVue);
Vue.use(BootstrapVueIcons);
Vue.use(vueInversifyPlugin(container));
Vue.use(HSMConfirmDialogPlugin);
Vue.use(HSMMessageDialogPlugin);
Vue.use(Clipboard);

Component.registerHooks([
  "beforeRouteEnter",
  "beforeRouteLeave",
  "beforeRouteUpdate",
]);

Vue.component("hsm-navbar", HSMNavbar);
Vue.component("hsm-button", HSMButton);
Vue.component("hsm-dropdown", HSMDropdown);
Vue.component("hsm-loading-dialog", HSMLoadingDialog);
Vue.component("hsm-success", HSMSuccess);
Vue.component("hsm-error", HSMError);
Vue.component("hsm-item-detail", HSMItemDetail);
Vue.component("hsm-dialog", HSMDialog);
Vue.component("hsm-data-table", HSMDataTable);
Vue.component("hsm-data-table-lite", HSMDataTableLite);
Vue.component("hsm-data-viewer", HSMDataViewer);
Vue.component("hsm-data-selection-dialog", HSMDataSelectionDialog);
Vue.component("hsm-post-list", HSMPostList);

new Vue({
  i18n,
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");

console.log(`Node Environment: ${process.env.NODE_ENV}`);
