<template>
  <div class="food-management">
    <b-row class="px-3 pt-2">
      <b-col>
        <b-card no-body>
          <b-tabs card v-model="tabIndex">
            <b-tab :title="$t('tab.food')" href="#food">
              <food-view ref="food"></food-view>
            </b-tab>
            <b-tab :title="$t('tab.foodCategory')" href="#food-category">
              <food-category-view ref="foodCategory"></food-category-view>
            </b-tab>
            <b-tab :title="$t('tab.foodIngredient')" href="#food-ingredient">
              <food-ingredient-view ref="foodIngredient"></food-ingredient-view>
            </b-tab>
            <b-tab
              :title="$t('tab.foodIngredientCategory')"
              href="#food-ingredient-category"
            >
              <food-ingredient-category-view
                ref="foodIngredientCategory"
              ></food-ingredient-category-view>
            </b-tab>
          </b-tabs>
        </b-card>
      </b-col>
    </b-row>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import FoodView from "@/views/Food.vue";
import FoodCategoryView from "@/views/FoodCategory.vue";
import FoodIngredientView from "@/views/FoodIngredient.vue";
import FoodIngredientCategoryView from "@/views/FoodIngredientCategory.vue";
import { NavigationGuardNext, Route } from "vue-router";
import ServiceModule from "@/store/modules/service";
import ServiceGroupModule from "@/store/modules/serviceGroup";
import { Ref } from "vue-property-decorator";

@Component({
  components: {
    "food-view": FoodView,
    "food-category-view": FoodCategoryView,
    "food-ingredient-view": FoodIngredientView,
    "food-ingredient-category-view": FoodIngredientCategoryView,
  },
})
export default class FoodManagement extends Vue {
  readonly tabs = [
    "#food",
    "#food-category",
    "#food-ingredient",
    "#food-ingredient-category",
  ];
  tabIndex = 0;

  @Ref()
  food!: FoodView;
  @Ref()
  foodCategory!: FoodCategoryView;
  @Ref()
  foodIngredient!: FoodIngredientView;
  @Ref()
  foodIngredientCategory!: FoodIngredientCategoryView;

  selectTabFromRoute(route: Route) {
    const index = this.tabs.findIndex((tab) => tab === route.hash);
    if (index !== -1) {
      this.tabIndex = index;
    }
  }

  async mounted() {
    this.selectTabFromRoute(this.$route);
  }

  beforeRouteUpdate(to: Route, from: Route, next: NavigationGuardNext) {
    this.selectTabFromRoute(to);
    next();
  }
}
</script>

<style lang="scss" scoped></style>
>
