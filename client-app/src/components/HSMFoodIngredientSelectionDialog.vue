<template>
  <hsm-data-selection-dialog
    :visible.sync="syncedVisible"
    dialog-title="재료 선택"
    header-icon="basket2-fill"
    header="재료"
    select-mode="range"
    @selection-confirm="select"
    @selection-cancel="cancel"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="ingredients"
    :entries-mapper="mapTableItems"
    :criteria="criteria"
    :criteria-options="criteriaOptions"
  >
    <template #cell(category)="{ value, unformatted }">
      <div class="d-flex justify-content-center text-center pt-1">
        <b-link :to="{ path: unformatted.name }">{{ value }}</b-link>
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
import {
  HSMFoodIngredient,
  HSMFoodIngredientCategory,
} from "@/models/api/Food";

@Component({
  components: {},
})
export default class HSMFoodIngredientSelectionDialog extends Vue {
  @PropSync("visible", { default: false })
  syncedVisible!: boolean;

  readonly labels: HSMDataTableLabels = {
    item: "재료",
  };
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    {
      key: "origin",
      label: "원산지",
      sortable: true,
      formatter: this.formatOrigin,
    },
    {
      key: "category",
      label: "카테고리",
      sortable: true,
      formatter: this.formatCategory,
      hasCustomRenderer: true,
    },
  ];
  readonly criteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "origin", text: "원산지" },
    { value: "category", text: "카테고리" },
  ];

  criteria: "id" | "name" | "origin" | "category" = "id";

  @Emit()
  select(selectedItems: HSMDataTableItem[]) {}
  @Emit()
  cancel() {}

  get ingredients() {
    return FoodIngredientModule.foodIngredients;
  }

  async fetchEntriesAsync() {
    return FoodIngredientModule.fetchFoodIngredientsAsync();
  }

  mapTableItems(ingredient: HSMFoodIngredient) {
    return {
      id: ingredient.id,
      name: ingredient.name,
      origin: ingredient.origin ?? null,
      category: ingredient.category ?? null,
      action: {
        edit: true,
        delete: true,
      },
      raw: ingredient,
    };
  }

  formatOrigin(origin: string | null, key: string, item: any) {
    return origin ?? "---";
  }

  formatCategory(
    category: HSMFoodIngredientCategory | null,
    key: string,
    item: any
  ) {
    return category ? category.name : "---";
  }
}
</script>

<style lang="scss" scoped></style>
>
