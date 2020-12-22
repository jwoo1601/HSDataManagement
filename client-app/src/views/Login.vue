<template>
  <b-row class="mt-5">
    <b-col cols="8" offset="2" md="6" offset-md="3" lg="4" offset-lg="4">
      <b-form
        name="login"
        class="px-4 py-3 border border-2 border-light shadow"
      >
        <div class="h3 my-3 text-center font-weight-bold">로그인</div>
        <b-form-group
          class="mt-5"
          id="group-username"
          label="아이디"
          label-for="input-username"
          :state="usernameState"
        >
          <b-form-input
            id="input-username"
            type="text"
            v-model="credentials.username"
            placeholder="아이디"
            :state="usernameState"
            required
            trim
          ></b-form-input>
        </b-form-group>
        <b-form-group
          class="mt-3"
          id="group-password"
          label="비밀번호"
          label-for="input-password"
          :state="passwordState"
        >
          <b-form-input
            id="input-password"
            type="password"
            v-model="credentials.password"
            placeholder="비밀번호"
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
          로그인 상태 유지
        </b-form-checkbox>

        <hsm-button
          icon="arrow-right-square-fill"
          variant="main"
          textVariant="light"
          fontWeight="bold"
          class="w-100 mt-4"
          @click="handleLogin()"
        >
          로그인
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
      AppModule.showLoading("로그인 중");

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
          title: "로그인 오류",
          message: result.errorMessage as string,
        });
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>
>
