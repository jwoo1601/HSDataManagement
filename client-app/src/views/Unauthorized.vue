<template>
  <hsm-error
    title="페이지 접근 권한이 없습니다"
    message="해당 페이지에 접근할 권한이 없습니다."
  ></hsm-error>
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
