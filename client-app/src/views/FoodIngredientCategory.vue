<template>
  <hsm-data-table
    header-icon="tag-fill"
    :header="$t('tab.foodIngredientCategory')"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="categories"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria-options="criteriaOptions"
  >
    <template #cell(note)="{ value, index }">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`foodIngredientCategory-showNote-${index}`"
          class="mr-3"
          size="sm"
          icon="pencil-fill"
          variant="outline-success"
          textVariant="success"
          hoverVariant="success"
          hoverTextVariant="light"
          fontWeight="bold"
        >
          {{ $t("field.note") }}
        </hsm-button>
        <b-tooltip
          v-if="value != null"
          :target="`foodIngredientCategory-showNote-${index}`"
          placement="top"
          variant="success"
        >
          {{ value }}
        </b-tooltip>
      </div>
    </template>

    <template #detail="{ visible, item, notify, close }">
      <hsm-food-ingredient-category-detail
        v-show="visible"
        :ingredient-category="item"
        @save="notify"
        @delete="notify"
        @close="close"
      ></hsm-food-ingredient-category-detail>
    </template>
  </hsm-data-table>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { HSMFoodIngredientCategory } from "@/models/api/Food";
import AppModule from "@/store/modules/app";
import FoodIngredientModule from "@/store/modules/foodIngredient";
import FoodIngredientCategoryModule from "@/store/modules/foodIngredientCategory";
import {
  BFormSelectOption,
  BTable,
  BvTableCtxObject,
  BvTableFieldArray,
  BvTableFormatterCallback,
} from "bootstrap-vue";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableFilterCriteria,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "@/components/HSMDataTable.vue";
import HSMFoodIngredientCategoryDetail from "@/components/HSMFoodIngredientCategoryDetail.vue";

interface FoodIngredientCategoryTableItem extends HSMDataTableItem {
  note: string | null;
  numIngredients: number;
}

@Component({
  components: {
    "hsm-food-ingredient-category-detail": HSMFoodIngredientCategoryDetail,
  },
})
export default class FoodIngredientCategory extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "numIngredients", label: "field.numIngredients", sortable: true },
    {
      key: "note",
      label: "field.note",
      searchable: false,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "label.category",
  };
  readonly criteriaOptions: HSMDataTableFilterCriteria[] = [
    { value: "id", text: "field.id" },
    { value: "name", text: "field.name" },
    { value: "numIngredients", text: "field.numIngredients" },
  ];

  get categories() {
    return FoodIngredientCategoryModule.foodIngredientCategories;
  }

  async fetchEntriesAsync() {
    return FoodIngredientCategoryModule.fetchFoodIngredientCategoriesAsync();
  }

  async deleteEntryAsync(entry: HSMFoodIngredientCategory) {
    return FoodIngredientCategoryModule.deleteFoodIngredientCategoryAsync(
      entry.id
    );
  }

  mapTableItems(
    category: HSMFoodIngredientCategory
  ): FoodIngredientCategoryTableItem {
    return {
      id: category.id,
      name: category.name,
      note: category.note ?? null,
      numIngredients: FoodIngredientModule.foodIngredients.filter(
        (f) => f.category?.id === category.id
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
