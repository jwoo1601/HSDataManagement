<template>
  <div id="service-management">
    <b-row class="px-3 pt-2">
      <b-col>
        <b-card no-body>
          <b-tabs card v-model="tabIndex">
            <b-tab title="서비스" href="#service">
              <service-view ref="service"></service-view>
            </b-tab>
            <b-tab title="서비스 그룹" href="#service-group">
              <service-group-view ref="serviceGroup"></service-group-view>
            </b-tab>
          </b-tabs>
        </b-card>
      </b-col>
    </b-row>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import ServiceView from "@/views/Service.vue";
import ServiceGroupView from "@/views/ServiceGroup.vue";
import { NavigationGuardNext, Route } from "vue-router";
import ServiceModule from "@/store/modules/service";
import ServiceGroupModule from "@/store/modules/serviceGroup";
import { Ref } from "vue-property-decorator";

@Component({
  components: {
    "service-view": ServiceView,
    "service-group-view": ServiceGroupView,
  },
})
export default class ServiceManagement extends Vue {
  readonly tabs = ["#service", "#service-group"];
  tabIndex = 0;

  @Ref()
  service!: ServiceView;
  @Ref()
  serviceGroup!: ServiceGroupView;

  selectTabFromRoute(route: Route) {
    const index = this.tabs.findIndex((tab) => tab === route.hash);
    if (index !== -1) {
      this.tabIndex = index;
    }
  }

  async mounted() {
    this.selectTabFromRoute(this.$route);
  }

  beforeRouteUpdate(to: Route, from: Route, next: NavigationGuardNext) {
    this.selectTabFromRoute(to);
    next();
  }
}
</script>

<style lang="scss" scoped></style>
>
