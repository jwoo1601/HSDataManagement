<template>
  <hsm-data-table
    header-icon="egg-fried"
    :header="$t('tab.food')"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="foods"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
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
          {{ $t("field.note") }}
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
          {{ $t("field.ingredient") }}
        </hsm-button>
        <b-tooltip
          v-if="value.length > 0"
          :target="`food-ingredients-${index}`"
          placement="top"
          variant="submain"
        >
          {{ value.map((ig) => `${ig.name} (${ig.origin})`).join(", ") }}
        </b-tooltip>
      </div>
    </template>

    <template #detail="{ visible, item, notify, close }">
      <hsm-food-detail
        v-show="visible"
        :food="item"
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
  HSMDataTableFilterCriteria,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "@/components/HSMDataTable.vue";
import HSMFoodDetail from "@/components/HSMFoodDetail.vue";

interface FoodTableItem extends HSMDataTableItem {
  note: string | null;
  category: HSMFoodCategory | null;
  ingredients: HSMFoodIngredient[];
}

@Component({
  components: {
    "hsm-food-detail": HSMFoodDetail,
  },
})
export default class Food extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    {
      key: "category",
      label: "field.category",
      sortable: true,
      formatter: this.formatFoodCategory,
      hasCustomRenderer: true,
    },
    {
      key: "ingredients",
      label: "field.ingredient",
      searchable: false,
      hasCustomRenderer: true,
    },
    {
      key: "note",
      label: "field.note",
      searchable: false,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "field.food",
  };
  readonly criteriaOptions: HSMDataTableFilterCriteria[] = [
    { value: "id", text: "field.id" },
    { value: "name", text: "field.name" },
    { value: "category", text: "field.category" },
    { value: "numIngredients", text: "field.numIngredients" },
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
      ingredients: food.ingredients,
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
