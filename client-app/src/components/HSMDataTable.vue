<template>
  <div id="hsm-data-table-component">
    <div class="hsm-data-table-wrapper" v-show="!showItemDetail">
      <b-row class="justify-content-around pl-5">
        <b-col cols="9">
          <h2 class="font-weight-bold d-inline-block">
            <b-icon :icon="headerIcon" class="mr-3"></b-icon>
            {{ pageHeader }}
          </h2>
        </b-col>
        <b-col offset="1">
          <hsm-button
            icon="arrow-counterclockwise"
            size="md"
            variant="success"
            textVariant="light"
            fontWeight="bold"
            spacing="0"
            @click="handleReloadItemsAsync()"
          ></hsm-button>
        </b-col>
      </b-row>
      <hr />
      <b-row class="mt-2 px-4">
        <b-col cols="6" class="pt-3">
          <div>
            <hsm-button
              size="sm"
              icon="tools"
              variant="secondary"
              textVariant="light"
              fontWeight="bold"
              v-b-toggle.collapseTableSettings
            >
              {{ $t("action.tableSettings") }}
            </hsm-button>
            <b-collapse id="collapseTableSettings" class="mt-2">
              <b-card class="py-0">
                <b-form-row>
                  <b-col cols="6">
                    <b-form-group
                      id="group-perPage"
                      :label="$t('label.perPage')"
                      label-for="input-perPage"
                    >
                      <b-form-input
                        id="input-perPage"
                        type="text"
                        v-model="perPage"
                        number
                      ></b-form-input>
                    </b-form-group>
                  </b-col>
                </b-form-row>
              </b-card>
            </b-collapse>
          </div>
        </b-col>
        <b-col v-if="allowSearching" class="pt-2">
          <b-form>
            <b-form-row>
              <b-col cols="3" offset="1">
                <b-form-select
                  v-model="filterCriteria"
                  :options="filterCriteriaOptions"
                ></b-form-select>
              </b-col>
              <b-col cols="7">
                <b-input-group>
                  <b-input-group-prepend is-text>
                    <b-icon icon="search"></b-icon>
                  </b-input-group-prepend>
                  <b-form-input
                    id="input-filter"
                    type="search"
                    v-model="filterString"
                    :placeholder="$t('label.searchFilter')"
                    debounce="300"
                  ></b-form-input>
                </b-input-group>
              </b-col>
            </b-form-row>
          </b-form>
        </b-col>
      </b-row>
      <b-row class="mt-3 px-4">
        <b-col cols="12">
          <b-table
            :key="tableUpdateCount"
            id="hsm-data-table"
            ref="dataTable"
            primary-key="id"
            sticky-header="460px"
            responsive="lg"
            :fields="tableFields"
            :items="itemsProvider"
            :busy.sync="isBusy"
            :current-page="currentPage"
            :per-page="perPage"
            head-variant="dark"
            thead-class="text-center"
            :tbody-tr-class="tableRowClass"
            hover
            show-empty
            outlined
            sort-by="id"
            :sort-null-last="true"
            :filter="filterCriteria"
            :filter-included-fields="filteredFields"
            :filter-function="filterItems"
            :empty-html="emptyTableText"
            :empty-filtered-html="emptyFilteredTableText"
            :selectable="allowSelect"
            :select-mode="selectMode"
            selected-variant="warning"
            @row-selected="handleItemSelected"
            no-provider-filtering
            no-provider-sorting
          >
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
                  v-if="allowEdit && value.edit"
                  class="mr-3"
                  size="sm"
                  icon="pencil-fill"
                  variant="outline-primary"
                  textVariant="primary"
                  hoverVariant="primary"
                  hoverTextVariant="light"
                  fontWeight="bold"
                  @click="handleEditItemDetail(item)"
                >
                  {{ $t("action.edit") }}
                </hsm-button>
                <hsm-button
                  v-if="allowDelete && value.delete"
                  size="sm"
                  icon="trash-fill"
                  variant="outline-danger"
                  textVariant="danger"
                  hoverVariant="danger"
                  hoverTextVariant="light"
                  fontWeight="bold"
                  @click="handleDeleteItems([item])"
                >
                  {{ $t("action.remove") }}
                </hsm-button>
              </div>
            </template>

            <template #table-busy>
              <div class="text-center text-dark my-2">
                <b-spinner class="align-middle mr-3"></b-spinner>
                <strong>
                  {{ $t("loading.data") }}
                </strong>
              </div>
            </template>

            <template #cell()="{ value }">
              <div class="pt-1 font-weight-bold text-center">
                {{ value }}
              </div>
            </template>
          </b-table>
        </b-col>
      </b-row>

      <b-row class="justify-content-center">
        <b-col cols="2">
          <b-pagination
            :total-rows="numEntries"
            :per-page="perPage"
            v-model="currentPage"
            size="sm"
            align="fill"
          ></b-pagination>
        </b-col>
      </b-row>
      <b-row class="d-flex justify-content-between">
        <b-col cols="3" class="pl-5">
          <hsm-button
            v-if="allowAdd && !isBusy"
            size="sm"
            icon="patch-plus-fll"
            variant="primary"
            textVariant="light"
            fontWeight="bold"
            spacing="1"
            @click="handleAddItem()"
          >
            {{ $t("action.add") }}
          </hsm-button>
        </b-col>
        <b-col cols="5" cols-md="3" class="pr-5 d-flex justify-content-end">
          <slot name="selectionActions"></slot>
          <hsm-button
            v-if="selectedItems.length > 0"
            class="mr-2"
            size="sm"
            icon="dash-circle"
            variant="secondary"
            textVariant="light"
            fontWeight="bold"
            spacing="1"
            @click="handleUnselectItems()"
          >
            {{ $t("action.unselect") }}
          </hsm-button>
          <hsm-button
            v-if="allowDelete && selectedItems.length > 0"
            size="sm"
            icon="patch-minus-fll"
            variant="danger"
            textVariant="light"
            fontWeight="bold"
            spacing="1"
            @click="handleDeleteItems(selectedItems)"
          >
            {{ $t("action.removeSelected") }}
            <b-badge variant="dark">
              {{ selectedItems.length }}
            </b-badge>
          </hsm-button>
        </b-col>
      </b-row>
    </div>
    <slot
      name="detail"
      :visible="showItemDetail"
      :notify="notifyViewUpdates"
      :close="handleCloseItemDetail"
      :item="targetItem ? targetItem.raw : null"
    ></slot>
  </div>
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
import { Emit, Prop, Watch } from "vue-property-decorator";

export type HSMDataTableFieldDefinition = BvTableField & {
  key: string;
  searchable?: boolean;
  hasCustomRenderer?: boolean;
  order?: number;
};

export interface HSMDataEntry {
  id: any;
  name: string;
}

export interface HSMDataTableItem {
  id: any;
  name: string;
  action: {
    edit: boolean;
    delete: boolean;
  };
  raw: HSMDataEntry;
}

export type HSMDataTableEntriesFetcher = () => Promise<void>;
export type HSMDataTableEntriesMapper = (
  entry: HSMDataEntry
) => HSMDataTableItem;

export interface HSMDataTableLabels {
  data?: string;
  item?: string;
  id?: string;
  name?: string;
  action?: string;
}

export const DefaultHSMDataTableLabels: HSMDataTableLabels = {
  data: "label.data",
  item: "field.item",
  id: "field.id",
  name: "field.name",
  action: "field.actions",
};

export interface HSMDataTableFilterCriteria {
  value: string;
  text: string;
  filter?: (
    item: HSMDataTableItem,
    criteria: string,
    filterString: string
  ) => boolean;
}

@Component({
  components: {},
})
export default class HSMDataTable extends Vue {
  @Prop({ default: "" })
  readonly header!: string;
  @Prop({ default: "" })
  readonly headerIcon!: string;
  @Prop({ default: [] })
  readonly fieldDefinitions!: HSMDataTableFieldDefinition[];
  @Prop({ default: true })
  readonly fetchOnLoad!: boolean;
  @Prop({ default: null })
  readonly fetchEntriesAction?: HSMDataTableEntriesFetcher | null;
  @Prop({ default: [] })
  readonly entries!: HSMDataEntry[];
  @Prop({ default: null })
  readonly entriesMapper?: HSMDataTableEntriesMapper | null;
  @Prop({ default: true })
  readonly showNumEntries!: boolean;
  @Prop({ default: DefaultHSMDataTableLabels })
  readonly labels!: HSMDataTableLabels;
  @Prop({ default: [] })
  readonly criteriaOptions!: HSMDataTableFilterCriteria[];
  @Prop({ default: 10 })
  readonly perPage!: number;
  @Prop({ default: 1 })
  readonly currentPage!: number;
  @Prop({ default: "range" })
  readonly selectMode!: string;
  @Prop({ default: null })
  readonly rowClass!: (item: HSMDataTableItem) => string;

  @Prop({ default: true })
  readonly allowSelect!: boolean;
  @Prop({ default: true })
  readonly allowAdd!: boolean;
  @Prop({ default: true })
  readonly allowEdit!: boolean;
  @Prop({ default: true })
  readonly allowDelete!: boolean;
  @Prop({ default: true })
  readonly allowSorting!: boolean;
  @Prop({ default: true })
  readonly allowSearching!: boolean;
  @Prop({ default: null })
  readonly deleteEntryAction?: (entry: HSMDataEntry) => Promise<boolean> | null;
  @Prop({ default: true })
  readonly showActions!: boolean;

  @Emit()
  addEntry() {}
  @Emit()
  editEntry(entry: HSMDataEntry) {}
  @Emit()
  deleteEntry(entry: HSMDataEntry) {}

  tableUpdateCount = 0;
  isBusy = false;
  tableFields!: HSMDataTableFieldDefinition[];
  dataEntries: HSMDataTableItem[] = [];
  filterCriteria = "id";
  filterString: string | null = "";
  filteredFields!: string[];
  targetItem: HSMDataTableItem | null = null;
  selectedItems: HSMDataTableItem[] = [];
  savedLabels!: HSMDataTableLabels;
  showItemDetail = false;

  $refs!: {
    dataTable: BTable;
  };

  get numEntries() {
    return this.dataEntries.length;
  }

  get pageHeader() {
    return this.showNumEntries
      ? `${this.header} (${this.numEntries})`
      : this.header;
  }

  get filterCriteriaOptions() {
    return this.criteriaOptions.map((opt) => ({
      ...opt,
      text: this.$t(opt.text),
    }));
  }

  get currentLocale() {
    return AppModule.currentLocale;
  }

  generateFieldDefinitions() {
    const translate = this.$t.bind(this);

    function mapFieldDefinition(
      raw: BvTableField & { key: string }
    ): HSMDataTableFieldDefinition {
      return {
        searchable: true,
        ...raw,
        label: translate(raw.label ?? "").toString(),
      };
    }

    const defaultFields: HSMDataTableFieldDefinition[] = [
      {
        key: "id",
        label: this.savedLabels.id,
        sortable: this.allowSorting,
        order: 0,
      },
      {
        key: "name",
        label: this.savedLabels.name,
        sortable: this.allowSorting,
        order: 1,
      },
    ];

    if (this.showActions) {
      defaultFields.push({
        key: "action",
        label: this.savedLabels.action,
        searchable: false,
        order: Number.MAX_SAFE_INTEGER,
      });
    }

    this.tableFields = defaultFields
      .map((f) => mapFieldDefinition(f))
      .concat(
        this.fieldDefinitions.map((fd) => ({
          ...fd,
          label: this.$t(fd.label ?? "").toString(),
        }))
      )
      .sort((a, b) => (a?.order ?? 2) - (b?.order ?? 2));
  }

  get customRenderedFields() {
    return this.tableFields.filter((f) => f.hasCustomRenderer);
  }

  async fetchEntriesAsync() {
    if (this.fetchEntriesAction) {
      await this.fetchEntriesAction();
    }
  }

  mapDataEntries(rawEntries: HSMDataEntry[]): HSMDataTableItem[] {
    const mapper = (e) => ({
      id: e.id,
      name: e.name,
      action: {
        edit: true,
        delete: true,
      },
      raw: e,
    });

    return this.entriesMapper
      ? rawEntries.map(this.entriesMapper)
      : rawEntries.map(mapper);
  }

  async fetchItemsAsync() {
    this.isBusy = true;
    await this.fetchEntriesAsync();
    this.dataEntries = this.mapDataEntries(this.entries);
    this.isBusy = false;
  }

  created() {
    this.isBusy = true;
    this.savedLabels = { ...DefaultHSMDataTableLabels, ...this.labels };
    AppModule.showLoading(this.$t("loading.data").toString());

    this.generateFieldDefinitions();
    this.filteredFields = this.criteriaOptions.map((opt) => opt.value);

    AppModule.hideLoading();
  }

  mounted() {
    if (this.fetchOnLoad) {
      this.handleReloadItemsAsync();
    } else {
      this.dataEntries = this.mapDataEntries(this.entries);
    }
  }

  @Watch("entries")
  onUpdateEntries(newEntries: HSMDataEntry[]) {
    this.dataEntries = this.mapDataEntries(newEntries);
  }

  @Watch("currentLocale")
  onLocaleChanged() {
    this.generateFieldDefinitions();
  }

  async itemsProvider(ctx: BvTableCtxObject) {
    const startIndex = ctx.perPage * (ctx.currentPage - 1);
    const endIndex = startIndex + ctx.perPage;

    return this.dataEntries.slice(startIndex, endIndex);
  }

  notifyViewUpdates() {
    this.handleUnselectItems();
    this.tableUpdateCount += 1;
  }

  filterItems(item: HSMDataTableItem, criteria: string) {
    if (this.allowSearching && this.filterString) {
      const matchingOption = this.filterCriteriaOptions.find(
        (opt) => opt.value === criteria
      );

      return matchingOption?.filter
        ? matchingOption.filter(item, criteria, this.filterString)
        : `${item[criteria]}`.includes(this.filterString);
    }

    return true;
  }

  tableRowClass(item: HSMDataTableItem, type: string) {
    if (!item || type !== "row") {
      return;
    }

    if (this.rowClass) {
      return this.rowClass(item);
    }

    return;
  }

  setItemDetailVisible(visible: boolean) {
    this.showItemDetail = visible;
  }

  get emptyTableText() {
    return `<strong>${this.$t("message.tableEmpty")}</strong>`;
  }

  get emptyFilteredTableText() {
    return `<strong>${this.$t("message.tableNoMatch")}</strong>`;
  }

  async handleReloadItemsAsync() {
    this.isBusy = true;
    await this.fetchItemsAsync();
    this.notifyViewUpdates();
    this.isBusy = false;
  }

  handleAddItem() {
    this.addEntry();

    this.targetItem = null;
    this.setItemDetailVisible(true);
  }

  handleEditItemDetail(item: HSMDataTableItem) {
    this.editEntry(item.raw);

    this.targetItem = item;
    this.setItemDetailVisible(true);
  }

  async handleDeleteItems(items: HSMDataTableItem[]) {
    if (this.deleteEntryAction) {
      const confirmed = await this.$hsmConfirmDialog(
        this.$t("action.remove").toString(),
        `정말로 해당 항목(들)을 제거하시겠습니까?`
      );

      if (confirmed) {
        AppModule.showLoading(this.$t("loading.processing").toString());

        for (const item of items) {
          this.deleteEntry(item.raw);
          await this.deleteEntryAction(item.raw);
        }

        AppModule.hideLoading();

        await this.handleReloadItemsAsync();
      }
    }
  }

  handleItemSelected(items: HSMDataTableItem[]) {
    this.selectedItems = items;
  }

  handleUnselectItems() {
    this.$refs.dataTable.clearSelected();
    this.selectedItems = [];
  }

  handleCloseItemDetail() {
    this.setItemDetailVisible(false);
  }
}
</script>

<style lang="scss" scoped></style>
>
