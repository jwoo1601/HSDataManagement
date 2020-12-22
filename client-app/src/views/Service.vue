<template>
  <hsm-data-table
    header-icon="gear-fill"
    header="서비스"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="services"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria="criteria"
    :criteria-options="criteriaOptions"
  >
    <template #cell(note)="data">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`service-showNote-${data.index}`"
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
          :target="`service-showNote-${data.index}`"
          placement="top"
          variant="success"
        >
          {{ data.value }}
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
      label: "시간",
      sortable: true,
      formatter: this.formatTimeSpan,
    },
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
    {
      key: "group",
      label: "서비스 그룹",
      sortable: true,
      formatter: this.formatServiceGroup,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "서비스",
  };

  criteria: "id" | "name" | "duration" | "group" = "id";
  criteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "duration", text: "시간" },
    { value: "group", text: "서비스 그룹" },
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
    return timeSpan ? `${timeSpan.totalMinutes} 분` : "---";
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
