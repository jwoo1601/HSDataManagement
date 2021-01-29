<template>
  <hsm-data-selection-dialog
    :visible.sync="syncedVisible"
    dialog-title="재료 카테고리 선택"
    header-icon="tag-fill"
    header="재료 카테고리"
    select-mode="single"
    @selection-confirm="select"
    @selection-cancel="cancel"
    :field-definitions="foodIngredientCategoryFieldDefinitions"
    :fetch-entries-action="fetchFoodIngredientCategoryEntriesAsync"
    :entries="foodIngredientCategories"
    :entries-mapper="mapFoodIngredientCategoryTableItems"
    :criteria="foodIngredientCategoryCriteria"
    :criteria-options="foodIngredientCategoryCriteriaOptions"
  >
    <template #cell(note)="{ value, index }">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`selection-foodIngredientCategory-showNote-${index}`"
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
          v-if="value"
          :target="`selection-foodIngredientCategory-showNote-${index}`"
          placement="top"
          variant="success"
        >
          {{ value }}
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
import FoodIngredientModule from "@/store/modules/foodIngredient";
import FoodIngredientCategoryModule from "@/store/modules/foodIngredientCategory";
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
import { HSMFoodIngredientCategory } from "@/models/api/Food";

@Component({
  components: {},
})
export default class HSMFoodIngredientCategorySelectionDialog extends Vue {
  @PropSync("visible", { default: false })
  syncedVisible!: boolean;

  readonly labels: HSMDataTableLabels = {
    item: "재료 카테고리",
  };
  readonly foodIngredientCategoryFieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "numIngredients", label: "등록된 재료", sortable: true },
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
  ];
  readonly foodIngredientCategoryCriteria: "id" | "name" | "numIngredients" =
    "id";
  readonly foodIngredientCategoryCriteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "numIngredients", text: "재료 수" },
  ];

  @Emit()
  select(selectedItems: HSMDataTableItem[]) {}
  @Emit()
  cancel() {}

  get foodIngredientCategories() {
    return FoodIngredientCategoryModule.foodIngredientCategories;
  }

  async fetchFoodIngredientCategoryEntriesAsync() {
    return FoodIngredientCategoryModule.fetchFoodIngredientCategoriesAsync();
  }

  mapFoodIngredientCategoryTableItems(category: HSMFoodIngredientCategory) {
    return {
      id: category.id,
      name: category.name,
      note: category.note ?? null,
      numIngredients: FoodIngredientModule.foodIngredients.filter(
        (s) => s.category?.id === category.id
      ).length,
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
