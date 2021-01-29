<template>
  <div class="nutritionSupport">
    <b-row class="px-5">
      <b-col cols="8" offset="2">
        <h2 class="font-weight-bold d-inline-block">
          <b-icon icon="person-lines-fill" class="mr-3"></b-icon>
          {{ $t("menu.nutritionSupport") }}
        </h2>
      </b-col>
    </b-row>
    <hr />
    <b-row>
      <b-col cols="2" offset="1" class="pt-4" id="side-menu">
        <b-list-group>
          <b-list-group-item
            :to="{ name: 'ServiceManagement' }"
            active-class="bg-main text-white"
          >
            {{ $t("menu.serviceManagement") }}
          </b-list-group-item>
          <b-list-group-item
            :to="{ name: 'FoodManagement' }"
            active-class="bg-main text-white"
          >
            {{ $t("menu.foodManagement") }}
          </b-list-group-item>
          <b-list-group-item
            :to="{ name: 'MenuManagement' }"
            active-class="bg-main text-white"
          >
            {{ $t("menu.menuManagement") }}
          </b-list-group-item>
        </b-list-group>
      </b-col>
      <b-col cols="8">
        <router-view></router-view>
      </b-col>
    </b-row>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component({
  components: {},
})
export default class NutritionSupport extends Vue {}
</script>

<style lang="scss" scoped>
#side-menu {
  position: sticky;
  top: 0;
}
</style>
>
