<template>
  <div class="dashboard">
    <b-row class="px-5">
      <b-col cols="8" offset="2">
        <h2 class="font-weight-bold d-inline-block">
          <b-icon icon="server" class="mr-3"></b-icon>
          {{ $t("menu.dashboard") }}
        </h2>
      </b-col>
    </b-row>
    <hr />
    <b-row>
      <b-col cols="2" offset="1" class="pt-4" id="side-menu">
        <b-list-group>
          <b-list-group-item
            :to="{ name: 'SecurityCode' }"
            active-class="bg-main text-white"
          >
            <b-icon icon="shield-lock" class="mr-2"></b-icon>
            {{ $t("menu.securityCode") }}
          </b-list-group-item>
          <b-list-group-item
            :to="{ name: 'User' }"
            active-class="bg-main text-white"
          >
            <b-icon icon="people" class="mr-2"></b-icon>
            {{ $t("menu.user") }}
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
export default class Dashboard extends Vue {}
</script>

<style lang="scss" scoped></style>
>
