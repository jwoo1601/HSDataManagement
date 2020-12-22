<template>
  <transition
    name="service-detail"
    enter-active-class="animate__animated animate__slideInUp animate__fast"
    leave-active-class="animate__animated animate__slideOutDown animate__fast"
  >
    <div class="service-detail">
      <hsm-item-detail :header="header" @close="handleClose()">
        <b-form
          name="serviceDetail"
          class="px-4 py-4 border border-2 border-light"
        >
          <b-form-row v-if="service" class="mt-1">
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
                label="서비스명"
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
          <b-form-row class="mt-1">
            <b-col cols="12" xl="8">
              <b-form-group
                id="group-duration"
                label="시간"
                label-for="input-duration"
              >
                <b-form-timepicker
                  id="input-duration"
                  v-model="input.duration"
                  show-seconds
                  hide-header
                  no-close-button
                  :hour12="false"
                  placeholder="서비스 시간을 선택해주세요"
                  locale="ko-KR"
                  required
                ></b-form-timepicker>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" xl="8">
              <b-form-group
                id="group-group"
                label="서비스 그룹"
                label-for="input-group"
              >
                <div
                  v-if="input.group !== undefined"
                  class="d-inline-block px-3 py-2 mb-3 bg-main text-white h5 rounded"
                >
                  <b-icon icon="collection-fill" class="mr-2"></b-icon>
                  {{ serviceGroupInfo }}
                </div>
                <hsm-button
                  id="input-group"
                  class="d-block"
                  icon="list-check"
                  variant="outline-success"
                  textVariant="success"
                  hoverVariant="success"
                  hoverTextVariant="light"
                  fontWeight="bold"
                  spacing="1"
                  @click="openServiceGroupSelectionDialog()"
                >
                  서비스 그룹 선택
                </hsm-button>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1" v-if="service">
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
                v-if="service"
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
      <hsm-data-selection-dialog
        :visible.sync="selectionDialogVisible"
        dialog-title="서비스 그룹 선택"
        header-icon="collection-fill"
        header="서비스 그룹"
        select-mode="single"
        @selection-confirm="handleServiceGroupSelections"
        @selection-cancel="handleSelectionDialogClose"
        :field-definitions="serviceGroupFieldDefinitions"
        :fetch-entries-action="fetchServiceGroupEntriesAsync"
        :entries="serviceGroups"
        :entries-mapper="mapServiceGroupTableItems"
        :criteria="serviceGroupCriteria"
        :criteria-options="serviceGroupCriteriaOptions"
      >
        <template #cell(note)="data">
          <div class="d-flex justify-content-center text-center pt-1">
            <hsm-button
              :id="`selection-serviceGroup-showNote-${data.index}`"
              class="mr-3"
              size="sm"
              icon="pencil-fill"
              variant="outline-success"
              textVariant="success"
              hoverVariant="success"
              hoverTextVariant="light"
              fontWeight="bold"
            >
              노트
            </hsm-button>
            <b-tooltip
              v-if="data.value"
              :target="`selection-serviceGroup-showNote-${data.index}`"
              placement="top"
              variant="success"
            >
              {{ data.value }}
            </b-tooltip>
          </div>
        </template>
      </hsm-data-selection-dialog>
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
import ServiceModule from "@/store/modules/service";
import ServiceGroupModule from "@/store/modules/serviceGroup";
import HSMResponseModel from "@/models/api/ResponseModel";
import Success from "@/views/Success.vue";
import HSMService, {
  HSMServiceGroup,
  HSMServiceInputModel,
} from "@/models/api/Service";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
} from "./HSMDataTable.vue";

interface ServiceDetailInput {
  id?: number;
  name?: string;
  duration?: string;
  group?: HSMServiceGroup;
  note?: string;
}

@Component({
  components: {},
})
export default class HSMServiceDetail extends Vue {
  @Prop({ default: null })
  readonly service!: HSMService | null;

  input: ServiceDetailInput = {};
  selectionDialogVisible = false;
  readonly serviceGroupFieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "numServices", label: "등록된 서비스", sortable: true },
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
  ];
  readonly serviceGroupCriteria: "id" | "name" | "numServices" = "id";
  readonly serviceGroupCriteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "numServices", text: "서비스 갯수" },
  ];

  @Watch("service")
  mapCustomerToInput() {
    if (this.service) {
      this.input = {
        ...this.service,
        duration: this.service.duration.toString(),
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

  openServiceGroupSelectionDialog() {
    this.selectionDialogVisible = true;
  }

  get serviceGroupInfo() {
    return `[${this.input.group?.id}] ${this.input.group?.name}`;
  }

  get serviceGroups() {
    return ServiceGroupModule.serviceGroups;
  }

  get createdAt() {
    return this.service?.created_at.toLocaleString("ko-KR");
  }

  get lastUpdatedAt() {
    return this.service?.last_updated_at.toLocaleString("ko-KR");
  }

  async fetchServiceGroupEntriesAsync() {
    return ServiceGroupModule.fetchServiceGroupsAsync();
  }

  mapServiceGroupTableItems(serviceGroup: HSMServiceGroup) {
    return {
      id: serviceGroup.id,
      name: serviceGroup.name,
      note: serviceGroup.note ?? null,
      numServices: ServiceModule.services.filter(
        (s) => s.group?.id === serviceGroup.id
      ).length,
      action: {
        edit: true,
        delete: true,
      },
      raw: serviceGroup,
    };
  }

  async handleSave() {
    AppModule.showLoading("저장 중");

    const inputModel: HSMServiceInputModel = {
      name: this.input.name as string,
      duration: new TimeSpan(this.input.duration as string).toString(),
      group: this.input.group?.id,
      note: this.input.note,
    };

    let response: HSMResponseModel<HSMService>;
    if (this.service) {
      response = await ServiceModule.editServiceAsync({
        id: this.service.id,
        inputModel,
      });
    } else {
      response = await ServiceModule.addServiceAsync(inputModel);
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
      "서비스 제거",
      "정말로 해당 서비스를 제거하시겠습니까?"
    );

    if (confirmed && this.service) {
      AppModule.showLoading();

      const success = await ServiceModule.deleteServiceAsync(this.service.id);

      AppModule.hideLoading();

      if (success) {
        this.delete();
        this.close();
      } else {
        AppModule.showErrorDialog({
          title: "오류",
          message:
            "서비스를 제거하는 중에 오류가 발생했습니다. 잠시 후 다시 시도해주세요.",
        });
      }
    }
  }

  handleClose() {
    this.close();
  }

  handleServiceGroupSelections(selectedItems: HSMDataTableItem[]) {
    this.input.group = selectedItems[0].raw as HSMServiceGroup;
  }

  handleSelectionDialogClose() {}

  get header() {
    return this.service ? `서비스 ${this.service.name} 정보` : "서비스 추가";
  }
}
</script>

<style lang="scss" scoped></style>
>
