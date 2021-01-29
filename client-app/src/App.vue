<template>
  <div id="app">
    <header>
      <hsm-navbar variant="submain" sticky></hsm-navbar>
    </header>

    <b-container fluid>
      <main role="main" class="pb-3">
        <router-view />
      </main>
    </b-container>

    <hsm-loading-dialog
      id="loading-loading"
      :visible="loading"
      :message="loadingText"
    ></hsm-loading-dialog>

    <hsm-dialog
      id="error-dialog"
      :visible="errorDialog"
      :title="errorDialogTitle"
      :message="errorDialogText"
      @close="handleErrorDialogClosed()"
    ></hsm-dialog>

    <footer style="margin-top: 160px">
      <footer-view></footer-view>
    </footer>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import ServiceTypes from "@/services/serviceTypes";
import serviceContainer from "@/services/serviceContainer";
import AppModule from "@/store/modules/app";
import FooterView from "@/views/Footer.vue";

@Component({
  components: {
    "footer-view": FooterView,
  },
  provide: {
    [ServiceTypes.ServiceContainer]: serviceContainer,
  },
})
export default class App extends Vue {
  get loading() {
    return AppModule.loading;
  }

  get loadingText() {
    return AppModule.loadingText;
  }

  get errorDialog() {
    return AppModule.errorDialog.visible;
  }

  get errorDialogTitle() {
    return AppModule.errorDialog.title;
  }

  get errorDialogText() {
    return AppModule.errorDialog.message;
  }

  handleErrorDialogClosed() {
    AppModule.hideErrorDialog();
  }
}
</script>

<style lang="scss">
#app {
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
</style>
