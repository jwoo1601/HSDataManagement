<template>
  <hsm-data-viewer
    ref="dataViewer"
    :visible.sync="syncedVisible"
    :dialog-title="title"
    header-icon="egg-fried"
    header="음식"
    :field-definitions="foodFieldDefinitions"
    :fetch-entries-action="fetchFoodEntriesAsync"
    :entries="foods"
    :entries-mapper="mapFoodTableItems"
    :labels="labels"
    :delete-entry-action="deleteFoodEntryAsync"
    :criteria="foodCriteria"
    :criteria-options="foodCriteriaOptions"
  >
    <template #cell(note)="{ index, value }">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`view-food-showNote-${index}`"
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
          :target="`view-food-showNote-${index}`"
          placement="top"
          variant="success"
        >
          {{ value }}
        </b-tooltip>
      </div>
    </template>

    <template #cell(ingredients)="{ value, index }">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`view-food-ingredients-${index}`"
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
          v-if="value.length > 0"
          :target="`view-food-ingredients-${index}`"
          placement="top"
          variant="submain"
        >
          {{ value.join(", ") }}
        </b-tooltip>
      </div>
    </template>
  </hsm-data-viewer>
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
import FoodModule from "@/store/modules/food";
import HSMFood, { HSMFoodIngredient } from "@/models/api/Food";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "./HSMDataTable.vue";
import HSMDataViewer from "./HSMDataViewer.vue";

interface FoodViewTableItem extends HSMDataTableItem {
  note: string | null;
  ingredients: HSMFoodIngredient[];
}

@Component({
  components: {},
})
export default class HSMFoodViewer extends Vue {
  @Prop({ default: "음식 목록" })
  readonly title!: string;
  @Prop({ default: [] })
  readonly foods!: HSMFood[];

  @PropSync("visible", { default: false })
  syncedVisible!: boolean;

  readonly labels: HSMDataTableLabels = {
    item: "음식",
  };
  readonly foodFieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
    {
      key: "ingredients",
      label: "재료",
      searchable: false,
      hasCustomRenderer: true,
    },
  ];
  foodCriteria: "id" | "name" | "numIngredients" = "id";
  foodCriteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "numIngredients", text: "재료 수" },
  ];

  async fetchFoodEntriesAsync() {
    return FoodModule.fetchFoodsAsync();
  }

  async deleteFoodEntryAsync(entry: HSMFood) {
    return FoodModule.deleteFoodAsync(entry.id);
  }

  mapFoodTableItems(food: HSMFood): FoodViewTableItem {
    return {
      id: food.id,
      name: food.name,
      note: food.note ?? null,
      ingredients: food.ingredients,
      action: {
        edit: false,
        delete: true,
      },
      raw: food,
    };
  }
}
</script>

<style lang="scss" scoped></style>
>
