<template>
  <transition
    name="service-group-detail"
    enter-active-class="animate__animated animate__slideInUp animate__fast"
    leave-active-class="animate__animated animate__slideOutDown animate__fast"
  >
    <div class="service-group-detail">
      <hsm-item-detail :header="header" @close="handleClose()">
        <b-form
          name="serviceGroupDetail"
          class="px-4 py-4 border border-2 border-light"
        >
          <b-form-row v-if="serviceGroup" class="mt-1">
            <b-col cols="9" md="7" lg="5" xl="3">
              <b-form-group id="group-id" label="연번" label-for="input-id">
                <b-form-input
                  id="input-id"
                  type="text"
                  v-model="input.id"
                  number
                  readonly
                ></b-form-input>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" md="10" lg="8" xl="6">
              <b-form-group
                id="group-name"
                label="서비스 그룹명"
                label-for="input-name"
              >
                <b-form-input
                  id="input-name"
                  type="text"
                  v-model="input.name"
                  required
                  trim
                ></b-form-input>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1" v-if="serviceGroup">
            <b-col>
              <b-form-group
                id="group-created-at"
                label="최초 생성일"
                label-for="input-created-at"
              >
                <b-form-input
                  id="input-created-at"
                  type="text"
                  :value="createdAt"
                  readonly
                ></b-form-input>
              </b-form-group>
            </b-col>
            <b-col>
              <b-form-group
                id="group-last-updated-at"
                label="최근 변경일"
                label-for="input-last-updated-at"
              >
                <b-form-input
                  id="input-last-updated-at"
                  type="text"
                  :value="lastUpdatedAt"
                  readonly
                ></b-form-input>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1" v-if="serviceGroup">
            <b-col>
              <b-form-group
                id="group-settings"
                label="설정"
                label-for="input-settings"
              >
                <div id="input-settings" class="pl-2 py-2">
                  <hsm-button
                    class="ml-3"
                    icon="journal-plus"
                    variant="outline-primary"
                    textVariant="primary"
                    hoverVariant="primary"
                    hoverTextVariant="light"
                    fontWeight="bold"
                    @click="handleShowRegisteredServices"
                  >
                    등록된 서비스 보기
                  </hsm-button>
                </div>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" lg="10">
              <b-form-group id="group-note" label="노트" label-for="input-note">
                <b-form-textarea
                  id="input-note"
                  v-model="input.note"
                  required
                  trim
                  no-resize
                  rows="4"
                ></b-form-textarea>
              </b-form-group>
            </b-col>
          </b-form-row>

          <b-form-row class="mt-4">
            <b-col class="d-flex justify-content-end">
              <hsm-button
                icon="person-check-fill"
                variant="success"
                textVariant="light"
                fontWeight="bold"
                @click="handleSave()"
              >
                저장
              </hsm-button>
              <hsm-button
                v-if="serviceGroup"
                icon="person-x-fill"
                variant="danger"
                textVariant="light"
                fontWeight="bold"
                class="ml-3"
                @click="handleDelete()"
              >
                제거
              </hsm-button>

              <hsm-button
                icon="x-square-fill"
                variant="secondary"
                textVariant="light"
                fontWeight="bold"
                class="ml-3"
                @click="handleClose()"
              >
                취소
              </hsm-button>
            </b-col>
          </b-form-row>
        </b-form>
      </hsm-item-detail>
      <hsm-service-viewer
        v-if="serviceGroup"
        title="등록된 서비스 목록"
        :visible.sync="showRegisteredServices"
        :services="registeredServices"
      ></hsm-service-viewer>
    </div>
  </transition>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Emit, Inject, Prop, Ref, Watch } from "vue-property-decorator";
import "animate.css";
import TimeSpan from "@/models/TimeSpan";
import AppModule from "@/store/modules/app";
import ServiceModule from "@/store/modules/service";
import ServiceGroupModule from "@/store/modules/serviceGroup";
import HSMResponseModel from "@/models/api/ResponseModel";
import Success from "@/views/Success.vue";
import {
  HSMServiceGroup,
  HSMServiceGroupInputModel,
} from "@/models/api/Service";
import HSMServiceViewer from "@/components/HSMServiceViewer.vue";

interface ServiceGroupDetailInput {
  id?: number;
  name?: string;
  note?: string;
}

@Component({
  components: { "hsm-service-viewer": HSMServiceViewer },
})
export default class HSMServiceGroupDetail extends Vue {
  @Prop({ default: null })
  serviceGroup?: HSMServiceGroup;

  input: ServiceGroupDetailInput = {};
  showRegisteredServices = false;

  @Watch("serviceGroup")
  mapCustomerToInput() {
    if (this.serviceGroup) {
      this.input = {
        ...this.serviceGroup,
      };
    }
  }

  @Emit()
  save() {}

  @Emit()
  delete() {}

  @Emit()
  close() {
    this.input = {};
  }

  get createdAt() {
    return this.serviceGroup?.created_at.toLocaleString("ko-KR");
  }

  get lastUpdatedAt() {
    return this.serviceGroup?.last_updated_at.toLocaleString("ko-KR");
  }

  async handleSave() {
    AppModule.showLoading("저장 중");

    const inputModel: HSMServiceGroupInputModel = {
      name: this.input.name as string,
      note: this.input.note,
    };

    let response: HSMResponseModel<HSMServiceGroup>;
    if (this.serviceGroup) {
      response = await ServiceGroupModule.editServiceGroupAsync({
        id: this.serviceGroup.id,
        inputModel,
      });
    } else {
      response = await ServiceGroupModule.addServiceGroupAsync(inputModel);
    }

    AppModule.hideLoading();

    if (response.isSuccess) {
      this.close();
      this.save();
    } else {
      AppModule.showErrorDialog({
        title: "저장 오류",
        message: response.errorMessage as string,
      });
    }
  }

  async handleDelete() {
    const confirmed = await this.$hsmConfirmDialog(
      "서비스 그룹 제거",
      "정말로 해당 서비스 그룹을 제거하시겠습니까?"
    );

    if (confirmed && this.serviceGroup) {
      AppModule.showLoading();

      const success = await ServiceGroupModule.deleteServiceGroupAsync(
        this.serviceGroup.id
      );

      AppModule.hideLoading();

      if (success) {
        this.delete();
        this.close();
      } else {
        AppModule.showErrorDialog({
          title: "오류",
          message:
            "서비스 그룹을 제거하는 중에 오류가 발생했습니다. 잠시 후 다시 시도해주세요.",
        });
      }
    }
  }

  handleShowRegisteredServices() {
    this.showRegisteredServices = true;
  }

  handleClose() {
    this.close();
  }

  get header() {
    return this.serviceGroup
      ? `서비스 그룹 ${this.serviceGroup.name} 정보`
      : "서비스 그룹 추가";
  }

  get registeredServices() {
    return this.serviceGroup
      ? ServiceModule.services.filter(
          (s) => s.group?.id === this.serviceGroup?.id
        )
      : [];
  }
}
</script>

<style lang="scss" scoped></style>
>
