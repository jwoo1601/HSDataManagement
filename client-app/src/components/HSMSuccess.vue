<template>
  <div id="success">
    <b-row class="pt-4">
      <b-col>
        <b-alert class="px-5 py-4" show variant="success">
          <h2 class="font-weight-bold ml-2">
            <b-icon :icon="titleIcon" class="mr-2"></b-icon>
            {{ title }}
          </h2>
          <hr />
          <p>{{ message }}</p>
          <b-link v-if="showLink" :href="linkUrl">{{ linkText }}</b-link>
        </b-alert>
      </b-col>
    </b-row>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { BRow, BCol, BAlert, BLink } from "bootstrap-vue";
import { Prop } from "vue-property-decorator";

@Component({
  components: { BRow, BCol, BAlert, BLink },
})
export default class HSMSuccess extends Vue {
  @Prop({ default: "성공" })
  title!: string;
  @Prop({ default: "check-circle-fill" })
  titleIcon!: string;
  @Prop({ default: null })
  message!: string;
  @Prop({ default: false })
  showLink!: boolean;
  @Prop({ default: null })
  linkText!: string;
  @Prop({ default: null })
  linkUrl!: string;
}
</script>

<style lang="scss" scoped></style>>
