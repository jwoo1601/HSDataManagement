<template :key="componentUpdates">
  <div class="register">
    <b-row v-show="serverErrors" class="mt-4">
      <b-col cols="8" offset="2" md="6" offset-md="3" lg="4" offset-lg="4">
        <b-alert class="px-3 py-3" show variant="danger">
          <h2 class="font-weight-bold ml-2">
            <b-icon icon="exclamation-triangle-fill" class="mr-2"></b-icon>
            {{ $t("title.error") }}
          </h2>
          <hr />
          <ul v-if="serverErrors">
            <li :key="error.key" v-for="error in serverErrors.getAllErrors()">
              {{ error.value }}
            </li>
          </ul>
        </b-alert>
      </b-col>
    </b-row>
    <b-row v-if="!createdUser" class="my-4">
      <b-col cols="8" offset="2" lg="6" offset-lg="3" xl="4" offset-xl="4">
        <b-form
          name="login"
          class="px-4 py-3 border border-2 border-light shadow"
          :state="formState"
          :validated="serverValidated"
        >
          <div class="h3 my-3 text-center font-weight-bold">
            <b-icon icon="pencil-square" class="mr-2"></b-icon>
            {{ $t("title.register") }}
          </div>
          <b-form-group
            class="mt-5"
            id="group-username"
            :label="$t('title.username')"
            label-for="input-username"
            :state="usernameState"
          >
            <b-form-input
              id="input-username"
              type="text"
              v-model="userData.username"
              :placeholder="$t('title.username')"
              :state="usernameState"
              required
              trim
            ></b-form-input>
            <b-form-invalid-feedback
              v-for="error in getValidationErrors('username')"
              :key="error.key"
              :state="usernameState"
            >
              {{ error.value }}
            </b-form-invalid-feedback>
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
              v-model="userData.password"
              :placeholder="$t('title.password')"
              :state="passwordState"
              required
              trim
            ></b-form-input>
            <b-form-invalid-feedback
              v-for="error in getValidationErrors('password')"
              :key="error.key"
              :state="passwordState"
            >
              {{ error.value }}
            </b-form-invalid-feedback>
          </b-form-group>
          <b-form-group
            class="mt-3"
            id="group-confirmPassword"
            :label="$t('title.confirmPassword')"
            label-for="input-confirmPassword"
            :state="confirmPasswordState"
          >
            <b-form-input
              id="input-confirmPassword"
              type="password"
              v-model="userData.confirm_password"
              :placeholder="$t('title.confirmPassword')"
              :state="confirmPasswordState"
              required
              trim
            ></b-form-input>
            <b-form-invalid-feedback
              v-for="error in getValidationErrors('confirm_password')"
              :key="error.key"
              :state="confirmPasswordState"
            >
              {{ error.value }}
            </b-form-invalid-feedback>
          </b-form-group>
          <b-form-group
            class="mt-3"
            id="group-name"
            :label="$t('title.name')"
            label-for="input-name"
            :state="nameState"
          >
            <b-form-input
              id="input-name"
              type="text"
              v-model="userData.name"
              :placeholder="$t('title.name')"
              :state="nameState"
              required
              trim
            ></b-form-input>
            <b-form-invalid-feedback
              v-for="error in getValidationErrors('name')"
              :key="error.key"
              :state="nameState"
            >
              {{ error.value }}
            </b-form-invalid-feedback>
          </b-form-group>
          <b-form-group
            class="mt-3"
            id="group-email"
            :label="$t('title.emailAddress')"
            label-for="input-email"
            :state="nameState"
          >
            <b-input-group>
              <b-input-group-prepend>
                <b-form-input
                  id="input-email"
                  type="text"
                  v-model="userData.email"
                  :placeholder="$t('title.email')"
                  :state="emailState"
                  required
                  trim
                ></b-form-input>
              </b-input-group-prepend>
              <b-input-group-prepend is-text>@</b-input-group-prepend>
              <b-form-input
                id="input-email-domain"
                type="text"
                v-model="userData.email_domain"
                placeholder="example.com"
                :state="emailDomainState"
                required
                trim
              ></b-form-input>
              <b-form-invalid-feedback
                v-for="error in getValidationErrors('email')"
                :key="error.key"
                :state="emailState"
              >
                {{ error.value }}
              </b-form-invalid-feedback>
              <b-form-invalid-feedback
                v-for="error in getValidationErrors('email_domain')"
                :key="error.key"
                :state="emailDomainState"
              >
                {{ error.value }}
              </b-form-invalid-feedback>
            </b-input-group>
          </b-form-group>
          <b-form-group
            class="mt-3"
            id="group-security-code"
            :label="$t('title.securityCode')"
            label-for="input-security-code"
            :state="nameState"
          >
            <b-form-input
              id="input-security-code"
              type="text"
              v-model="userData.security_code"
              :placeholder="$t('title.securityCode')"
              :state="securityCodeState"
              required
            ></b-form-input>
            <b-form-invalid-feedback
              v-for="error in getValidationErrors('security_code')"
              :key="error.key"
              :state="securityCodeState"
            >
              {{ error.value }}
            </b-form-invalid-feedback>
          </b-form-group>

          <hsm-button
            icon="arrow-right-square-fill"
            variant="success"
            textVariant="light"
            fontWeight="bold"
            class="w-100 mt-4"
            @click="registerUserAsync()"
          >
            {{ $t("title.register") }}
          </hsm-button>
        </b-form>
      </b-col>
    </b-row>
    <hsm-success
      v-if="createdUser"
      :title="$t('success.register')"
      :message="$t('message.registrationSuccess', { email: createdUser.email })"
      :linkText="$t('title.resendConfirmationEmail')"
      linkUrl="/account/confirm-email/resend"
      showLink
    ></hsm-success>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

import { Inject } from "vue-property-decorator";
import { Container } from "inversify";
import serviceTypes from "@/services/serviceTypes";
import HSMUser, { HSMUserRegisterInputModel } from "@/models/api/User";
import HSMValidationError from "@/models/api/ValidationError";
import { NavigationGuardNext, Route } from "vue-router";
import IQueryService from "@/services/queryService";
import AppModule from "@/store/modules/app";

@Component({
  components: {},
})
export default class Register extends Vue {
  componentUpdates = 0;
  userData: HSMUserRegisterInputModel = {};
  errors = new HSMValidationError();
  serverErrors: HSMValidationError | null = null;
  serverValidated = false;
  createdUser: HSMUser | null = null;

  @Inject(serviceTypes.ServiceContainer)
  private services!: Container;
  private queryService!: IQueryService;

  created() {
    this.queryService = this.services.get<IQueryService>(
      serviceTypes.QueryService
    );
  }

  get formState() {
    return (
      this.usernameState &&
      this.passwordState &&
      this.confirmPasswordState &&
      this.nameState &&
      this.emailState &&
      this.emailDomainState &&
      this.securityCodeState
    );
  }

  get usernameState() {
    if (!this.userData.username) {
      return null;
    }

    if (this.userData.username.length <= 4) {
      this.errors.addErrorFor(
        "username",
        "아이디는 최소 4자 이상이어야 합니다.",
        "length"
      );
    } else {
      this.errors.clearErrorsFor("username", "length");
    }

    return !this.errors.hasAnyErrorsFor("username");
  }

  get passwordState() {
    if (!this.userData.password) {
      return null;
    }

    if (
      this.userData.password.length < 8 ||
      this.userData.password.length > 100
    ) {
      this.errors.addErrorFor(
        "password",
        "비밀번호는 8자에서 100자 사이여야 합니다.",
        "length"
      );
    } else {
      this.errors.clearErrorsFor("password", "length");
    }

    return !this.errors.hasAnyErrorsFor("password");
  }

  get confirmPasswordState() {
    if (!this.userData.confirm_password) {
      return null;
    }

    if (this.userData.password !== this.userData.confirm_password) {
      this.errors.addErrorFor(
        "confirm_password",
        "비밀번호와 일치하지 않습니다.",
        "notEqual"
      );
    } else {
      this.errors.clearErrorsFor("confirm_password", "notEqual");
    }

    return !this.errors.hasAnyErrorsFor("confirm_password");
  }

  get nameState() {
    if (!this.userData.name) {
      return null;
    }

    if (this.userData.name.length === 0) {
      this.errors.addErrorFor("name", "이름을 입력해야 합니다.", "empty");
    } else {
      this.errors.clearErrorsFor("name", "empty");
    }

    return !this.errors.hasAnyErrorsFor("name");
  }

  get emailState() {
    if (!this.userData.email) {
      return null;
    }

    if (this.userData.email.length === 0) {
      this.errors.addErrorFor("email", "이메일을 입력해야 합니다.", "empty");
    } else {
      this.errors.clearErrorsFor("email", "empty");
    }

    if (!/^[\w-\\.]+$/.test(this.userData.email)) {
      this.errors.addErrorFor("email", "올바른 이메일이 아닙니다.", "pattern");
    } else {
      this.errors.clearErrorsFor("email", "pattern");
    }

    return !this.errors.hasAnyErrorsFor("email");
  }

  get emailDomainState() {
    if (!this.userData.email_domain) {
      return null;
    }

    if (this.userData.email_domain.length === 0) {
      this.errors.addErrorFor(
        "email_domain",
        "이메일 주소을 입력해야 합니다.",
        "empty"
      );
    } else {
      this.errors.clearErrorsFor("email_domain", "empty");
    }

    if (!/^([\w-]+\.)+[\w-]{2,4}$/.test(this.userData.email_domain)) {
      this.errors.addErrorFor(
        "email_domain",
        "올바른 이메일 주소가 아닙니다.",
        "pattern"
      );
    } else {
      this.errors.clearErrorsFor("email_domain", "pattern");
    }

    return !this.errors.hasAnyErrorsFor("email_domain");
  }

  get securityCodeState() {
    if (!this.userData.security_code) {
      return null;
    }

    if (this.userData.security_code.length === 0) {
      this.errors.addErrorFor(
        "security_code",
        "보안코드를 입력해야 합니다.",
        "empty"
      );
    } else {
      this.errors.clearErrorsFor("security_code", "empty");
    }

    return !this.errors.hasAnyErrorsFor("security_code");
  }

  getValidationErrors(name: string) {
    return this.errors.getAllErrorsFor(name) ?? [];
  }

  resetState() {
    this.errors.clearAllErrors();
    this.serverErrors = null;
    this.createdUser = null;
    this.serverValidated = false;
  }

  async registerUserAsync() {
    if (this.formState) {
      AppModule.showLoading(this.$t("loading.processing").toString());

      this.resetState();

      const result = await this.queryService.createAsync<HSMUser>(
        "/api/accounts/register",
        this.userData
      );
      if (result.isSuccess) {
        this.createdUser = result.model;
      } else {
        this.serverErrors = result.validationErrors;
      }

      this.serverValidated = true;
      AppModule.hideLoading();
    }
  }

  beforeRouteLeave(to: Route, from: Route, next: NavigationGuardNext) {
    if (this.createdUser) {
      this.componentUpdates += 1;
    }

    next();
  }
}
</script>

<style lang="scss" scoped></style>
>
