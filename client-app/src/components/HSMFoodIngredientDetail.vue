<template>
  <transition
    name="food-ingredient-detail"
    enter-active-class="animate__animated animate__slideInUp animate__fast"
    leave-active-class="animate__animated animate__slideOutDown animate__fast"
  >
    <div class="food-ingredient-detail">
      <hsm-item-detail :header="header" @close="handleClose()">
        <b-form
          name="foodIngredientDetail"
          class="px-4 py-4 border border-2 border-light"
        >
          <b-form-row v-if="ingredient" class="mt-1">
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
                label="재료명"
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
            <b-col cols="12" md="10" lg="8" xl="6">
              <b-form-group
                id="group-origin"
                label="원산지"
                label-for="input-origin"
              >
                <b-form-input
                  id="input-origin"
                  type="text"
                  v-model="input.origin"
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
          <b-form-row class="mt-1" v-if="ingredient">
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
                v-if="ingredient"
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
      <hsm-food-ingredient-category-selection-dialog
        :visible.sync="categorySelectionDialogVisible"
        @select="handleCategorySelections"
        @cancel="handleCategorySelectionDialogClosed"
      ></hsm-food-ingredient-category-selection-dialog>
    </div>
  </transition>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Emit, Inject, Prop, Watch } from "vue-property-decorator";
import "animate.css";
import AppModule from "@/store/modules/app";
import FoodIngredientModule from "@/store/modules/foodIngredient";
import FoodIngredientCategoryModule from "@/store/modules/foodIngredientCategory";
import HSMResponseModel from "@/models/api/ResponseModel";
import Success from "@/views/Success.vue";
import {
  HSMFoodIngredient,
  HSMFoodIngredientCategory,
  HSMFoodIngredientInputModel,
} from "@/models/api/Food";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
} from "./HSMDataTable.vue";
import HSMFoodIngredientCategorySelectionDialog from "@/components/HSMFoodIngredientCategorySelectionDialog.vue";

interface FoodIngredientDetailInput {
  id?: number;
  name?: string;
  origin?: string;
  category?: HSMFoodIngredientCategory;
}

@Component({
  components: {
    "hsm-food-ingredient-category-selection-dialog": HSMFoodIngredientCategorySelectionDialog,
  },
})
export default class HSMFoodIngredientDetail extends Vue {
  @Prop({ default: null })
  readonly ingredient!: HSMFoodIngredient | null;

  input: FoodIngredientDetailInput = {};
  categorySelectionDialogVisible = false;

  @Watch("ingredient")
  mapIngredientToInput() {
    if (this.ingredient) {
      this.input = {
        ...this.ingredient,
      };
    }
  }

  @Emit()
  save() {}

  @Emit()
  delete() {}

  @Emit()
  close() {
    this.input = {};
  }

  openCategorySelectionDialog() {
    this.categorySelectionDialogVisible = true;
  }

  get categoryInfo() {
    return `[${this.input.category?.id}] ${this.input.category?.name}`;
  }

  get createdAt() {
    return this.ingredient?.created_at.toLocaleString("ko-KR");
  }

  get lastUpdatedAt() {
    return this.ingredient?.last_updated_at.toLocaleString("ko-KR");
  }

  async handleSave() {
    AppModule.showLoading("저장 중");

    const inputModel: HSMFoodIngredientInputModel = {
      name: this.input.name as string,
      origin: this.input.origin,
      category: this.input.category?.id as number,
    };

    let response: HSMResponseModel<HSMFoodIngredient>;
    if (this.ingredient) {
      response = await FoodIngredientModule.editFoodIngredientAsync({
        id: this.ingredient.id,
        inputModel,
      });
    } else {
      response = await FoodIngredientModule.addFoodIngredientAsync(inputModel);
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
      "재료 제거",
      "정말로 해당 재료를 제거하시겠습니까?"
    );

    if (confirmed && this.ingredient) {
      AppModule.showLoading();

      const success = await FoodIngredientModule.deleteFoodIngredientAsync(
        this.ingredient.id
      );

      AppModule.hideLoading();

      if (success) {
        this.delete();
        this.close();
      } else {
        AppModule.showErrorDialog({
          title: "오류",
          message:
            "재료를 제거하는 중에 오류가 발생했습니다. 잠시 후 다시 시도해주세요.",
        });
      }
    }
  }

  handleClose() {
    this.close();
  }

  handleCategorySelections(selectedItems: HSMDataTableItem[]) {
    this.input.category = selectedItems[0].raw as HSMFoodIngredientCategory;
  }

  handleCategorySelectionDialogClosed() {}

  get header() {
    return this.ingredient
      ? `재료 ${this.ingredient.name}의 정보`
      : "재료 추가";
  }
}
</script>

<style lang="scss" scoped></style>
>
