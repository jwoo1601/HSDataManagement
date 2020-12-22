<template>
  <b-modal
    id="hsm-data-selection-dialog"
    v-model="syncedVisible"
    size="lg"
    :title="dialogTitle"
    header-bg-variant="success"
    header-text-variant="white"
    ok-variant="success"
    :ok-title="selectLabel"
    cancel-variant="danger"
    :cancel-title="cancelLabel"
    scrollable
    hide-header-close
    no-close-on-backdrop
    no-close-on-esc
    @ok="handleSelect"
    @cancel="handleCancel"
    @show="onShowDialog"
  >
    <template #modal-title>
      <h5 class="px-2 mb-0 font-weight-bold">
        <b-icon icon="check2-square" class="mr-2"></b-icon>
        {{ dialogTitle }}
      </h5>
    </template>

    <hsm-data-table
      ref="dataTable"
      v-bind="$attrs"
      v-on="$listeners"
      :allow-add="false"
      :allow-delete="false"
      :show-actions="false"
      :fetch-on-load="false"
      :labels="labels"
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
import AppModule from "@/store/modules/app";
import {
  Emit,
  Model,
  Prop,
  PropSync,
  Ref,
  Watch,
} from "vue-property-decorator";
import HSMDataTable, {
  HSMDataTableItem,
  HSMDataTableLabels,
} from "./HSMDataTable.vue";
import { BvModalEvent } from "bootstrap-vue";

@Component({
  components: {},
})
export default class HSMDataSelectionDialog extends Vue {
  @Prop({ default: "" })
  dialogTitle!: string;
  @Prop({ default: "확인" })
  selectLabel!: string;
  @Prop({ default: "취소" })
  cancelLabel!: string;

  @PropSync("visible", { default: false })
  syncedVisible!: boolean;

  readonly labels: HSMDataTableLabels = {
    item: "서비스 그룹",
  };

  @Emit("selection-confirm")
  selectionConfirm(selectedItems: HSMDataTableItem[]) {}
  @Emit("selection-cancel")
  selectionCancel() {}

  @Ref()
  dataTable!: HSMDataTable;

  onShowDialog() {
    // this.dataTable.handleReloadItemsAsync();
  }

  handleSelect(e: BvModalEvent) {
    if (this.dataTable.selectedItems.length === 0) {
      e.preventDefault();
    } else {
      this.selectionConfirm(this.dataTable.selectedItems);
    }
  }

  handleCancel() {
    this.selectionCancel();
  }
}
</script>

<style lang="scss" scoped></style>
>
