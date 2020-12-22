<template>
  <transition
    name="customer-detail"
    enter-active-class="animate__animated animate__slideInUp animate__fast"
    leave-active-class="animate__animated animate__slideOutDown animate__fast"
  >
    <div class="customer-detail">
      <hsm-item-detail :header="header" @close="close()">
        <b-form
          name="customerDetail"
          class="px-4 py-4 border border-2 border-light"
        >
          <b-form-row>
            <b-col class="d-flex justify-content-end">
              <div v-show="input.hidden" class="text-secondary small">
                <b-icon icon="eye-slash-fill" class="mr-1"></b-icon>
                숨김
              </div>
              <div v-show="input.discharged" class="text-danger small ml-3">
                <b-icon icon="dash-circle-fill" class="mr-1"></b-icon>
                퇴원
              </div>
            </b-col>
          </b-form-row>
          <b-form-row v-if="customer" class="mt-1">
            <b-col cols="9" md="7" lg="5" xl="3">
              <b-form-group id="group-id" label="고객번호" label-for="input-id">
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
              <b-form-group id="group-name" label="성명" label-for="input-name">
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
            <b-col>
              <b-form-group
                id="group-admission-date"
                label="입원일"
                label-for="input-admission-date"
              >
                <b-form-datepicker
                  id="input-admission-date"
                  v-model="input.admission_date"
                  start-weekday="1"
                  locale="ko-KR"
                ></b-form-datepicker>
              </b-form-group>
            </b-col>
            <b-col>
              <b-form-group
                id="group-discharge-date"
                label="퇴원일"
                label-for="input-discharge-date"
              >
                <b-form-datepicker
                  id="input-discharge-date"
                  :disabled="!input.discharged"
                  v-model="input.discharge_date"
                  start-weekday="1"
                  locale="ko-KR"
                  today-button
                  label-today-button="오늘날짜 선택"
                  label-no-date-selected="선택된 날짜가 없습니다"
                  reset-button
                  label-reset-button="선택 해제"
                  :date-disabled-fn="isDischargeDateDisabled"
                ></b-form-datepicker>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col>
              <b-form-group
                id="group-settings"
                label="설정"
                label-for="input-settings"
              >
                <div id="input-settings" class="pl-2 py-2">
                  <hsm-button
                    v-if="input.hidden"
                    icon="eye-fill"
                    variant="outline-secondary"
                    textVariant="secondary"
                    hoverVariant="secondary"
                    hoverTextVariant="light"
                    fontWeight="bold"
                    @click="handleToggleVisibility(true)"
                  >
                    보이기
                  </hsm-button>
                  <hsm-button
                    v-else
                    icon="eye-slash-fill"
                    variant="outline-secondary"
                    textVariant="secondary"
                    hoverVariant="secondary"
                    hoverTextVariant="light"
                    fontWeight="bold"
                    @click="handleToggleVisibility(false)"
                  >
                    숨기기
                  </hsm-button>
                  <hsm-button
                    class="ml-3"
                    icon="journal-plus"
                    variant="outline-primary"
                    textVariant="primary"
                    hoverVariant="primary"
                    hoverTextVariant="light"
                    fontWeight="bold"
                  >
                    서비스 배정
                  </hsm-button>
                  <hsm-button
                    class="ml-3"
                    icon="journal-plus"
                    variant="outline-submain"
                    textVariant="submain"
                    hoverVariant="submain"
                    hoverTextVariant="light"
                    fontWeight="bold"
                  >
                    기타 정보 보기
                  </hsm-button>
                  <hsm-button
                    v-if="!input.discharged"
                    class="ml-3"
                    icon="dash-circle-fill"
                    variant="outline-danger"
                    textVariant="danger"
                    hoverVariant="danger"
                    hoverTextVariant="light"
                    fontWeight="bold"
                    @click="handleSetDischarged(true)"
                  >
                    퇴원처리
                  </hsm-button>
                  <hsm-button
                    v-else
                    class="ml-3"
                    icon="plus-circle-fill"
                    variant="outline-success"
                    textVariant="success"
                    hoverVariant="success"
                    hoverTextVariant="light"
                    fontWeight="bold"
                    @click="handleSetDischarged(false)"
                  >
                    퇴원처리 되돌리기
                  </hsm-button>
                </div>
              </b-form-group>
            </b-col>
          </b-form-row>
          <b-form-row class="mt-1">
            <b-col cols="12" lg="10">
              <b-form-group id="group-tags" label="태그" label-for="input-tags">
                <b-form-tags
                  input-id="input-tags"
                  tag-variant="submain"
                  tag-class="pl-2"
                  v-model="input.tags"
                  size="lg"
                  placeholder="태그 입력"
                  tag-removed-label="하하"
                  add-button-text="Enter"
                  add-button-variant="success"
                  remove-on-delete
                  duplicate-tag-text="중복된 태그 입니다"
                ></b-form-tags>
                <b-form-text>태그를 추가하려면 Enter 를 눌러주세요</b-form-text>
                <b-form-text>
                  태그를 제거하려면 &#10005; 버튼 클릭 또는 백스페이스를
                  눌러주세요
                </b-form-text>
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
                v-if="customer"
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
    </div>
  </transition>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Emit, Inject, Prop, Watch } from "vue-property-decorator";
import "animate.css";
import HSMCustomer, { HSMCustomerInputModel } from "@/models/api/Customer";
import TimeSpan from "@/models/TimeSpan";
import AppModule from "@/store/modules/app";
import CustomerModule from "@/store/modules/customer";
import HSMResponseModel from "@/models/api/ResponseModel";
import Success from "@/views/Success.vue";

interface CustomerDetailInput {
  id?: number;
  name?: string;
  hidden: boolean;
  discharged: boolean;
  admission_date: Date;
  discharge_date?: Date;
  note?: string;
  tags: string[];
}

@Component({
  components: {},
})
export default class HSMCustomerDetail extends Vue {
  @Prop({ default: undefined })
  customer!: HSMCustomer;
  input: CustomerDetailInput = {
    hidden: false,
    discharged: false,
    admission_date: new Date(),
    tags: [],
  };

  @Watch("customer")
  mapCustomerToInput() {
    this.input = {
      ...this.customer,
    };
  }

  @Emit()
  save() {}

  @Emit()
  delete() {}

  @Emit()
  close() {}

  async handleSave() {
    AppModule.showLoading("저장 중");

    const inputModel: HSMCustomerInputModel = {
      name: this.input.name as string,
      hidden: this.input.hidden,
      discharged: this.input.discharged,
      admission_date: this.input.admission_date,
      discharge_date: this.input.discharge_date,
      tags: this.input.tags,
      note: this.input.note,
    };

    let response: HSMResponseModel<HSMCustomer>;
    if (this.customer) {
      response = await CustomerModule.editCustomerAsync({
        id: this.customer.id,
        inputModel,
      });
    } else {
      response = await CustomerModule.addCustomerAsync(inputModel);
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
      "고객 제거",
      "정말로 해당 고객 정보를 제거하시겠습니까?"
    );

    if (confirmed && this.customer) {
      AppModule.showLoading();

      const success = await CustomerModule.deleteCustomerAsync(
        this.customer.id
      );

      AppModule.hideLoading();

      if (success) {
        this.delete();
        this.close();
      } else {
        AppModule.showErrorDialog({
          title: "오류",
          message:
            "고객 정보를 제거하는 중에 오류가 발생했습니다. 잠시 후 다시 시도해주세요.",
        });
      }
    }
  }

  handleClose() {
    this.close();
  }

  // customer: HSMCustomer = {
  //   id: 1,
  //   name: "황철희",
  //   hidden: false,
  //   discharged: false,
  //   admission_date: new Date(),
  //   note: "밥을 많이 드심",
  //   created_at: new Date(),
  //   last_updated_at: new Date(),
  //   tags: ["밥", "다져서", "포크"],
  //   serviceAssignments: {
  //     breakfast: {
  //       id: 22,
  //       name: "죽식(다)",
  //       duration: new TimeSpan("02:03:01"),
  //       group: {
  //         id: 5,
  //         name: "프로그램",
  //         note: "없음",
  //       },
  //     },
  //     lunch: {
  //       id: 22,
  //       name: "죽식(다)",
  //       duration: new TimeSpan("02:03:01"),
  //       group: {
  //         id: 5,
  //         name: "프로그램",
  //         note: "없음",
  //       },
  //     },
  //     dinner: {
  //       id: 22,
  //       name: "죽식(다)",
  //       duration: new TimeSpan("02:03:01"),
  //       group: {
  //         id: 5,
  //         name: "프로그램",
  //         note: "없음",
  //       },
  //     },
  //   },
  // };

  get header() {
    return this.customer ? `${this.customer.name}님의 고객 정보` : "고객 추가";
  }

  handleToggleVisibility(visible: boolean) {
    this.input.hidden = !visible;
  }

  handleSetDischarged(discharged: boolean) {
    if (discharged) {
      this.input.discharge_date = new Date();
    } else {
      this.input.discharge_date = undefined;
    }

    this.input.discharged = discharged;
  }

  isDischargeDateDisabled(ymd: string, date: Date) {
    return date <= this.input.admission_date;
  }
}
</script>

<style lang="scss" scoped></style>
>
