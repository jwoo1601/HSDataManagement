import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import HSMUser, { HSMUserSetRoleInputModel } from "@/models/api/User";
import IUserService from "@/services/userService";

export interface UserState {
  users: HSMUser[];
  editedUser: HSMUser | null;
  userDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "user" })
class User extends VuexModule implements UserState {
  @$inject()
  private userService!: IUserService;

  public users: HSMUser[] = [];
  public editedUser: HSMUser | null = null;
  public userDetailDialog = false;

  @Mutation
  private SET_USERS(users: HSMUser[]) {
    this.users = users;
  }

  @Mutation
  private SET_USER_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.userDetailDialog = visible;
  }

  @Action
  public async fetchUsersAsync() {
    this.SET_USERS(await this.userService.getUsersAsync());
  }

  @Action
  public async getUserDataAsync(id: string) {
    return await this.userService.getUserByIdAsync(id);
  }

  @Action
  public async activateUserAsync(id: string) {
    const response = await this.userService.activateUserAsync(id);
    if (response.isSuccess) {
      await this.fetchUsersAsync();
    }

    return response;
  }

  @Action
  public async inactivateUserAsync(id: string) {
    const response = await this.userService.inactivateUserAsync(id);
    if (response.isSuccess) {
      await this.fetchUsersAsync();
    }

    return response;
  }

  @Action
  public async markEmailConfirmedAsync(id: string) {
    const response = await this.userService.markEmailConfirmedAsync(id);
    if (response.isSuccess) {
      await this.fetchUsersAsync();
    }

    return response;
  }

  @Action
  public async endLockoutAsync(id: string) {
    const response = await this.userService.endLockoutAsync(id);
    if (response.isSuccess) {
      await this.fetchUsersAsync();
    }

    return response;
  }

  @Action
  public async setUserRoleAsync(data: {
    id: string;
    inputModel: HSMUserSetRoleInputModel;
  }) {
    const response = await this.userService.setUserRoleAsync(
      data.id,
      data.inputModel
    );
    if (response) {
      await this.fetchUsersAsync();
    }

    return response;
  }

  @Action
  public async deleteUserAsync(id: string) {
    const response = await this.userService.deleteUserAsync(id);
    if (response) {
      await this.fetchUsersAsync();
    }

    return response;
  }

  @Action
  public ClearUsers() {
    this.SET_USERS([]);
  }

  @Action
  public showUserDetailDialog() {
    this.SET_USER_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeUserDetailDialog() {
    this.SET_USER_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(User);
