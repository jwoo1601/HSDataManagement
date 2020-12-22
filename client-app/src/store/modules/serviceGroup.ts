import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import {
  HSMServiceGroup,
  HSMServiceGroupInputModel,
} from "@/models/api/Service";
import IServiceService from "@/services/serviceService";

export interface ServiceGroupState {
  serviceGroups: HSMServiceGroup[];
  editedServiceGroup: HSMServiceGroup | null;
  serviceGroupDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "serviceGroup" })
class ServiceGroup extends VuexModule implements ServiceGroupState {
  @$inject()
  private serviceService!: IServiceService;

  public serviceGroups: HSMServiceGroup[] = [];
  public editedServiceGroup: HSMServiceGroup | null = null;
  public serviceGroupDetailDialog = false;

  @Mutation
  private SET_SERVICE_GROUPS(serviceGroups: HSMServiceGroup[]) {
    this.serviceGroups = serviceGroups;
  }

  @Mutation
  private SET_EDITED_SERVICE_GROUP(serviceGroup: HSMServiceGroup | null) {
    this.editedServiceGroup = serviceGroup;
  }

  @Mutation
  private SET_SERVICE_GROUP_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.serviceGroupDetailDialog = visible;
  }

  @Action
  public async fetchServiceGroupsAsync() {
    this.SET_SERVICE_GROUPS(await this.serviceService.getServiceGroupsAsync());
  }

  @Action
  public async addServiceGroupAsync(inputModel: HSMServiceGroupInputModel) {
    const response = await this.serviceService.createServiceGroupAsync(
      inputModel
    );
    if (response.isSuccess) {
      await this.fetchServiceGroupsAsync();
    }

    return response;
  }

  @Action
  public async editServiceGroupAsync(data: {
    id: number;
    inputModel: HSMServiceGroupInputModel;
  }) {
    const response = await this.serviceService.updateServiceGroupAsync(
      data.id,
      data.inputModel
    );
    if (response.isSuccess) {
      await this.fetchServiceGroupsAsync();
    }

    return response;
  }

  @Action
  public async deleteServiceGroupAsync(id: number) {
    const result = await this.serviceService.deleteServiceGroupAsync(id);
    if (result) {
      await this.fetchServiceGroupsAsync();
    }

    return result;
  }

  @Action
  public ClearServiceGroups() {
    this.SET_SERVICE_GROUPS([]);
  }

  @Action
  public showEditServiceGroupDialogFor(serviceGroup: HSMServiceGroup) {
    this.SET_EDITED_SERVICE_GROUP(serviceGroup);
    this.SET_SERVICE_GROUP_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeEditServiceGroupDialog() {
    this.SET_SERVICE_GROUP_DETAIL_DIALOG_VISIBLE(false);
  }

  @Action
  public showAddServiceGroupDialog() {
    this.SET_EDITED_SERVICE_GROUP(null);
    this.SET_SERVICE_GROUP_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeAddServiceGroupDialog() {
    this.SET_SERVICE_GROUP_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(ServiceGroup);
