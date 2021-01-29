<template>
  <b-modal
    v-model="syncedVisible"
    size="md"
    header-text-variant="white"
    body-text-variant="white"
    footer-text-variant="white"
    header-bg-variant="dark"
    body-bg-variant="dark"
    footer-bg-variant="dark"
    :hide-footer="!showFooter"
    centered
    busy
    no-close-on-backdrop
    no-close-on-esc
    no-fade
    :hide-header-close="!showCloseButton"
    static
    v-bind="$attrs"
    v-on="$listeners"
    @close="close()"
  >
    <template #modal-title>
      <slot name="dialog-title">
        <div class="font-weight-bold px-2" :class="`text-${titleVariant}`">
          <b-icon :icon="icon" class="mr-2"></b-icon>
          {{ title }}
        </div>
      </slot>
    </template>
    <template #modal-header-close><b-icon icon="x-square"></b-icon></template>
    <template>
      <b-container class="py-2">
        <slot>
          <b-row>
            <b-col>{{ message }}</b-col>
          </b-row>
        </slot>
      </b-container>
    </template>
    <template #modal-footer>
      <hsm-button
        icon="check"
        variant="success"
        :spacing="1"
        @click="confirm()"
      >
        {{ $t("action.confirm") }}
      </hsm-button>
      <hsm-button
        icon="x"
        variant="danger"
        class="ml-2"
        :spacing="1"
        @click="cancel()"
      >
        {{ $t("action.cancel") }}
      </hsm-button>
    </template>
  </b-modal>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Emit, Prop, PropSync } from "vue-property-decorator";

@Component({
  components: {},
})
export default class HSMDialog extends Vue {
  @PropSync("visible", { default: false })
  syncedVisible!: boolean;
  @Prop({ default: "exclamation-triangle-fill" })
  icon!: string;
  @Prop({ default: "danger" })
  titleVariant!: string;
  @Prop({ default: "" })
  title!: string;
  @Prop({ default: "" })
  message!: string;
  @Prop({ default: false })
  showFooter!: boolean;
  @Prop({ default: true })
  showCloseButton!: boolean;

  @Emit()
  close() {}
  @Emit()
  confirm() {}
  @Emit()
  cancel() {}
}
</script>

<style lang="scss" scoped></style>
