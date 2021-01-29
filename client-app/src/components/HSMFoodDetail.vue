<template>
  <transition
    name="food-detail"
    enter-active-class="animate__animated animate__slideInUp animate__fast"
    leave-active-class="animate__animated animate__slideOutDown animate__fast"
  >
    <div class="food-detail">
      <hsm-item-detail :header="header" @close="handleClose()">
        <b-form
          name="foodDetail"
          class="px-4 py-4 border border-2 border-light"
        >
          <b-form-row v-if="food" class="mt-1">
            <b-col cols="9" md="7" lg="5" xl="3">
              <b-form-group id="group-id" label="연번" label-for="input-id">
                <b-form-input
                  id="input-id"
                  type="text"
                  v-model="input.id"
                  number
                  readonly
                ></b-form-input>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" md="10" lg="8" xl="6">
              <b-form-group
                id="group-name"
                label="음식명"
                label-for="input-name"
              >
                <b-form-input
                  id="input-name"
                  type="text"
                  v-model="input.name"
                  required
                  trim
                ></b-form-input>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" xl="8">
              <b-form-group
                id="group-category"
                label="카테고리"
                label-for="input-category"
              >
                <div
                  v-if="input.category !== undefined"
                  class="d-inline-block px-3 py-2 mb-3 bg-main text-white h5 rounded"
                >
                  <b-icon icon="collection-fill" class="mr-2"></b-icon>
                  {{ categoryInfo }}
                </div>
                <hsm-button
                  id="input-category"
                  class="d-block"
                  icon="list-check"
                  variant="outline-success"
                  textVariant="success"
                  hoverVariant="success"
                  hoverTextVariant="light"
                  fontWeight="bold"
                  spacing="1"
                  @click="openCategorySelectionDialog()"
                >
                  카테고리 선택
                </hsm-button>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" xl="8">
              <b-form-group
                id="group-ingredients"
                label="재료"
                label-for="input-ingredients"
              >
                <b-form-row>
                  <b-col>
                    <hsm-data-table-lite
                      header="음식 재료"
                      :field-definitions="ingredientFieldDefinitions"
                      :entries.sync="input.ingredients"
                      :entries-mapper="mapIngredientTableItems"
                    ></hsm-data-table-lite>
                  </b-col>
                </b-form-row>

                <hsm-button
                  id="input-ingredients"
                  class="d-block"
                  icon="list-check"
                  variant="outline-success"
                  textVariant="success"
                  hoverVariant="success"
                  hoverTextVariant="light"
                  fontWeight="bold"
                  spacing="1"
                  @click="openIngredientSelectionDialog()"
                >
                  재료 선택
                </hsm-button>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1" v-if="food">
            <b-col>
              <b-form-group
                id="group-created-at"
                label="최초 생성일"
                label-for="input-created-at"
              >
                <b-form-input
                  id="input-created-at"
                  type="text"
                  :value="createdAt"
                  readonly
                ></b-form-input>
              </b-form-group>
            </b-col>
            <b-col>
              <b-form-group
                id="group-last-updated-at"
                label="최근 변경일"
                label-for="input-last-updated-at"
              >
                <b-form-input
                  id="input-last-updated-at"
                  type="text"
                  :value="lastUpdatedAt"
                  readonly
                ></b-form-input>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" lg="10">
              <b-form-group id="group-note" label="노트" label-for="input-note">
                <b-form-textarea
                  id="input-note"
                  v-model="input.note"
                  required
                  trim
                  no-resize
                  rows="4"
                ></b-form-textarea>
              </b-form-group>
            </b-col>
          </b-form-row>

          <b-form-row class="mt-4">
            <b-col class="d-flex justify-content-end">
              <hsm-button
                icon="person-check-fill"
                variant="success"
                textVariant="light"
                fontWeight="bold"
                @click="handleSave()"
              >
                저장
              </hsm-button>
              <hsm-button
                v-if="food"
                icon="person-x-fill"
                variant="danger"
                textVariant="light"
                fontWeight="bold"
                class="ml-3"
                @click="handleDelete()"
              >
                제거
              </hsm-button>

              <hsm-button
                icon="x-square-fill"
                variant="secondary"
                textVariant="light"
                fontWeight="bold"
                class="ml-3"
                @click="handleClose()"
              >
                취소
              </hsm-button>
            </b-col>
          </b-form-row>
        </b-form>
      </hsm-item-detail>

      <hsm-food-category-selection-dialog
        :visible.sync="categorySelectionDialogVisible"
        @select="handleCategorySelections"
        @cancel="handleCategorySelectionDialogClosed"
      ></hsm-food-category-selection-dialog>
      <hsm-food-ingredient-selection-dialog
        :visible.sync="ingredientSelectionDialogVisible"
        @select="handleIngredientSelections"
        @cancel="handleIngredientSelectionDialogClosed"
      ></hsm-food-ingredient-selection-dialog>
    </div>
  </transition>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Emit, Inject, Prop, Watch } from "vue-property-decorator";
import "animate.css";
import AppModule from "@/store/modules/app";
import FoodModule from "@/store/modules/food";
import FoodCategoryModule from "@/store/modules/foodCategory";
import HSMResponseModel from "@/models/api/ResponseModel";
import Success from "@/views/Success.vue";
import HSMFood, {
  HSMFoodCategory,
  HSMFoodIngredient,
  HSMFoodIngredientCategory,
  HSMFoodInputModel,
} from "@/models/api/Food";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
} from "./HSMDataTable.vue";
import { HSMDataTableLiteFieldDefinition } from "@/components/HSMDataTableLite.vue";
import HSMFoodCategorySelectionDialog from "@/components/HSMFoodCategorySelectionDialog.vue";
import HSMFoodIngredientSelectionDialog from "@/components/HSMFoodIngredientSelectionDialog.vue";

interface FoodDetailInput {
  id?: number;
  name?: string;
  category?: HSMFoodCategory;
  ingredients: HSMFoodIngredient[];
  note?: string;
}

@Component({
  components: {
    "hsm-food-category-selection-dialog": HSMFoodCategorySelectionDialog,
    "hsm-food-ingredient-selection-dialog": HSMFoodIngredientSelectionDialog,
  },
})
export default class HSMFoodDetail extends Vue {
  @Prop({ default: null })
  readonly food!: HSMFood | null;

  readonly ingredientFieldDefinitions: HSMDataTableLiteFieldDefinition[] = [
    {
      key: "origin",
      label: "원산지",
      formatter: this.formatIngredientOrigin,
    },
    {
      key: "category",
      label: "카테고리",
      formatter: this.formatIngredientCategory,
    },
  ];

  input: FoodDetailInput = {
    ingredients: [],
  };
  categorySelectionDialogVisible = false;
  ingredientSelectionDialogVisible = false;

  @Watch("food")
  mapFoodToInput() {
    if (this.food) {
      this.input = {
        ...this.food,
      };
    }
  }

  @Emit()
  save() {}

  @Emit()
  delete() {}

  @Emit()
  close() {
    this.input = {
      ingredients: [],
    };
  }

  openCategorySelectionDialog() {
    this.categorySelectionDialogVisible = true;
  }

  openIngredientSelectionDialog() {
    this.ingredientSelectionDialogVisible = true;
  }

  get categoryInfo() {
    return `[${this.input.category?.id}] ${this.input.category?.name}`;
  }

  get createdAt() {
    return this.food?.created_at.toLocaleString("ko-KR");
  }

  get lastUpdatedAt() {
    return this.food?.last_updated_at.toLocaleString("ko-KR");
  }

  async handleSave() {
    AppModule.showLoading("저장 중");

    const inputModel: HSMFoodInputModel = {
      name: this.input.name as string,
      note: this.input.note,
      category: this.input.category?.id as number,
      ingredients: this.input.ingredients.map((ig) => ig.id),
    };

    let response: HSMResponseModel<HSMFood>;
    if (this.food) {
      response = await FoodModule.editFoodAsync({
        id: this.food.id,
        inputModel,
      });
    } else {
      response = await FoodModule.addFoodAsync(inputModel);
    }

    AppModule.hideLoading();

    if (response.isSuccess) {
      this.close();
      this.save();
    } else {
      AppModule.showErrorDialog({
        title: "저장 오류",
        message: response.errorMessage as string,
      });
    }
  }

  async handleDelete() {
    const confirmed = await this.$hsmConfirmDialog(
      "음식 제거",
      "정말로 해당 음식을 제거하시겠습니까?"
    );

    if (confirmed && this.food) {
      AppModule.showLoading();

      const success = await FoodModule.deleteFoodAsync(this.food.id);

      AppModule.hideLoading();

      if (success) {
        this.delete();
        this.close();
      } else {
        AppModule.showErrorDialog({
          title: "오류",
          message:
            "음식을 제거하는 중에 오류가 발생했습니다. 잠시 후 다시 시도해주세요.",
        });
      }
    }
  }

  handleClose() {
    this.close();
  }

  handleCategorySelections(selectedItems: HSMDataTableItem[]) {
    this.input.category = selectedItems[0].raw as HSMFoodCategory;
  }

  handleIngredientSelections(selectedItems: HSMDataTableItem[]) {
    this.input.ingredients = selectedItems.map(
      (it) => it.raw as HSMFoodIngredient
    );
  }

  handleCategorySelectionDialogClosed() {}

  handleIngredientSelectionDialogClosed() {}

  get header() {
    return this.food ? `음식 ${this.food.name}의 정보` : "음식 추가";
  }

  mapIngredientTableItems(ingredient: HSMFoodIngredient) {
    return {
      id: ingredient.id,
      name: ingredient.name,
      origin: ingredient.origin ?? null,
      category: ingredient.category ?? null,
      action: {
        delete: true,
      },
      raw: ingredient,
    };
  }

  formatIngredientOrigin(origin: string | null, key: string, item: any) {
    return origin ?? "---";
  }

  formatIngredientCategory(
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
