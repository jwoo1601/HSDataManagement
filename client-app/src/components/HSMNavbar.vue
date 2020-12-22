<template>
  <div class="hsm-nav-bar">
    <b-navbar
      toggleable="md"
      :variant="variant"
      type="dark"
      :sticky="sticky"
      class="shadow-sm border-bottom mb-3"
    >
      <b-container>
        <b-navbar-brand to="/" class="hsm">
          <b-badge variant="light" class="text-main">
            효성
            <span class="d-none d-md-inline">제일건강센터</span>
          </b-badge>
          <span class="ml-3">데이터 관리 시스템</span>
        </b-navbar-brand>

        <b-navbar-toggle target="collapse-nav-items"></b-navbar-toggle>

        <b-collapse id="collapse-nav-items" class="ml-3" is-nav>
          <b-navbar-nav>
            <b-nav-item
              v-for="item in navItems"
              :key="item.id"
              :to="item.route"
              :class="[getMenuItemClass(item), 'hsm']"
              @click="handleClickNavItem(item)"
              >{{ item.name }}
            </b-nav-item>
          </b-navbar-nav>
        </b-collapse>

        <b-navbar-nav class="ml-auto d-none d-md-inline">
          <b-nav-form>
            <hsm-button
              v-if="!authenticated"
              icon="box-arrow-right"
              to="/login"
              size="sm"
              variant="outline-light"
              textVariant="light"
              hoverVariant="white"
              hoverTextVariant="submain"
              fontWeight="bolder"
              class="mr-3"
              squared
              focusEffect
              >로그인</hsm-button
            >
            <hsm-button
              v-if="!authenticated"
              icon="key-fill"
              to="/register"
              size="sm"
              variant="outline-light"
              textVariant="light"
              hoverVariant="white"
              hoverTextVariant="submain"
              fontWeight="bolder"
              class="mr-2"
              squared
              focusEffect
              >회원가입</hsm-button
            >
            <hsm-dropdown
              v-if="authenticated"
              icon="person-fill"
              :text="username"
              size="sm"
              spacing="1"
              variant="light"
              split-variant="light"
              textVariant="submain"
              hoverVariant="main"
              hoverTextVariant="light"
              fontWeight="bolder"
              split
              squared
              focusEffect
            >
              <b-dropdown-text variant="submain">{{ role }}</b-dropdown-text>
              <b-dropdown-divider> </b-dropdown-divider>
              <b-dropdown-item-button
                button-class="text-danger"
                @click="handleLogout()"
                >로그아웃</b-dropdown-item-button
              >
            </hsm-dropdown>
          </b-nav-form>
        </b-navbar-nav>
      </b-container>
    </b-navbar>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch } from "vue-property-decorator";
import AppModule from "@/store/modules/app";
import AccountModule from "@/store/modules/account";

interface HSMNavItemData {
  id: string;
  name: string;
  route?: string;
  smallScreenOnly?: boolean;
  handleClick?(): void;
}

@Component({
  components: {},
})
export default class HSMNavbar extends Vue {
  @Prop()
  variant!: string;
  @Prop({ default: true })
  sticky!: boolean;
  @Prop({ default: true })
  itemHoverEffects!: boolean;
  @Prop({ default: true })
  itemActiveEffects!: boolean;
  @Prop({ default: "dark" })
  type!: string;

  private activeNavItem?: HSMNavItemData;
  private navItems: HSMNavItemData[] = [
    {
      id: "customer",
      name: "고객관리",
      route: "/customer",
    },
    {
      id: "nutrition-support",
      name: "영양지원과",
      route: "/nutrition-support",
    },
    {
      id: "report",
      name: "리포트",
      route: "/report",
    },
    {
      id: "dashboard",
      name: "대시보드",
      route: "/dashboard",
    },
    {
      id: "test",
      name: "테스트",
      route: "/test",
    },
    {
      id: "login",
      name: "로그인",
      route: "/login",
      smallScreenOnly: true,
    },
    {
      id: "register",
      name: "회원가입",
      route: "/register",
      smallScreenOnly: true,
    },
  ];

  private get textVariantFromType() {
    switch (this.type) {
      case "light":
        return "dark";
      case "dark":
        return "light";
    }

    return "light";
  }

  @Watch("activeNavItem")
  getMenuItemClass(item: HSMNavItemData) {
    return {
      "d-md-none": item.smallScreenOnly,
      active: this.activeNavItem ? this.activeNavItem.id == item.id : false,
      "border-show": this.itemHoverEffects,
      "border-transparent": this.itemHoverEffects,
      "border-2": this.itemHoverEffects,
      [`hover-bb-${this.textVariantFromType}`]: this.itemHoverEffects,
      [`active-bb-${this.textVariantFromType}`]: this.itemActiveEffects,
    };
  }

  handleClickNavItem(item: HSMNavItemData) {
    // this.activeNavItem = item;

    if (item.handleClick) {
      item.handleClick();
    }
  }

  async handleLogout() {
    AppModule.showLoading("로그아웃 중");

    AccountModule.logout();
    if (this.$router.currentRoute.name !== "Home") {
      this.$router.push({ name: "Home" });
    }

    AppModule.hideLoading();
  }

  get authenticated() {
    return AccountModule.authenticated;
  }

  get username() {
    return AccountModule.userInfo?.username;
  }

  get role() {
    return AccountModule.userInfo?.role;
  }
}
</script>

<style scoped lang="scss">
.hsm-nav-bar {
  font-weight: bold !important;
}
</style>
