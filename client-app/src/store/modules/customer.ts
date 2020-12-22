import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import HSMCustomer, { HSMCustomerInputModel } from "@/models/api/Customer";
import ICustomerService from "@/services/customerService";

export interface CustomerState {
  customers: HSMCustomer[];
  editedCustomer: HSMCustomer | null;
  customerDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "customer" })
class Customer extends VuexModule implements CustomerState {
  @$inject()
  private customerService!: ICustomerService;

  public customers: HSMCustomer[] = [];
  public editedCustomer: HSMCustomer | null = null;
  public customerDetailDialog = false;

  @Mutation
  private SET_CUSTOMERS(customers: HSMCustomer[]) {
    this.customers = customers;
  }

  @Mutation
  private SET_CUSTOMER_VISIBILITY(data: { id: number; visible: boolean }) {
    this.customers.forEach((c) => {
      if (c.id === data.id) {
        c.hidden = !data.visible;
      }
    });
  }

  @Mutation
  private SET_CUSTOMER_DISCHARGED(data: {
    id: number;
    discharged: boolean;
    dischargeDate?: Date;
  }) {
    this.customers.forEach((c) => {
      if (c.id === data.id) {
        c.discharged = data.discharged;
        c.discharge_date = data.dischargeDate;
      }
    });
  }

  @Mutation
  private SET_EDITED_CUSTOMER(customer: HSMCustomer | null) {
    this.editedCustomer = customer;
  }

  @Mutation
  private SET_CUSTOMER_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.customerDetailDialog = visible;
  }

  @Action
  public async fetchCustomersAsync() {
    this.SET_CUSTOMERS(await this.customerService.getCustomersAsync());
  }

  @Action
  public async addCustomerAsync(inputModel: HSMCustomerInputModel) {
    const response = await this.customerService.createCustomerAsync(inputModel);
    if (response.isSuccess) {
      await this.fetchCustomersAsync();
    }

    return response;
  }

  @Action
  public async editCustomerAsync(data: {
    id: number;
    inputModel: HSMCustomerInputModel;
  }) {
    const response = await this.customerService.updateCustomerAsync(
      data.id,
      data.inputModel
    );
    if (response.isSuccess) {
      await this.fetchCustomersAsync();
    }

    return response;
  }

  @Action
  public async deleteCustomerAsync(id: number) {
    const result = await this.customerService.deleteCustomerAsync(id);
    if (result) {
      await this.fetchCustomersAsync();
    }

    return result;
  }

  @Action
  public async toggleCustomerVisibilityAsync(data: {
    id: number;
    visible: boolean;
  }) {
    const response = await this.customerService.setCustomerVisibilityAsync(
      data.id,
      data.visible
    );
    if (response.isSuccess) {
      this.SET_CUSTOMER_VISIBILITY(data);
    }

    return response;
  }

  @Action
  public async toggleCustomerDischargedAsync(data: {
    id: number;
    discharged: boolean;
    dischargeDate?: Date;
  }) {
    const response = await this.customerService.setCustomerDischargedAsync(
      data.id,
      data.discharged,
      data.dischargeDate
    );
    if (response.isSuccess) {
      this.SET_CUSTOMER_DISCHARGED(data);
    }

    return response;
  }

  @Action
  public ClearCustomers() {
    this.SET_CUSTOMERS([]);
  }

  @Action
  public showEditCustomerDialogFor(customer: HSMCustomer) {
    this.SET_EDITED_CUSTOMER(customer);
    this.SET_CUSTOMER_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeEditCustomerDialog() {
    this.SET_CUSTOMER_DETAIL_DIALOG_VISIBLE(false);
  }

  @Action
  public showAddCustomerDialog() {
    this.SET_EDITED_CUSTOMER(null);
    this.SET_CUSTOMER_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeAddCustomerDialog() {
    this.SET_CUSTOMER_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(Customer);
