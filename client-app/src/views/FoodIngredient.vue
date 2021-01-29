<template>
  <hsm-data-table
    header-icon="basket2-fill"
    :header="$t('tab.foodIngredient')"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="ingredients"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria-options="criteriaOptions"
  >
    <template #cell(category)="{ value, unformatted }">
      <div class="d-flex justify-content-center text-center pt-1">
        <b-link :to="{ path: unformatted.name }">{{ value }}</b-link>
      </div>
    </template>

    <template #detail="{ visible, item, notify, close }">
      <hsm-food-ingredient-detail
        v-show="visible"
        :ingredient="item"
        @save="notify"
        @delete="notify"
        @close="close"
      ></hsm-food-ingredient-detail>
    </template>
  </hsm-data-table>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import {
  HSMFoodIngredientCategory,
  HSMFoodIngredient,
} from "@/models/api/Food";
import AppModule from "@/store/modules/app";
import FoodIngredientModule from "@/store/modules/foodIngredient";
import {
  BFormSelectOption,
  BTable,
  BvTableCtxObject,
  BvTableFieldArray,
  BvTableFormatterCallback,
} from "bootstrap-vue";
import App from "@/App.vue";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "@/components/HSMDataTable.vue";
import HSMFoodIngredientDetail from "@/components/HSMFoodIngredientDetail.vue";

interface FoodIngredientTableItem extends HSMDataTableItem {
  origin: string | null;
  category: HSMFoodIngredientCategory | null;
}

@Component({
  components: {
    "hsm-food-ingredient-detail": HSMFoodIngredientDetail,
  },
})
export default class FoodIngredient extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    {
      key: "origin",
      label: "field.origin",
      sortable: true,
      formatter: this.formatOrigin,
    },
    {
      key: "category",
      label: "field.category",
      sortable: true,
      formatter: this.formatCategory,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "label.ingredient",
  };
  readonly criteriaOptions = [
    { value: "id", text: "field.id" },
    { value: "name", text: "field.name" },
    { value: "origin", text: "field.origin" },
    { value: "category", text: "field.category" },
  ];

  get ingredients() {
    return FoodIngredientModule.foodIngredients;
  }

  async fetchEntriesAsync() {
    return FoodIngredientModule.fetchFoodIngredientsAsync();
  }

  async deleteEntryAsync(entry: HSMFoodIngredient) {
    return FoodIngredientModule.deleteFoodIngredientAsync(entry.id);
  }

  mapTableItems(ingredient: HSMFoodIngredient): FoodIngredientTableItem {
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
