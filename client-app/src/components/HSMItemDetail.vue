<template>
  <div class="hsm-component">
    <b-row
      ><b-col
        cols="12"
        md="10"
        offset-md="1"
        lg="8"
        offset-lg="2"
        xl="6"
        offset-xl="3"
      >
        <b-row class="py-3 border-bottom"
          ><b-col cols="10" class="px-5">
            <h3 class="font-weight-bold">
              <b-icon icon="card-text" class="mr-4"></b-icon>{{ header }}
            </h3></b-col
          ><b-col cols="2" class="px-0 d-flex justify-content-center">
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
            ></hsm-button></b-col></b-row
        ><b-row class="pt-4"
          ><b-col><slot></slot> </b-col></b-row></b-col
    ></b-row>
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

  @Emit()
  close() {}
}
</script>

<style lang="scss" scoped>
#back {
}
</style>
