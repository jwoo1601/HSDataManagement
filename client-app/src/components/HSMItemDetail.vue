<template>
  <div class="hsm-item-detail">
    <b-row class="mx-1 py-3 border-bottom">
      <b-col cols="10" class="px-5">
        <h3 class="font-weight-bold">
          <b-icon :icon="headerIcon" class="mr-4"></b-icon>
          {{ header }}
        </h3>
      </b-col>
      <b-col cols="2" class="px-0 d-flex justify-content-center">
        <hsm-button
          id="back"
          icon="caret-down-fill"
          variant="outline-danger"
          textVariant="danger"
          hoverVariant="danger"
          hoverTextVariant="light"
          :spacing="0"
          class="rounded px-2 pt-2 pb-1"
          @click="close()"
        ></hsm-button>
      </b-col>
    </b-row>
    <b-row class="pt-4">
      <b-col><slot></slot></b-col>
    </b-row>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import "reflect-metadata";
import { Prop, Watch, Emit } from "vue-property-decorator";

@Component({
  components: {},
})
export default class HSMItemDetail extends Vue {
  @Prop({ default: "아이템 디테일" })
  header!: string;
  @Prop({ default: "card-text" })
  headerIcon!: string;

  @Emit()
  close() {}
}
</script>

<style lang="scss" scoped>
#back {
}
</style>
