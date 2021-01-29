<template>
  <hsm-data-selection-dialog
    :visible.sync="syncedVisible"
    dialog-title="음식 카테고리 선택"
    header-icon="collection-fill"
    header="음식 카테고리"
    select-mode="single"
    @selection-confirm="select"
    @selection-cancel="cancel"
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
import FoodModule from "@/store/modules/food";
import FoodCategoryModule from "@/store/modules/foodCategory";
import {
  Emit,
  Model,
  Prop,
  PropSync,
  Ref,
  Watch,
} from "vue-property-decorator";
import HSMDataTable, {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "./HSMDataTable.vue";
import { BvModalEvent } from "bootstrap-vue";
import { HSMFoodCategory } from "@/models/api/Food";

@Component({
  components: {},
})
export default class HSMFoodCategorySelectionDialog extends Vue {
  @PropSync("visible", { default: false })
  syncedVisible!: boolean;

  readonly labels: HSMDataTableLabels = {
    item: "음식 카테고리",
  };
  readonly foodCategoryFieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "numFoods", label: "등록된 음식", sortable: true },
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
  ];
  readonly foodCategoryCriteria: "id" | "name" | "numFoods" = "id";
  readonly foodCategoryCriteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "numFoods", text: "음식 수" },
  ];

  @Emit()
  select(selectedItems: HSMDataTableItem[]) {}
  @Emit()
  cancel() {}

  get foodCategories() {
    return FoodCategoryModule.foodCategories;
  }

  async fetchFoodCategoryEntriesAsync() {
    return FoodCategoryModule.fetchFoodCategoriesAsync();
  }

  mapFoodCategoryTableItems(category: HSMFoodCategory) {
    return {
      id: category.id,
      name: category.name,
      note: category.note ?? null,
      numFoods: FoodModule.foods.filter((s) => s.category?.id === category.id)
        .length,
      action: {
        edit: true,
        delete: true,
      },
      raw: category,
    };
  }
}
</script>

<style lang="scss" scoped></style>
>
