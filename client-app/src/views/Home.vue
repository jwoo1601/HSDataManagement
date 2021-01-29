<template>
  <div class="home">
    <b-row class="pt-4">
      <b-col cols="12" offset="0" md="10" offset-md="1" lg="8" offset-lg="2">
        <b-jumbotron header-level="4">
          <template #header>
            {{ $t("title.hsm") }}
          </template>

          <template #lead>
            {{ $t("message.aboutHSM[0]") }}
          </template>

          <hr class="my-3" />

          <p class="mb-4">
            {{ $t("message.aboutHSM[1]") }}
          </p>
          <hsm-button
            href="https://www.sample.hsadmin.net"
            icon="info-square"
            variant="outline-main"
            text-variant="main"
          >
            {{ $t("label.visitSampleWebsite") }}
          </hsm-button>
        </b-jumbotron>

        <hsm-post-list
          class="mt-5"
          :header="$t('title.announcements')"
          :collapsible="true"
        ></hsm-post-list>
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
export default class Home extends Vue {}
</script>

<style lang="scss" scoped></style>
