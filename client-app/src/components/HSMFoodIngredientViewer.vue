<template>
  <hsm-data-viewer
    ref="dataViewer"
    :visible.sync="syncedVisible"
    :dialog-title="title"
    header-icon="basket2-fill"
    header="재료"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchFoodIngredientEntriesAsync"
    :entries="ingredients"
    :entries-mapper="mapFoodIngredientTableItems"
    :labels="labels"
    :delete-entry-action="deleteFoodIngredientEntryAsync"
    :criteria="foodIngredientCriteria"
    :criteria-options="criteriaOptions"
  ></hsm-data-viewer>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import {
  Emit,
  Inject,
  Prop,
  PropSync,
  Ref,
  Watch,
} from "vue-property-decorator";
import AppModule from "@/store/modules/app";
import FoodIngredientModule from "@/store/modules/foodIngredient";
import { HSMFoodIngredient } from "@/models/api/Food";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "./HSMDataTable.vue";
import HSMDataViewer from "./HSMDataViewer.vue";

interface FoodIngredientViewTableItem extends HSMDataTableItem {
  origin: string | null;
  category?: string | null;
}

@Component({
  components: {},
})
export default class HSMFoodIngredientViewer extends Vue {
  @Prop({ default: "재료 목록" })
  readonly title!: string;
  @Prop({ default: [] })
  readonly ingredients!: HSMFoodIngredient[];
  @Prop({ default: true })
  readonly showCategory!: boolean;

  @PropSync("visible", { default: false })
  syncedVisible!: boolean;

  readonly labels: HSMDataTableLabels = {
    item: "재료",
  };
  readonly foodIngredientFieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "origin", label: "원산지", sortable: true },
  ];
  foodIngredientCriteria: "id" | "name" | "origin" | "category" = "id";
  foodIngredientCriteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "origin", text: "원산지" },
  ];

  get fieldDefinitions() {
    return this.showCategory
      ? this.foodIngredientFieldDefinitions.concat([
          { key: "category", label: "카테고리", sortable: true },
        ])
      : this.foodIngredientFieldDefinitions;
  }

  get criteriaOptions() {
    return this.showCategory
      ? this.foodIngredientCriteriaOptions.concat([
          { value: "category", text: "카테고리" },
        ])
      : this.foodIngredientCriteriaOptions;
  }

  async fetchFoodIngredientEntriesAsync() {
    return FoodIngredientModule.fetchFoodIngredientsAsync();
  }

  async deleteFoodIngredientEntryAsync(entry: HSMFoodIngredient) {
    return FoodIngredientModule.deleteFoodIngredientAsync(entry.id);
  }

  mapFoodIngredientTableItems(
    ingredient: HSMFoodIngredient
  ): FoodIngredientViewTableItem {
    const mapped: FoodIngredientViewTableItem = {
      id: ingredient.id,
      name: ingredient.name,
      origin: ingredient.origin ?? null,
      action: {
        edit: false,
        delete: true,
      },
      raw: ingredient,
    };

    return this.showCategory
      ? { ...mapped, category: ingredient.category?.name ?? null }
      : mapped;
  }
}
</script>

<style lang="scss" scoped></style>
>
