<template>
  <hsm-error
    title="404 Page Not Found"
    message="요청하신 페이지를 찾을 수 없습니다."
  ></hsm-error>
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

<style lang="scss" scoped></style>>
