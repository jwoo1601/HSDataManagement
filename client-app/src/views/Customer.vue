<template>
  <b-row>
    <b-col cols="8" offset="2">
      <hsm-data-table
        header-icon="person-lines-fill"
        :header="$t('tab.customer')"
        :field-definitions="fieldDefinitions"
        :fetch-entries-action="fetchEntriesAsync"
        :entries="customers"
        :entries-mapper="mapTableItems"
        :labels="labels"
        :delete-entry-action="deleteEntryAsync"
        :criteria-options="criteriaOptions"
        :row-class="rowClass"
      >
        <template #cell(visible)="data">
          <div class="d-flex justify-content-center text-center pb-3">
            <b-form-checkbox
              class="mt-3"
              name="visible"
              v-model="data.value"
              @change="handleToggleVisibility(data.item)"
              switch
            ></b-form-checkbox>
          </div>
        </template>

        <template #cell(note)="{ value, index }">
          <div class="d-flex justify-content-center text-center pt-1">
            <hsm-button
              :id="`customer-showNote-${index}`"
              class="mr-3"
              size="sm"
              icon="pencil-fill"
              variant="outline-success"
              textVariant="success"
              hoverVariant="success"
              hoverTextVariant="light"
              fontWeight="bold"
            >
              {{ $t("field.note") }}
            </hsm-button>
            <b-tooltip
              v-if="value !== null"
              :target="`customer-showNote-${index}`"
              placement="top"
              variant="success"
            >
              {{ value }}
            </b-tooltip>
          </div>
        </template>

        <template #detail="{ visible, item, notify, close }">
          <hsm-customer-detail
            v-show="visible"
            :customer="item"
            @save="notify"
            @delete="notify"
            @close="close"
          ></hsm-customer-detail>
        </template>
      </hsm-data-table>
    </b-col>
  </b-row>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import HSMCustomer from "@/models/api/Customer";
import AppModule from "@/store/modules/app";
import CustomerModule from "@/store/modules/customer";
import {
  BFormSelectOption,
  BTable,
  BvTableCtxObject,
  BvTableFieldArray,
  BvTableFormatterCallback,
} from "bootstrap-vue";
import App from "@/App.vue";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableFilterCriteria,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "@/components/HSMDataTable.vue";
import HSMCustomerDetail from "@/components/HSMCustomerDetail.vue";

interface CustomerTableItem extends HSMDataTableItem {
  visible: boolean;
  discharged: boolean;
  admissionDate: Date;
  dischargeDate: Date | null;
  note: string | null;
}

@Component({
  components: {
    "hsm-customer-detail": HSMCustomerDetail,
  },
})
export default class Customer extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "visible", label: "field.visible", hasCustomRenderer: true },
    {
      key: "admissionDate",
      label: "field.admissionDate",
      sortable: true,
      formatter: this.formatDate,
    },
    {
      key: "dischargeDate",
      label: "field.dischargeDate",
      sortable: true,
      formatter: this.formatDate,
    },
    {
      key: "note",
      label: "field.note",
      searchable: false,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "label.customer",
  };
  readonly criteriaOptions: HSMDataTableFilterCriteria[] = [
    { value: "id", text: "field.id" },
    { value: "name", text: "field.name" },
    { value: "admissionDate", text: "field.admissionDate" },
    { value: "dischargeDate", text: "field.dischargeDate" },
  ];

  get customers() {
    return CustomerModule.customers;
  }

  async fetchEntriesAsync() {
    return CustomerModule.fetchCustomersAsync();
  }

  async deleteEntryAsync(entry: HSMCustomer) {
    return CustomerModule.deleteCustomerAsync(entry.id);
  }

  async handleToggleVisibility(item: CustomerTableItem) {
    AppModule.showLoading("수정 중");

    await CustomerModule.toggleCustomerVisibilityAsync({
      id: item.id,
      visible: !item.visible,
    });

    AppModule.hideLoading();
  }

  mapTableItems(customer: HSMCustomer): CustomerTableItem {
    return {
      id: customer.id,
      name: customer.name,
      visible: !customer.hidden,
      discharged: customer.discharged,
      admissionDate: customer.admission_date,
      dischargeDate: customer.discharge_date ?? null,
      note: customer.note ?? null,
      action: {
        edit: true,
        delete: true,
      },
      raw: customer,
    };
  }

  formatDate(date: Date | null, key: string, item: any) {
    if (date) {
      return new Date(date).toLocaleDateString(this.$i18n.locale);
    }

    return "---";
  }

  rowClass(item: CustomerTableItem) {
    if (!item.visible) {
      return "table-secondary";
    }

    return;
  }
}
</script>

<style lang="scss" scoped></style>
>
