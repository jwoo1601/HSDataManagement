<template>
  <b-dropdown
    v-bind="$attrs"
    v-on="$listeners"
    :class="finalDropdownClass"
    @mouseenter="toggleHoverState(true)"
    @mouseleave="toggleHoverState(false)"
    @focus="focusEffect && toggleHoverState(true)"
    @blur="focusEffect && toggleHoverState(false)"
    squared
  >
    <template v-slot:button-content>
      <b-icon
        :icon="icon"
        :variant="finalIconVariant"
        :class="finalIconClass"
      ></b-icon>
      <span :class="finalTextClass">{{ text }}</span>
    </template>

    <template v-slot:default>
      <slot></slot>
    </template>
  </b-dropdown>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import "reflect-metadata";
import { Prop } from "vue-property-decorator";

import { BDropdown, BIcon } from "bootstrap-vue";

@Component({
  components: {
    BDropdown,
    BIcon,
  },
})
export default class HSMButton extends Vue {
  @Prop() icon!: string;
  @Prop() iconVariant?: string;
  @Prop() iconClass?: string | object;
  @Prop({ default: "dropdown" }) text!: string;
  @Prop({ default: "light" }) textVariant!: string;
  @Prop({ default: null }) hoverTextVariant!: string;
  @Prop({ default: null }) hoverVariant!: string;
  @Prop({ default: "normal" }) fontWeight!: string;
  @Prop({ default: "2" }) spacing!: string;
  @Prop({ default: true }) overrideStyle!: boolean;
  @Prop({ default: false }) focusEffect!: boolean;

  data() {
    return {
      hover: 0,
    };
  }

  get finalIconVariant() {
    return this.iconVariant ? this.iconVariant : this.textVariant;
  }

  get finalIconClass() {
    return [
      this.iconClass,
      `mr-${this.spacing}`,
      "btn-icon",
      {
        [`text-${this.hoverTextVariant}`]: this.$data.hover,
      },
    ];
  }

  get finalTextClass() {
    return [
      `text-${this.textVariant}`,
      `font-weight-${this.fontWeight}`,
      {
        [`text-${this.hoverTextVariant}`]: this.$data.hover,
      },
    ];
  }

  get finalDropdownClass() {
    return [
      `text-${this.textVariant}`,
      `font-weight-${this.fontWeight}`,
      {
        [`text-${this.hoverTextVariant}`]: this.$data.hover,
        [`bg-${this.hoverVariant}`]: this.$data.hover,
        "shadow-none": this.overrideStyle,
      },
    ];
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
