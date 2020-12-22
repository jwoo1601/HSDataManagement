<template>
  <div id="error">
    <b-row class="pt-4">
      <b-col>
        <b-alert class="px-5 py-4" show variant="danger">
          <h2 class="font-weight-bold ml-2">
            <b-icon icon="exclamation-triangle-fill" class="mr-2"></b-icon>
            {{ title }}
          </h2>
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
            hoverTextVariant="light"
            fontWeight="bold"
            class="mr-3"
            @click="$router.back()"
            >이전 페이지로</hsm-button
          >

          <hsm-button
            icon="house-door-fill"
            variant="outline-secondary"
            textVariant="secondary"
            hoverVariant="secondary"
            hoverTextVariant="light"
            fontWeight="bold"
            @click="$router.push('/')"
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
import { BRow, BCol, BAlert, BIcon } from "bootstrap-vue";
import { Prop } from "vue-property-decorator";

@Component({
  components: { BRow, BCol, BAlert, BIcon },
})
export default class HSMError extends Vue {
  @Prop({ default: "오류" })
  title!: string;
  @Prop({ default: null })
  message!: string;
}
</script>

<style lang="scss" scoped></style>>
