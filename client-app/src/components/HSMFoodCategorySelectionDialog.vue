<template>
  <hsm-data-selection-dialog
    :visible.sync="categorySelectionDialogVisible"
    dialog-title="음식 카테고리 선택"
    header-icon="collection-fill"
    header="음식 카테고리"
    select-mode="single"
    @selection-confirm="handleFoodCategorySelections"
    @selection-cancel="handleFoodCategorySelectionDialogClose"
    :field-definitions="foodCategoryFieldDefinitions"
    :fetch-entries-action="fetchFoodCategoryEntriesAsync"
    :entries="foodCategories"
    :entries-mapper="mapFoodCategoryTableItems"
    :criteria="foodCategoryCriteria"
    :criteria-options="foodCategoryCriteriaOptions"
  >
    <template #cell(note)="data">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`selection-foodCategory-showNote-${data.index}`"
          class="mr-3"
          size="sm"
          icon="pencil-fill"
          variant="outline-success"
          textVariant="success"
          hoverVariant="success"
          hoverTextVariant="light"
          fontWeight="bold"
        >
          노트
        </hsm-button>
        <b-tooltip
          v-if="data.value"
          :target="`selection-foodCategory-showNote-${data.index}`"
          placement="top"
          variant="success"
        >
          {{ data.value }}
        </b-tooltip>
      </div>
    </template>
  </hsm-data-selection-dialog>
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
