<template>
  <hsm-data-table
    header-icon="gear-fill"
    :header="$t('tab.service')"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="services"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria-options="criteriaOptions"
  >
    <template #cell(note)="{ value, index }">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`service-showNote-${index}`"
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
          :target="`service-showNote-${index}`"
          placement="top"
          variant="success"
        >
          {{ value }}
        </b-tooltip>
      </div>
    </template>

    <template #cell(group)="{ value, unformatted }">
      <div class="d-flex justify-content-center text-center pt-1">
        <b-link :to="{ path: unformatted.name }">{{ value }}</b-link>
      </div>
    </template>

    <template #detail="{ visible, item, notify, close }">
      <hsm-service-detail
        v-show="visible"
        :service="item"
        @save="notify"
        @delete="notify"
        @close="close"
      ></hsm-service-detail>
    </template>
  </hsm-data-table>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import HSMService, { HSMServiceGroup } from "@/models/api/Service";
import AppModule from "@/store/modules/app";
import ServiceModule from "@/store/modules/service";
import {
  BFormSelectOption,
  BTable,
  BvTableCtxObject,
  BvTableFieldArray,
  BvTableFormatterCallback,
} from "bootstrap-vue";
import App from "@/App.vue";
import TimeSpan from "@/models/TimeSpan";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableFilterCriteria,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "@/components/HSMDataTable.vue";
import HSMServiceDetail from "@/components/HSMServiceDetail.vue";

interface ServiceTableItem extends HSMDataTableItem {
  note: string | null;
  duration: TimeSpan | null;
  group: HSMServiceGroup | null;
}

@Component({
  components: {
    "hsm-service-detail": HSMServiceDetail,
  },
})
export default class Service extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    {
      key: "duration",
      label: "field.duration",
      sortable: true,
      formatter: this.formatTimeSpan,
    },
    {
      key: "note",
      label: "field.note",
      searchable: false,
      hasCustomRenderer: true,
    },
    {
      key: "group",
      label: "field.serviceGroup",
      sortable: true,
      formatter: this.formatServiceGroup,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "label.service",
  };
  readonly criteriaOptions: HSMDataTableFilterCriteria[] = [
    { value: "id", text: "field.id" },
    { value: "name", text: "field.name" },
    { value: "duration", text: "field.duration" },
    { value: "group", text: "field.serviceGroup" },
  ];

  get services() {
    return ServiceModule.services;
  }

  async fetchEntriesAsync() {
    return ServiceModule.fetchServicesAsync();
  }

  async deleteEntryAsync(entry: HSMService) {
    return ServiceModule.deleteServiceAsync(entry.id);
  }

  mapTableItems(service: HSMService): ServiceTableItem {
    return {
      id: service.id,
      name: service.name,
      note: service.note ?? null,
      duration: service.duration ?? null,
      group: service.group ?? null,
      action: {
        edit: true,
        delete: true,
      },
      raw: service,
    };
  }

  formatTimeSpan(timeSpan: TimeSpan | null, key: string, item: any) {
    return timeSpan ? this.$n(timeSpan.totalMinutes, "minute") : "---";
  }

  formatServiceGroup(
    serviceGroup: HSMServiceGroup | null,
    key: string,
    item: any
  ) {
    return serviceGroup ? serviceGroup.name : "---";
  }
}
</script>

<style lang="scss" scoped></style>
>
