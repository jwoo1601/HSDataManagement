import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import HSMSecurityCode, {
  HSMSecurityCodeGenerateInputModel,
  HSMSecurityCodeInvalidateInputModel,
} from "@/models/api/SecurityCode";
import IAccountService from "@/services/accountService";

export interface SecurityCodeState {
  securityCodeList: HSMSecurityCode[];
  editedSecurityCode: HSMSecurityCode | null;
  securityCodeDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "securityCode" })
class SecurityCode extends VuexModule implements SecurityCodeState {
  @$inject()
  private accountService!: IAccountService;

  public securityCodeList: HSMSecurityCode[] = [];
  public editedSecurityCode: HSMSecurityCode | null = null;
  public securityCodeDetailDialog = false;

  @Mutation
  private SET_SECURITY_CODE_LIST(list: HSMSecurityCode[]) {
    this.securityCodeList = list;
  }

  @Mutation
  private SET_SECURITY_CODE_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.securityCodeDetailDialog = visible;
  }

  @Action
  public async fetchSecurityCodeListAsync() {
    this.SET_SECURITY_CODE_LIST(
      await this.accountService.getSecurityCodeListAsync()
    );
  }

  @Action
  public async generateSecurityCodeAsync(
    inputModel: HSMSecurityCodeGenerateInputModel
  ) {
    const response = await this.accountService.generateSecurityCodeAsync(
      inputModel
    );
    if (response.isSuccess) {
      await this.fetchSecurityCodeListAsync();
    }

    return response;
  }

  @Action
  public async invalidateSecurityCodeAsync(
    inputModel: HSMSecurityCodeInvalidateInputModel
  ) {
    const result = await this.accountService.invalidateSecurityCodeAsync(
      inputModel
    );
    if (result) {
      await this.fetchSecurityCodeListAsync();
    }

    return result;
  }

  @Action
  public ClearSecurityCodeList() {
    this.SET_SECURITY_CODE_LIST([]);
  }

  @Action
  public showGenerateSecurityCodeDialog() {
    this.SET_SECURITY_CODE_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeGenerateSecurityCodeDialog() {
    this.SET_SECURITY_CODE_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(SecurityCode);
