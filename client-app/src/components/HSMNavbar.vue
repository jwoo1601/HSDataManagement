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
            {{ $t("label.hsm[0]") }}
            <span class="d-none d-md-inline">{{ $t("label.hsm[1]") }}</span>
          </b-badge>
          <span class="ml-3">{{ $t("label.hsm[2]") }}</span>
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
            >
              {{ $t(item.key) }}
            </b-nav-item>
          </b-navbar-nav>
        </b-collapse>

        <b-navbar-nav class="ml-auto">
          <b-nav-form>
            <hsm-dropdown
              id="dropdown-locale"
              class="mr-3 px-0 py-0"
              icon="globe"
              size="md"
              spacing="0"
              text=""
              variant="outline-submain"
              text-variant="light"
              font-weight="bolder"
              focus-effect
              no-caret
              no-flip
              boundary="window"
            >
              <b-dropdown-item-button
                v-for="locale in supportedLocales"
                :key="locale.code"
                :active="locale.code === currentLocale.code"
                active-class="bg-primary text-light"
                button-class=""
                @click="setCurrentLocaleAsync(locale)"
              >
                {{ locale.name }}
              </b-dropdown-item-button>
            </hsm-dropdown>
          </b-nav-form>
        </b-navbar-nav>

        <b-navbar-nav class="ml-auto d-none d-md-inline">
          <b-nav-form>
            <hsm-button
              v-if="!authenticated"
              icon="lock-fill"
              to="/login"
              size="sm"
              spacing="0"
              variant="outline-light"
              textVariant="light"
              hoverVariant="white"
              hoverTextVariant="submain"
              fontWeight="bold"
              class="mr-3"
              squared
              focusEffect
            ></hsm-button>
            <hsm-button
              v-if="!authenticated"
              icon="pencil-square"
              to="/register"
              size="sm"
              spacing="0"
              variant="outline-light"
              textVariant="light"
              hoverVariant="white"
              hoverTextVariant="submain"
              fontWeight="bold"
              class="mr-2"
              squared
              focusEffect
            ></hsm-button>

            <hsm-dropdown
              id="dropdown-account"
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
              <b-dropdown-divider></b-dropdown-divider>
              <b-dropdown-item-button
                button-class="text-danger"
                @click="handleLogout()"
              >
                {{ $t("menu.logout") }}
              </b-dropdown-item-button>
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
import Locale, { UILocales } from "@/models/Locale";

interface HSMNavItemData {
  id: string;
  key: string;
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
      key: "menu.customer",
      route: "/customer",
    },
    {
      id: "nutrition-support",
      key: "menu.nutritionSupport",
      route: "/nutrition-support",
    },
    {
      id: "employee",
      key: "menu.employee",
      route: "/employee",
    },
    {
      id: "report",
      key: "menu.report",
      route: "/report",
    },
    {
      id: "dashboard",
      key: "menu.dashboard",
      route: "/dashboard",
    },
    // {
    //   id: "test",
    //   key: "menu.test",
    //   route: "/test",
    // },
    {
      id: "login",
      key: "menu.login",
      route: "/login",
      smallScreenOnly: true,
    },
    {
      id: "register",
      key: "menu.register",
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
    AppModule.showLoading(this.$t("loading.logout").toString());

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

  get supportedLocales() {
    return Object.values(UILocales);
  }

  get currentLocale() {
    return AppModule.currentLocale;
  }

  async setCurrentLocaleAsync(locale: Locale) {
    AppModule.showLoading(this.$t("loading.general").toString());

    await AppModule.changeLocaleAsync(locale);

    AppModule.hideLoading();
  }
}
</script>

<style scoped lang="scss">
.hsm-nav-bar {
  font-weight: bold !important;
}
</style>
