import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import HSMService, { HSMServiceInputModel } from "@/models/api/Service";
import IServiceService from "@/services/serviceService";

export interface ServiceState {
  services: HSMService[];
  editedService: HSMService | null;
  serviceDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "service" })
class Service extends VuexModule implements ServiceState {
  @$inject()
  private serviceService!: IServiceService;

  public services: HSMService[] = [];
  public editedService: HSMService | null = null;
  public serviceDetailDialog = false;

  @Mutation
  private SET_SERVICES(services: HSMService[]) {
    this.services = services;
  }

  @Mutation
  private SET_EDITED_SERVICE(service: HSMService | null) {
    this.editedService = service;
  }

  @Mutation
  private SET_SERVICE_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.serviceDetailDialog = visible;
  }

  @Action
  public async fetchServicesAsync() {
    this.SET_SERVICES(await this.serviceService.getServicesAsync());
  }

  @Action
  public async addServiceAsync(inputModel: HSMServiceInputModel) {
    const response = await this.serviceService.createServiceAsync(inputModel);
    if (response.isSuccess) {
      await this.fetchServicesAsync();
    }

    return response;
  }

  @Action
  public async editServiceAsync(data: {
    id: number;
    inputModel: HSMServiceInputModel;
  }) {
    const response = await this.serviceService.updateServiceAsync(
      data.id,
      data.inputModel
    );
    if (response.isSuccess) {
      await this.fetchServicesAsync();
    }

    return response;
  }

  @Action
  public async deleteServiceAsync(id: number) {
    const result = await this.serviceService.deleteServiceAsync(id);
    if (result) {
      await this.fetchServicesAsync();
    }

    return result;
  }

  @Action
  public ClearServices() {
    this.SET_SERVICES([]);
  }

  @Action
  public showEditServiceDialogFor(service: HSMService) {
    this.SET_EDITED_SERVICE(service);
    this.SET_SERVICE_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeEditServiceDialog() {
    this.SET_SERVICE_DETAIL_DIALOG_VISIBLE(false);
  }

  @Action
  public showAddServiceDialog() {
    this.SET_EDITED_SERVICE(null);
    this.SET_SERVICE_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeAddServiceDialog() {
    this.SET_SERVICE_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(Service);
