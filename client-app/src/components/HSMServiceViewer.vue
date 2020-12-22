<template>
  <hsm-data-viewer
    ref="dataViewer"
    :visible.sync="syncedVisible"
    :dialog-title="title"
    header-icon="gear-fill"
    header="서비스"
    :field-definitions="serviceFieldDefinitions"
    :fetch-entries-action="fetchServiceEntriesAsync"
    :entries="services"
    :entries-mapper="mapServiceTableItems"
    :labels="labels"
    :delete-entry-action="deleteServiceEntryAsync"
    :criteria="serviceCriteria"
    :criteria-options="serviceCriteriaOptions"
  >
    <template #cell(note)="data">
      <div class="d-flex justify-content-center text-center pt-1">
        <hsm-button
          :id="`view-service-showNote-${data.index}`"
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
          :target="`view-service-showNote-${data.index}`"
          placement="top"
          variant="success"
        >
          {{ data.value }}
        </b-tooltip>
      </div>
    </template>
  </hsm-data-viewer>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import {
  Emit,
  Inject,
  Prop,
  PropSync,
  Ref,
  Watch,
} from "vue-property-decorator";
import TimeSpan from "@/models/TimeSpan";
import AppModule from "@/store/modules/app";
import ServiceModule from "@/store/modules/service";
import HSMService, {
  HSMServiceGroup,
  HSMServiceInputModel,
} from "@/models/api/Service";
import {
  HSMDataTableFieldDefinition,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "./HSMDataTable.vue";
import HSMDataViewer from "./HSMDataViewer.vue";

interface ServiceViewTableItem extends HSMDataTableItem {
  note: string | null;
  duration: TimeSpan | null;
}

@Component({
  components: {},
})
export default class HSMServiceViewer extends Vue {
  @Prop({ default: "서비스 목록" })
  readonly title!: string;
  @Prop({ default: [] })
  readonly services!: HSMService[];

  @PropSync("visible", { default: false })
  syncedVisible!: boolean;

  readonly labels: HSMDataTableLabels = {
    item: "서비스",
  };
  readonly serviceFieldDefinitions: HSMDataTableFieldDefinition[] = [
    {
      key: "duration",
      label: "시간",
      sortable: true,
      formatter: this.formatTimeSpan,
    },
    { key: "note", label: "노트", searchable: false, hasCustomRenderer: true },
  ];
  serviceCriteria: "id" | "name" | "duration" = "id";
  serviceCriteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "duration", text: "시간" },
  ];

  async fetchServiceEntriesAsync() {
    return ServiceModule.fetchServicesAsync();
  }

  async deleteServiceEntryAsync(entry: HSMService) {
    return ServiceModule.deleteServiceAsync(entry.id);
  }

  mapServiceTableItems(service: HSMService): ServiceViewTableItem {
    return {
      id: service.id,
      name: service.name,
      note: service.note ?? null,
      duration: service.duration ?? null,
      action: {
        edit: false,
        delete: true,
      },
      raw: service,
    };
  }

  formatTimeSpan(timeSpan: TimeSpan | null, key: string, item: any) {
    return timeSpan ? `${timeSpan.totalMinutes} 분` : "---";
  }
}
</script>

<style lang="scss" scoped></style>
>
