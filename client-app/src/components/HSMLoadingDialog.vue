<template>
  <b-modal
    :id="$attrs.id"
    :visible="visible"
    size="sm"
    title-class="font-weight-bold mx-auto"
    body-text-variant="white"
    body-bg-variant="dark"
    centered
    busy
    no-close-on-backdrop
    no-close-on-esc
    no-fade
    hide-header-close
    hide-header
    hide-footer
    static
  >
    <b-container class="pt-5 pb-4 text-center">
      <b-row>
        <b-col>
          <b-spinner class="spinner h4"></b-spinner>
        </b-col>
      </b-row>
      <b-row class="mt-4">
        <b-col>
          <div class="h5 font-weight-bold">{{ message }}</div>
        </b-col>
      </b-row>
    </b-container>
  </b-modal>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import "reflect-metadata";
import { Prop, Watch, Emit } from "vue-property-decorator";
import { State } from "vuex-class";

import { BModal, BSpinner, BContainer, BRow, BCol } from "bootstrap-vue";

@Component({
  components: { BModal, BSpinner, BContainer, BRow, BCol },
})
export default class HSMLoadingDialog extends Vue {
  @Prop({ default: false })
  visible!: boolean;
  @Prop({ default: "로딩 중" })
  message!: string;
}
</script>

<style lang="scss" scoped>
.spinner {
  width: 3rem !important;
  height: 3rem !important;
}
</style>
