<template>
  <hsm-data-table
    header-icon="egg-fried"
    header="음식"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="foods"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria="criteria"
    :criteria-options="criteriaOptions"
  >
    <template #cell(note)="data">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`food-showNote-${data.index}`"
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
          v-if="data.value !== null"
          :target="`food-showNote-${data.index}`"
          placement="top"
          variant="success"
        >
          {{ data.value }}
        </b-tooltip>
      </div>
    </template>

    <template #cell(category)="{ value, unformatted }">
      <div class="d-flex justify-content-center text-center pt-1">
        <b-link :to="{ path: unformatted.name }">{{ value }}</b-link>
      </div>
    </template>

    <template #cell(ingredients)="{ value, index }">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`food-ingredients-${index}`"
          class="mr-3"
          size="sm"
          icon="egg-fill"
          variant="outline-submain"
          textVariant="submain"
          hoverVariant="submain"
          hoverTextVariant="light"
          fontWeight="bold"
        >
          재료
        </hsm-button>
        <b-tooltip
          v-if="value !== null"
          :target="`food-ingredients-${index}`"
          placement="top"
          variant="submain"
        >
          {{ value }}
        </b-tooltip>
      </div>
    </template>

    <template #detail="{ visible, item, notify, close }">
      <hsm-food-detail
        v-show="visible"
        :service="item"
        @save="notify"
        @delete="notify"
        @close="close"
      ></hsm-food-detail>
    </template>
  </hsm-data-table>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import HSMFood, { HSMFoodCategory, HSMFoodIngredient } from "@/models/api/Food";
import AppModule from "@/store/modules/app";
import FoodModule from "@/store/modules/food";
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
import HSMFoodDetail from "@/components/HSMServiceDetail.vue";

interface FoodTableItem extends HSMDataTableItem {
  note: string | null;
  category: HSMFoodCategory | null;
  ingredients: string | null;
}

@Component({
  components: {
    "hsm-food-detail": HSMFoodDetail,
  },
})
export default class Food extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
    {
      key: "category",
      label: "카테고리",
      sortable: true,
      formatter: this.formatFoodCategory,
      hasCustomRenderer: true,
    },
    {
      key: "ingredients",
      label: "재료",
      searchable: false,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "음식",
  };

  criteria: "id" | "name" | "category" | "numIngredients" = "id";
  criteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "category", text: "카테고리" },
    { value: "numIngredients", text: "재료 수" },
  ];

  get foods() {
    return FoodModule.foods;
  }

  async fetchEntriesAsync() {
    return FoodModule.fetchFoodsAsync();
  }

  async deleteEntryAsync(entry: HSMFood) {
    return FoodModule.deleteFoodAsync(entry.id);
  }

  mapTableItems(food: HSMFood): FoodTableItem {
    return {
      id: food.id,
      name: food.name,
      note: food.note ?? null,
      category: food.category ?? null,
      ingredients:
        food.ingredients.length > 0 ? food.ingredients.join(", ") : null,
      action: {
        edit: true,
        delete: true,
      },
      raw: food,
    };
  }

  formatFoodCategory(
    foodCategory: HSMFoodCategory | null,
    key: string,
    item: any
  ) {
    return foodCategory ? foodCategory.name : "---";
  }
}
</script>

<style lang="scss" scoped></style>
>
