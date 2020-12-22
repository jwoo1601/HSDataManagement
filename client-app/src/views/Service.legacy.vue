<template>
  <div id="service">
    <div class="service-list" v-show="!showDetail">
      <b-row class="justify-content-around pl-5">
        <b-col cols="9">
          <h2 class="font-weight-bold d-inline-block">
            <!-- <b-icon icon="person-lines-fill" class="mr-3"></b-icon> -->
            서비스 ({{ services.length }})
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
            @click="reloadItems()"
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
              표 설정
            </hsm-button>
            <b-collapse id="collapseTableSettings" class="mt-2">
              <b-card class="py-0">
                <b-form-row>
                  <b-col cols="6">
                    <b-form-group
                      id="group-perPage"
                      label="페이지 당 표시 갯수"
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
        <b-col class="pt-2">
          <b-form>
            <b-form-row>
              <b-col cols="3" offset="1">
                <b-form-select
                  v-model="criteria"
                  :options="criteriaOptions"
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
                    v-model="filter"
                    placeholder="검색 필터"
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
            id="serviceTable"
            ref="serviceTable"
            primary-key="id"
            sticky-header="460px"
            responsive="lg"
            :fields="fields"
            :items="itemsProvider"
            :busy.sync="isBusy"
            :current-page="currentPage"
            :per-page="perPage"
            head-variant="dark"
            thead-class="text-center"
            :tbody-tr-class="tableRowClass"
            selected-variant="warning"
            hover
            selectable
            show-empty
            outlined
            sort-by="id"
            :sort-null-last="true"
            :filter="criteria"
            :filter-included-fields="filteredFields"
            :filter-function="filterRows"
            empty-html="<strong>표시할 데이터가 없습니다.</strong>"
            empty-filtered-html="<strong>조건에 맞는 데이터가 없습니다.</strong>"
            @row-selected="handleRowSelected"
            no-provider-filtering
            no-provider-sorting
          >
            <template #cell(id)="data">
              <div class="d-flex justify-content-center pt-3">
                <b-badge variant="warning">{{ data.value }}</b-badge>
              </div>
            </template>

            <template #cell(note)="data">
              <div class="d-flex justify-content-center text-center pb-3">
                <hsm-button
                  :id="`showNote-${data.index}`"
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
                  :target="`showNote-${data.index}`"
                  placement="bottom"
                  variant="success"
                >
                  {{ data.value }}
                </b-tooltip>
              </div>
            </template>

            <template #cell(group)="data">
              <div class="d-flex justify-content-center text-center pb-3">
                <b-link>{{ data.value ? data.value.name : "---" }}</b-link>
              </div>
            </template>

            <template #cell(action)="data">
              <div class="d-flex justify-content-center pt-3">
                <hsm-button
                  v-if="data.value.edit"
                  class="mr-3"
                  size="sm"
                  icon="pencil-fill"
                  variant="outline-primary"
                  textVariant="primary"
                  hoverVariant="primary"
                  hoverTextVariant="light"
                  fontWeight="bold"
                  @click="handleEditItemDetail(data.item)"
                >
                  수정
                </hsm-button>
                <hsm-button
                  v-if="data.value.delete"
                  size="sm"
                  icon="trash-fill"
                  variant="outline-danger"
                  textVariant="danger"
                  hoverVariant="danger"
                  hoverTextVariant="light"
                  fontWeight="bold"
                  @click="handleDeleteItems([data.item])"
                >
                  제거
                </hsm-button>
              </div>
            </template>

            <template #table-busy>
              <div class="text-center text-dark my-2">
                <b-spinner class="align-middle mr-3"></b-spinner>
                <strong>데이터 로딩 중</strong>
              </div>
            </template>

            <template #cell()="data">
              <div class="pt-3 font-weight-bold text-center">
                {{ data.value }}
              </div>
            </template>
          </b-table>
        </b-col>
      </b-row>
      <b-row class="justify-content-center">
        <b-col cols="2">
          <b-pagination
            :total-rows="numRows"
            :per-page="perPage"
            v-model="currentPage"
            size="sm"
            align="fill"
          ></b-pagination>
        </b-col>
      </b-row>
      <b-row class="d-flex justify-content-between">
        <b-col cols="4" class="d-flex justify-content-center">
          <hsm-button
            v-if="!isBusy"
            size="sm"
            icon="person-plus-fill"
            variant="primary"
            textVariant="light"
            fontWeight="bold"
            @click="handleAddItem()"
          >
            서비스 추가
          </hsm-button>
        </b-col>
        <b-col cols="5" cols-md="3" class="d-flex justify-content-center">
          <hsm-button
            v-if="selectedItems.length > 0"
            size="sm"
            icon="dash-circle"
            variant="secondary"
            textVariant="light"
            fontWeight="bold"
            @click="handleUnselectItems()"
          >
            선택 해제
          </hsm-button>
          <hsm-button
            v-if="selectedItems.length > 0"
            class="ml-2"
            size="sm"
            icon="person-dash-fill"
            variant="danger"
            textVariant="light"
            fontWeight="bold"
            @click="handleDeleteItems(selectedItems)"
          >
            선택 제거
            <b-badge variant="dark">{{ selectedItems.length }}</b-badge>
          </hsm-button>
        </b-col>
      </b-row>
    </div>
    <hsm-service-group-detail
      v-show="showDetail"
      :service-group="targetItem ? targetItem.raw : null"
      @save="reflectViewUpdates()"
      @delete="reflectViewUpdates()"
      @close="showDetail = false"
    ></hsm-service-group-detail>
  </div>
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
import HSMServiceGroupDetail from "@/components/HSMServiceGroupDetail.vue";

interface ServiceTableItem {
  id: number;
  name: string;
  note: string | null;
  duration: TimeSpan | null;
  group: HSMServiceGroup | null;
  action: {
    edit: boolean;
    delete: boolean;
  };
  raw: HSMService;
}

@Component({
  components: { "hsm-service-group-detail": HSMServiceGroupDetail },
})
export default class Service extends Vue {
  tableUpdateCount = 0;
  fields: BvTableFieldArray = [
    { key: "id", label: "연번", sortable: true },
    { key: "name", label: "이름", sortable: true },
    {
      key: "duration",
      label: "시간",
      sortable: true,
      formatter: this.formatTimeSpan,
    },
    { key: "note", label: "노트" },
    { key: "group", label: "서비스 그룹", sortable: true },
    { key: "action", label: "동작" },
  ];
  selectedItems: ServiceTableItem[] = [];
  isBusy = false;
  perPage = 10;
  currentPage = 1;
  filter: string | null = "";
  criteria: "id" | "name" | "duration" | "group" = "id";
  criteriaOptions = [
    { value: "id", text: "연번" },
    { value: "name", text: "이름" },
    { value: "duration", text: "시간" },
    { value: "group", text: "서비스 그룹" },
  ];
  filteredFields = this.criteriaOptions.map((o) => o.value);
  showDetail = false;
  targetItem: ServiceTableItem | null = null;

  $refs!: {
    serviceTable: BTable;
  };

  get numRows() {
    return this.services.length;
  }

  async created() {
    AppModule.showLoading("데이터 로딩 중");

    await ServiceModule.fetchServicesAsync();
    this.reflectViewUpdates();

    AppModule.hideLoading();
  }

  get services() {
    return ServiceModule.services;
  }

  async itemsProvider(ctx: BvTableCtxObject) {
    const startIndex = ctx.perPage * (ctx.currentPage - 1);
    const endIndex = startIndex + ctx.perPage;

    // console.log(typeof this.services[0].duration);

    return this.mapTableItems(this.services.slice(startIndex, endIndex));
  }

  mapTableItems(services: HSMService[]): ServiceTableItem[] {
    return services.map((s) => ({
      id: s.id,
      name: s.name,
      duration: s.duration ?? null,
      note: s.note ?? null,
      group: s.group ?? null,
      action: {
        edit: true,
        delete: true,
      },
      raw: s,
    }));
  }

  filterRows(item: ServiceTableItem, criteria: string) {
    if (this.filter) {
      return `${item[criteria]}`.includes(this.filter);
    }

    return true;
  }

  formatTimeSpan(timeSpan: TimeSpan | null, key: string, item: any) {
    if (timeSpan) {
      console.log(timeSpan instanceof TimeSpan);
      return `${timeSpan.totalMinutes} 분`;
    }

    return "---";
  }

  tableRowClass(item: any, type: string) {
    if (!item || type !== "row") {
      return;
    }

    return;
  }

  reflectViewUpdates() {
    this.tableUpdateCount += 1;
  }

  async reloadItems() {
    this.isBusy = true;
    await ServiceModule.fetchServicesAsync();
    this.reflectViewUpdates();
    this.isBusy = false;
  }

  handleRowSelected(items: ServiceTableItem[]) {
    this.selectedItems = items;
  }

  handleAddItem() {
    console.log(`haha; ${this.services[0].duration?.hours}`);
    this.targetItem = null;
    this.showDetail = true;
  }

  handleEditItemDetail(item: ServiceTableItem) {
    this.targetItem = item;
    this.showDetail = true;
  }

  async handleDeleteItems(items: ServiceTableItem[]) {
    const confirmed = await this.$hsmConfirmDialog(
      "서비스 제거",
      "정말로 해당 서비스들을 제거하시겠습니까?"
    );

    if (confirmed) {
      AppModule.showLoading("제거 중");

      for (const item of items) {
        await ServiceModule.deleteServiceAsync(item.id);
      }
      this.reflectViewUpdates();

      AppModule.hideLoading();
    }
  }

  handleUnselectItems() {
    this.$refs.serviceTable.clearSelected();
  }
}
</script>

<style lang="scss" scoped></style>
>
