<template>
  <div id="customer">
    <div class="customer-list" v-show="!showDetail">
      <b-row class="justify-content-around pl-4"
        ><b-col cols="6" offset="2">
          <h2 class="font-weight-bold d-inline-block">
            <b-icon icon="person-lines-fill" class="mr-3"></b-icon>고객관리 ({{
              customers.length
            }})
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
            @click="reloadCustomers()"
          >
          </hsm-button>
        </b-col>
      </b-row>
      <hr />
      <b-row class="mt-2">
        <b-col cols="4" offset="2" cols-lg="6" offset-lg="2" class="px-4 pt-3">
          <div>
            <hsm-button
              size="sm"
              icon="tools"
              variant="secondary"
              textVariant="light"
              fontWeight="bold"
              v-b-toggle.collapseTableSettings
              >표 설정</hsm-button
            >
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
                      ></b-form-input></b-form-group></b-col></b-form-row
              ></b-card>
            </b-collapse>
          </div>
        </b-col>
        <b-col class="px-4 pt-3">
          <b-form>
            <b-form-row>
              <b-col cols="2">
                <b-form-select
                  v-model="criteria"
                  :options="criteriaOptions"
                ></b-form-select
              ></b-col>
              <b-col cols="4">
                <b-input-group>
                  <b-input-group-prepend is-text
                    ><b-icon icon="search"></b-icon
                  ></b-input-group-prepend>
                  <b-form-input
                    id="input-filter"
                    type="search"
                    v-model="filter"
                    placeholder="검색 필터"
                    debounce="300"
                  ></b-form-input
                ></b-input-group>
              </b-col>
            </b-form-row>
          </b-form>
        </b-col>
      </b-row>
      <b-row class="mt-3">
        <b-col cols="8" offset="2" cols-lg="6" offset-lg="2">
          <b-table
            :key="tableUpdateCount"
            id="customerTable"
            ref="customerTable"
            primary-key="id"
            sticky-header="460px"
            responsive="lg"
            :fields="fields"
            :items="customersProvider"
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
                  >수정</hsm-button
                >
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
                  >제거</hsm-button
                >
              </div></template
            >

            <template #table-busy>
              <div class="text-center text-dark my-2">
                <b-spinner class="align-middle mr-3"></b-spinner>
                <strong>데이터 로딩 중</strong>
              </div></template
            >

            <template #cell()="data">
              <div class="pt-3 font-weight-bold text-center">
                {{ data.value }}
              </div>
            </template>
          </b-table>
        </b-col>
      </b-row>
      <b-row class="justify-content-center">
        <b-col cols="2"
          ><b-pagination
            :total-rows="numRows"
            :per-page="perPage"
            v-model="currentPage"
            size="sm"
            align="fill"
          ></b-pagination></b-col
      ></b-row>
      <b-row class="mt-2 d-flex justify-content-around px-2">
        <b-col cols="4" class="d-flex justify-content-center"
          ><hsm-button
            v-if="!isBusy"
            size="sm"
            icon="person-plus-fill"
            variant="primary"
            textVariant="light"
            fontWeight="bold"
            @click="handleAddCustomer()"
            >고객 추가</hsm-button
          ></b-col
        >
        <b-col cols="5" cols-md="3" class="d-flex justify-content-center">
          <hsm-button
            v-if="selectedItems.length > 0"
            size="sm"
            icon="dash-circle"
            variant="secondary"
            textVariant="light"
            fontWeight="bold"
            @click="handleUnselectItems()"
            >선택 해제</hsm-button
          >
          <hsm-button
            v-if="selectedItems.length > 0"
            class="ml-2"
            size="sm"
            icon="person-dash-fill"
            variant="danger"
            textVariant="light"
            fontWeight="bold"
            @click="handleDeleteItems(selectedItems)"
            >선택 제거<b-badge variant="dark">{{
              selectedItems.length
            }}</b-badge></hsm-button
          ></b-col
        >
      </b-row>
    </div>
    <hsm-customer-detail
      v-show="showDetail"
      :customer="targetItem ? targetItem.raw : null"
      @save="reflectViewUpdates()"
      @delete="reflectViewUpdates()"
      @close="showDetail = false"
    ></hsm-customer-detail>
  </div>
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

interface CustomerTableItem {
  id: number;
  name: string;
  visible: boolean;
  discharged: boolean;
  admissionDate: Date;
  dischargeDate: Date | null;
  action: {
    edit: boolean;
    delete: boolean;
  };
  raw: HSMCustomer;
}

@Component({
  components: {},
})
export default class Customer extends Vue {
  tableUpdateCount = 0;
  fields: BvTableFieldArray = [
    { key: "id", label: "고객번호", sortable: true },
    { key: "name", label: "성명", sortable: true },
    { key: "visible", label: "숨김/보임" },
    {
      key: "admissionDate",
      label: "입소일",
      sortable: true,
      formatter: this.formatDate,
    },
    {
      key: "dischargeDate",
      label: "퇴소일",
      sortable: true,
      formatter: this.formatDate,
    },
    { key: "action", label: "동작" },
  ];
  selectedItems: CustomerTableItem[] = [];
  isBusy = false;
  perPage = 10;
  currentPage = 1;
  filter: string | null = "";
  filteredFields = ["id", "name", "admissionDate", "dischargeDate"];
  criteria: "id" | "name" | "admissionDate" | "dischargeDate" = "id";
  criteriaOptions = [
    { value: "id", text: "고객번호" },
    { value: "name", text: "성명" },
    { value: "admissionDate", text: "입원날짜" },
    { value: "dischargeDate", text: "퇴원날짜" },
  ];
  showDetail = false;
  targetItem: CustomerTableItem | null = null;

  $refs!: {
    customerTable: BTable;
  };

  get numRows() {
    return this.customers.length;
  }

  async created() {
    AppModule.showLoading("데이터 로딩 중");

    await CustomerModule.fetchCustomersAsync();
    this.reflectViewUpdates();

    AppModule.hideLoading();
  }

  get customers() {
    return CustomerModule.customers;
  }

  async customersProvider(ctx: BvTableCtxObject) {
    const startIndex = ctx.perPage * (ctx.currentPage - 1);
    const endIndex = startIndex + ctx.perPage;

    return this.mapTableItems(this.customers.slice(startIndex, endIndex));
  }

  mapTableItems(customers: HSMCustomer[]): CustomerTableItem[] {
    return customers.map(c => ({
      id: c.id,
      name: c.name,
      visible: !c.hidden,
      discharged: c.discharged,
      admissionDate: c.admission_date,
      dischargeDate: c.discharge_date ?? null,
      action: {
        edit: true,
        delete: true,
      },
      raw: c,
    }));
  }

  filterRows(item: CustomerTableItem, criteria: string) {
    if (this.filter) {
      return `${item[criteria]}`.includes(this.filter);
    }

    return true;
  }

  formatDate(date: Date | null, key: string, item: any) {
    if (date) {
      return new Date(date).toLocaleDateString("ko-KR");
    }

    return "---";
  }

  tableRowClass(item: any, type: string) {
    if (!item || type !== "row") {
      return;
    }

    if (!item.visible) {
      return "table-secondary";
    }
  }

  reflectViewUpdates() {
    this.tableUpdateCount += 1;
  }

  async reloadCustomers() {
    this.isBusy = true;
    await CustomerModule.fetchCustomersAsync();
    this.reflectViewUpdates();
    this.isBusy = false;
  }

  async handleToggleVisibility(item: CustomerTableItem) {
    AppModule.showLoading("수정 중");

    await CustomerModule.toggleCustomerVisibilityAsync({
      id: item.id,
      visible: !item.visible,
    });

    AppModule.hideLoading();
  }

  handleRowSelected(items: CustomerTableItem[]) {
    this.selectedItems = items;
  }

  handleAddCustomer() {
    this.targetItem = null;
    this.showDetail = true;
  }

  handleEditItemDetail(item: CustomerTableItem) {
    this.targetItem = item;
    this.showDetail = true;
  }

  async handleDeleteItems(items: CustomerTableItem[]) {
    const confirmed = await this.$hsmConfirmDialog(
      "고객 제거",
      "정말로 해당 고객들을 제거하시겠습니까?"
    );

    if (confirmed) {
      AppModule.showLoading("제거 중");

      for (const item of items) {
        await CustomerModule.deleteCustomerAsync(item.id);
      }
      this.reflectViewUpdates();

      AppModule.hideLoading();
    }
  }

  handleUnselectItems() {
    this.$refs.customerTable.clearSelected();
  }
}
</script>

<style lang="scss" scoped></style>>
