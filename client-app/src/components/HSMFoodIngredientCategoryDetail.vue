<template>
  <transition
    name="food-ingredient-category-detail"
    enter-active-class="animate__animated animate__slideInUp animate__fast"
    leave-active-class="animate__animated animate__slideOutDown animate__fast"
  >
    <div class="food-ingredient-category-detail">
      <hsm-item-detail :header="header" @close="handleClose()">
        <b-form
          name="foodIngredientCategoryDetail"
          class="px-4 py-4 border border-2 border-light"
        >
          <b-form-row v-if="ingredientCategory" class="mt-1">
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
                label="카테고리 이름"
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
          <b-form-row class="mt-1" v-if="ingredientCategory">
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
          <b-form-row class="mt-1" v-if="ingredientCategory">
            <b-col>
              <b-form-group
                id="group-settings"
                label="설정"
                label-for="input-settings"
              >
                <div id="input-settings" class="pl-2 py-2">
                  <hsm-button
                    class="ml-3"
                    icon="journal-plus"
                    variant="outline-primary"
                    textVariant="primary"
                    hoverVariant="primary"
                    hoverTextVariant="light"
                    fontWeight="bold"
                    @click="handleShowRegisteredIngredients"
                  >
                    등록된 재료 보기
                  </hsm-button>
                </div>
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
                v-if="ingredientCategory"
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
      <hsm-food-ingredient-viewer
        v-if="ingredientCategory"
        title="등록된 재료 목록"
        :visible.sync="showRegisteredIngredients"
        :ingredients="registeredIngredients"
        :show-category="false"
      ></hsm-food-ingredient-viewer>
    </div>
  </transition>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Emit, Inject, Prop, Ref, Watch } from "vue-property-decorator";
import "animate.css";
import AppModule from "@/store/modules/app";
import FoodIngredientModule from "@/store/modules/foodIngredient";
import FoodIngredientCategoryModule from "@/store/modules/foodIngredientCategory";
import HSMResponseModel from "@/models/api/ResponseModel";
import Success from "@/views/Success.vue";
import {
  HSMFoodIngredientCategory,
  HSMFoodIngredientCategoryInputModel,
} from "@/models/api/Food";
import HSMFoodIngredientViewer from "@/components/HSMFoodIngredientViewer.vue";

interface FoodIngredientCategoryDetailInput {
  id?: number;
  name?: string;
  note?: string;
}

@Component({
  components: { "hsm-food-ingredient-viewer": HSMFoodIngredientViewer },
})
export default class HSMFoodCategoryDetail extends Vue {
  @Prop({ default: null })
  ingredientCategory?: HSMFoodIngredientCategory;

  input: FoodIngredientCategoryDetailInput = {};
  showRegisteredIngredients = false;

  @Watch("foodIngredientCategory")
  mapFoodIngredientCategoryToInput(newCategory: HSMFoodIngredientCategory) {
    if (newCategory) {
      this.input = {
        ...newCategory,
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

  get createdAt() {
    return this.ingredientCategory?.created_at.toLocaleString("ko-KR");
  }

  get lastUpdatedAt() {
    return this.ingredientCategory?.last_updated_at.toLocaleString("ko-KR");
  }

  async handleSave() {
    AppModule.showLoading("저장 중");

    const inputModel: HSMFoodIngredientCategoryInputModel = {
      name: this.input.name as string,
      note: this.input.note,
    };

    let response: HSMResponseModel<HSMFoodIngredientCategory>;
    if (this.ingredientCategory) {
      response = await FoodIngredientCategoryModule.editFoodIngredientCategoryAsync(
        {
          id: this.ingredientCategory.id,
          inputModel,
        }
      );
    } else {
      response = await FoodIngredientCategoryModule.addFoodIngredientCategoryAsync(
        inputModel
      );
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
      "재료 카테고리 제거",
      "정말로 해당 카테고리를 제거하시겠습니까?"
    );

    if (confirmed && this.ingredientCategory) {
      AppModule.showLoading();

      const success = await FoodIngredientCategoryModule.deleteFoodIngredientCategoryAsync(
        this.ingredientCategory.id
      );

      AppModule.hideLoading();

      if (success) {
        this.delete();
        this.close();
      } else {
        AppModule.showErrorDialog({
          title: "오류",
          message:
            "재료 카테고리를 제거하는 중에 오류가 발생했습니다. 잠시 후 다시 시도해주세요.",
        });
      }
    }
  }

  handleShowRegisteredIngredients() {
    this.showRegisteredIngredients = true;
  }

  handleClose() {
    this.close();
  }

  get header() {
    return this.ingredientCategory
      ? `재료 카테고리 ${this.ingredientCategory.name}의 정보`
      : "재료 카테고리 추가";
  }

  get registeredIngredients() {
    return this.ingredientCategory
      ? FoodIngredientModule.foodIngredients.filter(
          (s) => s.category?.id === this.ingredientCategory?.id
        )
      : [];
  }
}
</script>

<style lang="scss" scoped></style>
>
