<template>
  <b-row>
    <b-col cols="12" offset="0" md="10" offset-md="1" lg="8" offset-lg="2">
      <hsm-error
        :title="$t('title.notFound')"
        :message="$t('message.notFound')"
      ></hsm-error>
    </b-col>
  </b-row>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { BRow, BCol, BAlert } from "bootstrap-vue";

@Component({
  components: { BRow, BCol, BAlert },
})
export default class NotFound extends Vue {}
</script>

<style lang="scss" scoped></style>
>
