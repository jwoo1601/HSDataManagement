<template>
  <hsm-data-table
    header-icon="collection-fill"
    :header="$t('tab.foodCategory')"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="foodCategories"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria-options="criteriaOptions"
  >
    <template #cell(note)="{ value, index }">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`foodCategory-showNote-${index}`"
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
          :target="`foodCategory-showNote-${index}`"
          placement="top"
          variant="success"
        >
          {{ value }}
        </b-tooltip>
      </div>
    </template>

    <template #detail="{ visible, item, notify, close }">
      <hsm-food-category-detail
        v-show="visible"
        :food-category="item"
        @save="notify"
        @delete="notify"
        @close="close"
      ></hsm-food-category-detail>
    </template>
  </hsm-data-table>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { HSMFoodCategory } from "@/models/api/Food";
import AppModule from "@/store/modules/app";
import FoodModule from "@/store/modules/food";
import FoodCategoryModule from "@/store/modules/foodCategory";
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
import HSMFoodCategoryDetail from "@/components/HSMFoodCategoryDetail.vue";

interface FoodCategoryTableItem extends HSMDataTableItem {
  note: string | null;
  numFoods: number;
}

@Component({
  components: {
    "hsm-food-category-detail": HSMFoodCategoryDetail,
  },
})
export default class FoodCategory extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "numFoods", label: "field.numFoods", sortable: true },
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
    { value: "numFoods", text: "field.numFoods" },
  ];

  get foodCategories() {
    return FoodCategoryModule.foodCategories;
  }

  async fetchEntriesAsync() {
    return FoodCategoryModule.fetchFoodCategoriesAsync();
  }

  async deleteEntryAsync(entry: HSMFoodCategory) {
    return FoodCategoryModule.deleteFoodCategoryAsync(entry.id);
  }

  mapTableItems(category: HSMFoodCategory): FoodCategoryTableItem {
    return {
      id: category.id,
      name: category.name,
      note: category.note ?? null,
      numFoods: FoodModule.foods.filter((f) => f.category?.id === category.id)
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
