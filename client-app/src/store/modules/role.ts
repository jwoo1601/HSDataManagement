import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import { $inject } from "@vanroeybe/vue-inversify-plugin";
import HSMRole from "@/models/api/Role";
import IRoleService from "@/services/roleService";

export interface RoleState {
  roles: HSMRole[];
  editedRole: HSMRole | null;
  roleDetailDialog: boolean;
}

@Module({ dynamic: true, store, name: "role" })
class Role extends VuexModule implements RoleState {
  @$inject()
  private roleService!: IRoleService;

  public roles: HSMRole[] = [];
  public editedRole: HSMRole | null = null;
  public roleDetailDialog = false;

  @Mutation
  private SET_ROLES(roles: HSMRole[]) {
    this.roles = roles;
  }

  @Mutation
  private SET_ROLE_DETAIL_DIALOG_VISIBLE(visible: boolean) {
    this.roleDetailDialog = visible;
  }

  @Action
  public async fetchRolesAsync() {
    this.SET_ROLES(await this.roleService.getRolesAsync());
  }

  @Action
  public async getRoleDataAsync(id: string) {
    return await this.roleService.getRoleByIdAsync(id);
  }

  @Action
  public ClearRoles() {
    this.SET_ROLES([]);
  }

  @Action
  public showRoleDetailDialog() {
    this.SET_ROLE_DETAIL_DIALOG_VISIBLE(true);
  }

  @Action
  public closeRoleDetailDialog() {
    this.SET_ROLE_DETAIL_DIALOG_VISIBLE(false);
  }
}

export default getModule(Role);
