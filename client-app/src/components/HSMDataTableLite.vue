<template>
  <b-table-lite
    :key="tableUpdateCount"
    class="hsm-data-table-lite"
    ref="dataTable"
    primary-key="id"
    sticky-header="460px"
    responsive="lg"
    :fields="fields"
    :items="items"
    head-variant="dark"
    thead-class="text-center"
    :tbody-tr-class="tableRowClass"
    hover
    outlined
  >
    <template #table-caption>{{ caption }}</template>

    <template #cell(id)="{ value }">
      <div class="d-flex justify-content-center pt-2">
        <b-badge variant="warning">{{ value }}</b-badge>
      </div>
    </template>

    <template
      v-for="field in customRenderedFields"
      v-slot:[`cell(${field.key})`]="data"
    >
      <slot :name="`cell(${field.key})`" v-bind="data"></slot>
    </template>

    <template v-if="showActions" #cell(action)="{ value, item }">
      <div class="d-flex justify-content-center pt-1">
        <hsm-button
          v-if="allowDelete && value.delete"
          size="sm"
          icon="dash-square-fill"
          variant="outline-danger"
          textVariant="danger"
          hoverVariant="danger"
          hoverTextVariant="light"
          fontWeight="bold"
          @click="handleDeleteItems([item])"
        >
          제거
        </hsm-button>
      </div>
    </template>

    <template #cell()="{ value }">
      <div class="pt-1 font-weight-bold text-center">
        {{ value }}
      </div>
    </template>
  </b-table-lite>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import AppModule from "@/store/modules/app";
import {
  BTable,
  BvTableCtxObject,
  BvTableField,
  BvTableFieldArray,
  BvTableFormatterCallback,
  BvTableProviderCallback,
} from "bootstrap-vue";
import { Emit, Prop, PropSync, Watch } from "vue-property-decorator";
import {
  HSMDataEntry,
  HSMDataTableLabels,
  DefaultHSMDataTableLabels,
} from "@/components/HSMDataTable.vue";

export type HSMDataTableLiteFieldDefinition = BvTableField & {
  key: string;
  hasCustomRenderer?: boolean;
  order?: number;
};

export interface HSMDataTableLiteItem {
  id: number;
  name: string;
  action: {
    delete: boolean;
  };
  raw: HSMDataEntry;
}

export type HSMDataTableLiteEntriesMapper = (
  entry: HSMDataEntry
) => HSMDataTableLiteItem;

@Component({
  components: {},
})
export default class HSMDataTableLite extends Vue {
  @Prop({ default: "" })
  readonly header!: string;
  @Prop({ default: "" })
  readonly headerIcon!: string;
  @Prop({ default: [] })
  readonly fieldDefinitions!: HSMDataTableLiteFieldDefinition[];
  @PropSync("entries", { default: [] })
  readonly syncedEntries!: HSMDataEntry[];
  @Prop({ default: null })
  readonly entriesMapper?: HSMDataTableLiteEntriesMapper | null;
  @Prop({ default: true })
  readonly showNumEntries!: boolean;
  @Prop({ default: DefaultHSMDataTableLabels })
  readonly labels!: HSMDataTableLabels;

  @Prop({ default: true })
  readonly showActions!: boolean;
  @Prop({ default: true })
  readonly allowDelete!: boolean;

  @Emit()
  deleteEntry(entry: HSMDataEntry) {}

  tableUpdateCount = 0;
  fields!: HSMDataTableLiteFieldDefinition[];
  items: HSMDataTableLiteItem[] = [];
  savedLabels!: HSMDataTableLabels;

  $refs!: {
    dataTable: BTable;
  };

  get numItems() {
    return this.items.length;
  }

  get caption() {
    return this.showNumEntries
      ? `${this.header} (${this.numItems})`
      : this.header;
  }

  generateFieldDefinitions() {
    function mapFieldDefinition(
      raw: BvTableField & { key: string }
    ): HSMDataTableLiteFieldDefinition {
      return { ...raw };
    }

    const defaultFields: HSMDataTableLiteFieldDefinition[] = [
      {
        key: "id",
        label: this.savedLabels.id,
        order: 0,
      },
      {
        key: "name",
        label: this.savedLabels.name,
        order: 1,
      },
    ];

    if (this.showActions) {
      defaultFields.push({
        key: "action",
        label: this.savedLabels.action,
        order: Number.MAX_SAFE_INTEGER,
      });
    }

    this.fields = defaultFields
      .map((f) => mapFieldDefinition(f))
      .concat(this.fieldDefinitions)
      .sort((a, b) => (a?.order ?? 2) - (b?.order ?? 2));
  }

  get customRenderedFields() {
    return this.fields.filter((f) => f.hasCustomRenderer);
  }

  mapDataEntries(rawEntries: HSMDataEntry[]): HSMDataTableLiteItem[] {
    const mapper = (e) => ({
      id: e.id,
      name: e.name,
      action: {
        delete: true,
      },
      raw: e,
    });

    return this.entriesMapper
      ? rawEntries.map(this.entriesMapper)
      : rawEntries.map(mapper);
  }

  loadItems() {
    this.items = this.mapDataEntries(this.syncedEntries);
  }

  created() {
    this.savedLabels = { ...DefaultHSMDataTableLabels, ...this.labels };
    AppModule.showLoading(`${this.savedLabels.data} 로딩 중`);

    this.generateFieldDefinitions();

    AppModule.hideLoading();
  }

  mounted() {
    this.loadItems();
  }

  @Watch("entries")
  onUpdateEntries(newEntries: HSMDataEntry[]) {
    this.loadItems();
  }

  notifyViewUpdates() {
    this.tableUpdateCount += 1;
  }

  tableRowClass(item: any, type: string) {
    if (!item || type !== "row") {
      return;
    }

    return;
  }

  async handleDeleteItems(items: HSMDataTableLiteItem[]) {
    if (this.allowDelete) {
      const confirmed = await this.$hsmConfirmDialog(
        `${this.savedLabels.item} 제거`,
        `정말로 해당 ${this.savedLabels.item}(들)을 제거하시겠습니까?`
      );

      if (confirmed) {
        AppModule.showLoading("제거 중");

        for (const item of items) {
          const idx = this.syncedEntries.findIndex(
            (it) => it.id === item.raw.id
          );
          if (idx !== -1) {
            this.deleteEntry(item.raw);
            this.syncedEntries.splice(idx, 1);
          }
        }

        this.loadItems();

        AppModule.hideLoading();
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>
>
