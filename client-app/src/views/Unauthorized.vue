<template>
  <b-row>
    <b-col cols="12" offset="0" md="10" offset-md="1" lg="8" offset-lg="2">
      <hsm-error
        :title="$t('title.unauthorized')"
        :message="$t('message.unauthorized')"
      ></hsm-error>
    </b-col>
  </b-row>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component({
  components: {},
})
export default class Unauthorized extends Vue {}
</script>

<style lang="scss" scoped></style>
>
