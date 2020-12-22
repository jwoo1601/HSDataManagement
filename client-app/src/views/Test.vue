<template>
  <div class="test">
    <hsm-button v-if="false" @click="showLoadingDialog()">로딩창</hsm-button>
    <hsm-success
      title="회원가입 성공"
      :message="
        `효성제일건강센터 데이터 관리 시스템으로의 회원가입을 성공했습니다. 회원가입 절차를 완료하기 위해서는 이메일 주소 확인이 필요합니다. 자세한 사항은 이메일을 확인해주세요.`
      "
      linkText="이메일 재전송"
      linkUrl="/confirm-email/resend"
      showLink
    ></hsm-success>
    <hsm-error></hsm-error>
    <hsm-button :spacing="0" to="/customer/5">Customer Detail</hsm-button>

    <hsm-button v-b-modal.confirm-dialog>Confirm Dialog</hsm-button>
    <hsm-dialog id="confirm-dialog"></hsm-dialog>

    <hsm-button @click="showMessageDialog()">Message Dialog</hsm-button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import AppModule from "@/store/modules/app";

@Component({
  components: {},
})
export default class Test extends Vue {
  showLoadingDialog() {
    AppModule.showLoading();
  }

  showMessageDialog() {
    AppModule.showErrorDialog({
      title: "하하하",
      message: "호호호",
    });
  }
}
</script>

<style lang="scss" scoped></style>>
