<template>
  <hsm-data-table
    ref="dataTable"
    header-icon="shield-lock-fill"
    :header="$t('menu.securityCode')"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="codes"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :show-actions="false"
    :allow-edit="false"
    :allow-delete="false"
    :criteria-options="criteriaOptions"
    :row-class="rowClass"
    select-mode="single"
  >
    <template #cell(status)="{ value }">
      <div class="pt-1 font-weight-bold text-center">
        <span :class="[`text-${getStatusVariant(value)}`]">
          {{ value }}
        </span>
      </div>
    </template>

    <template #cell(condActions)="{ item }">
      <div class="d-flex justify-content-center pt-1">
        <hsm-button
          v-if="item.status === 'Valid'"
          class="mr-3"
          size="sm"
          icon="shield-slash-fill"
          variant="outline-danger"
          textVariant="danger"
          hoverVariant="danger"
          hoverTextVariant="light"
          fontWeight="bold"
          @click="invalidateAsync(item)"
        >
          Invalidate
        </hsm-button>
      </div>
    </template>

    <template #selectionActions>
      <hsm-button
        v-if="dataTable.selectedItems.length > 0"
        class="mr-2"
        size="sm"
        icon="clipboard"
        variant="main"
        textVariant="light"
        fontWeight="bold"
        spacing="1"
        @click="handleCopySelectedCode()"
      >
        {{ $t("action.copySecurityCode") }}
      </hsm-button>
    </template>

    <template #detail="{ visible, notify, close }">
      <hsm-generate-security-code
        v-show="visible"
        @save="notify"
        @delete="notify"
        @close="close"
      ></hsm-generate-security-code>
    </template>
  </hsm-data-table>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import HSMSecurityCode, {
  HSMSecurityCodeType,
} from "@/models/api/SecurityCode";
import AppModule from "@/store/modules/app";
import SecurityCodeModule from "@/store/modules/securityCode";
import {
  BFormSelectOption,
  BTable,
  BvTableCtxObject,
  BvTableFieldArray,
  BvTableFormatterCallback,
} from "bootstrap-vue";
import App from "@/App.vue";
import HSMDataTable, {
  HSMDataTableFieldDefinition,
  HSMDataTableFilterCriteria,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "@/components/HSMDataTable.vue";
import HSMGenerateSecurityCode from "@/components/HSMGenerateSecurityCode.vue";
import { Ref } from "vue-property-decorator";

interface SecurityCodeTableItem extends HSMDataTableItem {
  type: HSMSecurityCodeType;
  valid: boolean;
  generatedAt: Date;
  expiresAt: Date | null;
  status: HSMSecurityCodeStatus;
}

enum HSMSecurityCodeStatus {
  Valid = "Valid",
  Invalid = "Invalid",
  Expired = "Expired",
}

@Component({
  components: {
    "hsm-generate-security-code": HSMGenerateSecurityCode,
  },
})
export default class SecurityCode extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "type", label: "Type", sortable: true },
    {
      key: "status",
      label: "Status",
      sortable: true,
      hasCustomRenderer: true,
    },
    {
      key: "generatedAt",
      label: "Generated At",
      sortable: true,
      formatter: this.formatDate,
    },
    {
      key: "expiresAt",
      label: "Expires At",
      sortable: true,
      formatter: this.formatDate,
    },
    {
      key: "condActions",
      label: "Actions",
      searchable: false,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "label.securityCode",
    name: "Code",
  };
  readonly criteriaOptions: HSMDataTableFilterCriteria[] = [
    { value: "id", text: "ID" },
    { value: "name", text: "Code" },
    { value: "type", text: "Type" },
    { value: "generatedAt", text: "Generated At" },
    { value: "expiresAt", text: "Expires At" },
    {
      value: "status",
      text: "Status",
    },
  ];

  @Ref()
  dataTable!: HSMDataTable;

  get codes() {
    return SecurityCodeModule.securityCodeList;
  }

  async fetchEntriesAsync() {
    return SecurityCodeModule.fetchSecurityCodeListAsync();
  }

  async invalidateAsync(item: SecurityCodeTableItem) {
    AppModule.showLoading(this.$t("loading.processing").toString());

    await SecurityCodeModule.invalidateSecurityCodeAsync({
      code: item.name,
    });
    this.dataTable.notifyViewUpdates();

    AppModule.hideLoading();
  }

  handleCopySelectedCode() {
    if (this.dataTable.selectedItems.length > 0) {
      if (this.$clipboard(this.dataTable.selectedItems[0].name)) {
        this.$bvToast.toast(this.$t("success.copySecurityCode").toString(), {
          title: this.$t("title.copiedToClipboard").toString(),
        });
      }
    }
  }

  mapTableItems(code: HSMSecurityCode): SecurityCodeTableItem {
    return {
      id: code.id,
      name: code.value,
      type: code.code_type,
      valid: code.is_valid,
      generatedAt: code.generated_at,
      expiresAt: code.expires_at ?? null,
      status: this.getCodeStatus(code),
      action: {
        edit: false,
        delete: false,
      },
      raw: { ...code, name: code.value },
    };
  }

  formatDate(date: Date | null, key: string, item: any) {
    if (date) {
      return new Date(date).toLocaleDateString(this.$i18n.locale);
    }

    return "---";
  }

  rowClass(item: SecurityCodeTableItem) {
    if (item.status !== HSMSecurityCodeStatus.Valid) {
      return "table-secondary";
    }

    return;
  }

  getCodeStatus(code: HSMSecurityCode) {
    if (!code.is_valid) {
      return HSMSecurityCodeStatus.Invalid;
    } else if (code.expires_at && code.expires_at.getTime() >= Date.now()) {
      return HSMSecurityCodeStatus.Expired;
    }

    return HSMSecurityCodeStatus.Valid;
  }

  getStatusVariant(status: HSMSecurityCodeStatus) {
    switch (status) {
      case HSMSecurityCodeStatus.Valid:
        return "success";
      case HSMSecurityCodeStatus.Invalid:
        return "danger";
      case HSMSecurityCodeStatus.Expired:
        return "secondary";
    }
  }
}
</script>

<style lang="scss" scoped></style>
>
