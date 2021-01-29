<template>
  <hsm-data-table
    header-icon="collection-fill"
    :header="$t('tab.serviceGroup')"
    :field-definitions="fieldDefinitions"
    :fetch-entries-action="fetchEntriesAsync"
    :entries="serviceGroups"
    :entries-mapper="mapTableItems"
    :labels="labels"
    :delete-entry-action="deleteEntryAsync"
    :criteria-options="criteriaOptions"
  >
    <template #cell(note)="{ value, index }">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`serviceGroup-showNote-${index}`"
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
          v-if="value != null"
          :target="`serviceGroup-showNote-${index}`"
          placement="top"
          variant="success"
        >
          {{ value }}
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
  HSMDataTableFilterCriteria,
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
    { key: "numServices", label: "field.numServices", sortable: true },
    {
      key: "note",
      label: "field.note",
      searchable: false,
      hasCustomRenderer: true,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "label.group",
  };
  readonly criteriaOptions: HSMDataTableFilterCriteria[] = [
    { value: "id", text: "field.id" },
    { value: "name", text: "field.name" },
    { value: "numServices", text: "field.numServices" },
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
