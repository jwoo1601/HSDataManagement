<template>
  <b-row class="mt-5">
    <b-col cols="8" offset="2" md="6" offset-md="3" lg="4" offset-lg="4">
      <b-form
        name="login"
        class="px-4 py-3 border border-2 border-light shadow"
      >
        <div class="h3 my-3 text-center font-weight-bold">
          <b-icon icon="lock-fill" class="mr-2"></b-icon>
          {{ $t("title.login") }}
        </div>
        <b-form-group
          class="mt-5"
          id="group-username"
          :label="$t('title.id')"
          label-for="input-username"
          :state="usernameState"
        >
          <b-form-input
            id="input-username"
            type="text"
            v-model="credentials.username"
            :placeholder="$t('title.id')"
            :state="usernameState"
            required
            trim
          ></b-form-input>
        </b-form-group>
        <b-form-group
          class="mt-3"
          id="group-password"
          :label="$t('title.password')"
          label-for="input-password"
          :state="passwordState"
        >
          <b-form-input
            id="input-password"
            type="password"
            v-model="credentials.password"
            :placeholder="$t('title.password')"
            :state="passwordState"
            required
          ></b-form-input>
        </b-form-group>
        <b-form-checkbox
          class="mt-3"
          v-model="rememberLogin"
          name="RememberLogin"
          switch
        >
          {{ $t("action.rememberMe") }}
        </b-form-checkbox>

        <hsm-button
          icon="arrow-right-square-fill"
          variant="main"
          textVariant="light"
          fontWeight="bold"
          class="w-100 mt-4"
          @click="handleLogin()"
        >
          {{ $t("title.login") }}
        </hsm-button>
      </b-form>
    </b-col>
  </b-row>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

import { UserCredentials } from "@/models/api/Account";
import AppModule from "@/store/modules/app";
import AccountModule from "@/store/modules/account";
import { Prop } from "vue-property-decorator";

@Component({
  components: {},
})
export default class Login extends Vue {
  @Prop({ default: null })
  redirectUrl?: string | null;

  credentials: UserCredentials = {};
  rememberLogin = false;

  get formState() {
    return this.usernameState && this.passwordState;
  }

  get usernameState() {
    if (!this.credentials.username) {
      return null;
    }

    return this.credentials.username.length > 0;
  }

  get passwordState() {
    if (!this.credentials.password) {
      return null;
    }

    return this.credentials.password.length >= 8;
  }

  async handleLogin() {
    if (this.formState) {
      AppModule.showLoading(this.$t("loading.login").toString());

      const result = await AccountModule.loginAsync(this.credentials);

      AppModule.hideLoading();

      if (result.success) {
        if (this.redirectUrl) {
          this.$router.push(this.redirectUrl);
        } else {
          this.$router.back();
        }
      } else {
        AppModule.showErrorDialog({
          title: this.$t("error.login").toString(),
          message: this.$t(result.errorMessage as string, {
            message: result.payload,
          }).toString(),
        });
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>
>
