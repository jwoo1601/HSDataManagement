<template>
  <b-button
    :class="finalButtonClass"
    :style="{ fontWeight }"
    @mouseenter="toggleHoverState(true)"
    @mouseleave="toggleHoverState(false)"
    @focus="focusEffect && toggleHoverState(true)"
    @blur="focusEffect && toggleHoverState(false)"
    squared
    v-bind="$attrs"
    v-on="$listeners"
  >
    <b-icon
      :icon="icon"
      :variant="finalIconVariant"
      :class="finalIconClass"
    ></b-icon>
    <slot></slot>
  </b-button>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import "reflect-metadata";
import { Prop } from "vue-property-decorator";

import { BButton, BIcon } from "bootstrap-vue";

type HSMTheme =
  | "primary"
  | "secondary"
  | "success"
  | "warning"
  | "info"
  | "error"
  | "light"
  | "dark"
  | "main"
  | "submain";

@Component({
  components: {
    BButton,
    BIcon,
  },
})
export default class HSMButton extends Vue {
  @Prop() icon!: string;
  @Prop() iconVariant?: string;
  @Prop() iconClass?: string | object;
  @Prop({ default: "light" }) textVariant!: string;
  @Prop({ default: null }) hoverTextVariant!: string;
  @Prop({ default: null }) hoverVariant!: string;
  @Prop({ default: "normal" }) fontWeight!: string | number;
  @Prop({ default: "2" }) spacing!: number | string;
  @Prop({ default: true }) overrideStyle!: boolean;
  @Prop({ default: false }) focusEffect!: boolean;

  hover = 0;

  get finalIconVariant() {
    if (this.hover > 0) {
      return this.hoverTextVariant ?? this.iconVariant;
    } else {
      return this.iconVariant ?? this.textVariant;
    }
  }

  get finalIconClass() {
    return [this.iconClass, `mr-${this.spacing}`, "btn-icon"];
  }

  get finalButtonClass() {
    if (this.hover > 0) {
      return [`text-${this.hoverTextVariant}`, `bg-${this.hoverVariant}`];
    } else {
      return [
        `text-${this.textVariant}`,
        {
          "shadow-none": this.overrideStyle,
        },
      ];
    }
  }

  toggleHoverState(hover: boolean) {
    if (hover) {
      this.$data.hover += 1;
    } else {
      this.$data.hover -= 1;
    }
  }
}
</script>

<style lang="scss" scoped></style>>
