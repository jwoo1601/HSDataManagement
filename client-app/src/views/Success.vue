<template>
  <div id="error">
    <b-row class="pt-4">
      <b-col cols="10" offset="1" sm="8" offset-sm="2" md="6" offset-md="3">
        <b-alert class="px-5 py-3" show variant="danger">
          <h1>{{ title }}</h1>
          <hr />
          <p>{{ message }}</p>
        </b-alert>
      </b-col>
    </b-row>
    <b-row class="mt-3">
      <b-col cols="10" offset="1">
        <div class="d-flex justify-content-center">
          <hsm-button
            icon="arrow-left-square"
            variant="outline-primary"
            textVariant="primary"
            hoverVariant="primary"
            hoverTextVariant="white"
            fontWeight="bold"
            class="mr-3"
            squared
            >이전 페이지로</hsm-button
          >

          <hsm-button
            icon="house-door"
            variant="outline-secondary"
            textVariant="secondary"
            hoverVariant="secondary"
            hoverTextVariant="white"
            fontWeight="bold"
            squared
            >홈으로</hsm-button
          >
        </div>
      </b-col>
    </b-row>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { BRow, BCol, BAlert } from "bootstrap-vue";
import { Prop } from "vue-property-decorator";

@Component({
  components: { BRow, BCol, BAlert },
})
export default class Success extends Vue {
  @Prop({ default: "성공" })
  title!: string;
  @Prop({ default: null })
  message!: string;
}
</script>

<style lang="scss" scoped></style>>
