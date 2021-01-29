<template>
  <transition
    name="generate-securityCode"
    enter-active-class="animate__animated animate__slideInUp animate__fast"
    leave-active-class="animate__animated animate__slideOutDown animate__fast"
  >
    <div>
      <hsm-item-detail :header="header" @close="handleClose()">
        <b-form
          name="generate-securityCode"
          class="px-4 py-4 border border-2 border-light"
        >
          <b-form-row class="mt-1">
            <b-col cols="12" md="10" lg="8" xl="6">
              <b-form-group id="group-type" label="Type" label-for="input-type">
                <b-form-select
                  name="type"
                  :options="typeOptions"
                  v-model="input.type"
                  required
                ></b-form-select>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" xl="8">
              <b-form-group
                id="group-age"
                label="Age in seconds"
                label-for="input-age"
                :description="ageDescription"
              >
                <b-form-input
                  id="input-age"
                  type="text"
                  v-model="input.age"
                  required
                  trim
                  number
                ></b-form-input>
              </b-form-group>
            </b-col>
          </b-form-row>

          <b-form-row class="mt-4">
            <b-col class="d-flex justify-content-end">
              <hsm-button
                icon="shield-fill-check"
                variant="success"
                textVariant="light"
                fontWeight="bold"
                @click="handleSave()"
              >
                {{ $t("action.save") }}
              </hsm-button>

              <hsm-button
                icon="shield-fill-x"
                variant="secondary"
                textVariant="light"
                fontWeight="bold"
                class="ml-3"
                @click="handleClose()"
              >
                {{ $t("action.cancel") }}
              </hsm-button>
            </b-col>
          </b-form-row>
        </b-form>
      </hsm-item-detail>
    </div>
  </transition>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Emit, Inject, Prop, Watch } from "vue-property-decorator";
import "animate.css";
import TimeSpan from "@/models/TimeSpan";
import AppModule from "@/store/modules/app";
import SecurityCodeModule from "@/store/modules/securityCode";
import HSMResponseModel from "@/models/api/ResponseModel";
import Success from "@/views/Success.vue";
import HSMSecurityCode, {
  HSMSecurityCodeGenerateInputModel,
  HSMSecurityCodeType,
  HSMSecurityCodeTypes,
} from "@/models/api/SecurityCode";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
} from "./HSMDataTable.vue";

interface SecurityCodeInput {
  type?: HSMSecurityCodeType;
  age?: string;
}

@Component({
  components: {},
})
export default class HSMGenerateSecurityCode extends Vue {
  readonly typeOptions = [
    { value: "normal", text: "Normal" },
    { value: "transient", text: "Transient (One-time use only)" },
    { value: "persistent", html: "<b>Persistent</b>" },
  ];

  input: SecurityCodeInput = {
    type: HSMSecurityCodeTypes.Persistent,
  };

  @Emit()
  save() {}

  @Emit()
  delete() {}

  @Emit()
  close() {
    this.input = {
      type: HSMSecurityCodeTypes.Persistent,
    };
  }

  get ageDescription() {
    if (this.input.age) {
      let desc = "";
      const age = Math.round(parseInt(this.input.age));

      const days = Math.floor(age / (60 * 60 * 24));
      if (days > 0) {
        desc += `${days}d `;
      }

      const divisor_for_hours = age % (60 * 60 * 24);
      const hours = Math.floor(divisor_for_hours / (60 * 60));
      if (hours > 0) {
        desc += `${hours}h `;
      }

      const divisor_for_minutes = divisor_for_hours % (60 * 60);
      const minutes = Math.floor(divisor_for_minutes / 60);
      if (minutes > 0) {
        desc += `${minutes}m `;
      }

      const divisor_for_seconds = divisor_for_minutes % 60;
      const seconds = Math.ceil(divisor_for_seconds);
      if (seconds > 0) {
        desc += `${seconds}s `;
      }

      return desc;
    }

    return "";
  }

  async handleSave() {
    AppModule.showLoading(this.$t("loading.save").toString());

    const inputModel: HSMSecurityCodeGenerateInputModel = {
      code_type: this.input.type,
      age: this.input.age ? parseInt(this.input.age) : undefined,
    };

    const response = await SecurityCodeModule.generateSecurityCodeAsync(
      inputModel
    );

    AppModule.hideLoading();

    if (response.isSuccess) {
      this.close();
      this.save();
    } else {
      AppModule.showErrorDialog({
        title: this.$t("title.error").toString(),
        message: this.$t(response.errorMessage as string).toString(),
      });
    }
  }

  handleClose() {
    this.close();
  }

  get header() {
    return this.$t("title.newSecurityCode").toString();
  }
}
</script>

<style lang="scss" scoped></style>
>
