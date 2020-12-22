<template>
  <b-modal
    id="hsm-data-viewer"
    v-model="syncedVisible"
    size="lg"
    :title="dialogTitle"
    header-bg-variant="primary"
    header-text-variant="white"
    scrollable
    hide-footer
    @show="onShowDialog"
  >
    <template #modal-title>
      <h5 class="px-2 mb-0 font-weight-bold">
        <b-icon icon="list-ol" class="mr-2"></b-icon>
        {{ dialogTitle }}
      </h5>
    </template>

    <hsm-data-table
      ref="dataTable"
      v-bind="$attrs"
      v-on="$listeners"
      :allow-add="false"
      :allow-edit="false"
      :fetch-on-load="false"
    >
      <template v-for="(index, name) in $slots" v-slot:[name]>
        <slot :name="name" />
      </template>

      <template v-for="(index, name) in $scopedSlots" v-slot:[name]="data">
        <slot :name="name" v-bind="data"></slot>
      </template>
    </hsm-data-table>
  </b-modal>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import "reflect-metadata";
import { Emit, Prop, PropSync, Ref, Watch } from "vue-property-decorator";
import HSMDataTable, { HSMDataTableItem } from "./HSMDataTable.vue";
import { BvModalEvent } from "bootstrap-vue";

@Component({
  components: {},
})
export default class HSMDataViewer extends Vue {
  @Prop({ default: "" })
  dialogTitle!: string;

  @PropSync("visible", { default: false })
  syncedVisible!: boolean;

  @Ref()
  dataTable!: HSMDataTable;

  onShowDialog() {
    // if (this.dataTable) {
    //   this.dataTable.handleReloadItemsAsync();
    // }
  }
}
</script>

<style lang="scss" scoped></style>
>
