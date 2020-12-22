<template>
  <hsm-data-table
    header-icon="collection-fill"
    header="음식 카테고리"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="foodCategories"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria="criteria"
    :criteria-options="criteriaOptions"
  >
    <template #cell(note)="data">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`foodCategory-showNote-${data.index}`"
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
          v-if="data.value != null"
          :target="`foodCategory-showNote-${data.index}`"
          placement="top"
          variant="success"
        >
          {{ data.value }}
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
    { key: "numFoods", label: "등록된 음식", sortable: true },
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "음식 카테고리",
  };

  criteria: "id" | "name" | "numFoods" = "id";
  criteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "numFoods", text: "음식 갯수" },
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
