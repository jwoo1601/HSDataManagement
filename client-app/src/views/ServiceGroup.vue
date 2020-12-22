<template>
  <hsm-data-table
    header-icon="collection-fill"
    header="서비스 그룹"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="serviceGroups"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria="criteria"
    :criteria-options="criteriaOptions"
  >
    <template #cell(note)="data">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`serviceGroup-showNote-${data.index}`"
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
          v-if="data.value != null"
          :target="`serviceGroup-showNote-${data.index}`"
          placement="top"
          variant="success"
        >
          {{ data.value }}
        </b-tooltip>
      </div>
    </template>

    <template #detail="{ visible, item, notify, close }">
      <hsm-service-group-detail
        v-show="visible"
        :service-group="item"
        @save="notify"
        @delete="notify"
        @close="close"
      ></hsm-service-group-detail>
    </template>
  </hsm-data-table>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { HSMServiceGroup } from "@/models/api/Service";
import AppModule from "@/store/modules/app";
import ServiceGroupModule from "@/store/modules/serviceGroup";
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
import HSMServiceGroupDetail from "@/components/HSMServiceGroupDetail.vue";

interface ServiceGroupTableItem extends HSMDataTableItem {
  note: string | null;
  numServices: number;
}

@Component({
  components: {
    "hsm-service-group-detail": HSMServiceGroupDetail,
  },
})
export default class ServiceGroup extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "numServices", label: "등록된 서비스", sortable: true },
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "서비스 그룹",
  };

  criteria: "id" | "name" | "numServices" = "id";
  criteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "numServices", text: "서비스 갯수" },
  ];

  get serviceGroups() {
    return ServiceGroupModule.serviceGroups;
  }

  async fetchEntriesAsync() {
    return ServiceGroupModule.fetchServiceGroupsAsync();
  }

  async deleteEntryAsync(entry: HSMServiceGroup) {
    return ServiceGroupModule.deleteServiceGroupAsync(entry.id);
  }

  mapTableItems(serviceGroup: HSMServiceGroup): ServiceGroupTableItem {
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
}
</script>

<style lang="scss" scoped></style>
>
