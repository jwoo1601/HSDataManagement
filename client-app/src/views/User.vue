<template>
  <div>
    <hsm-data-table
      ref="dataTable"
      header-icon="people-fill"
      :header="$t('menu.user')"
      :field-definitions="fieldDefinitions"
      :fetch-entries-action="fetchEntriesAsync"
      :entries="users"
      :entries-mapper="mapTableItems"
      :delete-entry-action="deleteEntryAsync"
      :labels="labels"
      :allow-add="false"
      :allow-edit="false"
      :criteria-options="criteriaOptions"
      :row-class="rowClass"
      select-mode="single"
    >
      <template #cell(email)="{ value }">
        <b-link
          :to="`mailto:${value}`"
          class="pt-1 font-weight-bold text-center"
        >
          {{ value }}
        </b-link>
      </template>

      <template #cell(activated)="data">
        <div class="d-flex justify-content-center text-center pb-3">
          <b-form-checkbox
            class="mt-3"
            name="activated"
            v-model="data.value"
            @change="handleToggleActivated(data.item)"
            switch
          ></b-form-checkbox>
        </div>
      </template>

      <template #cell(emailConfirmed)="{ value }">
        <div class="pt-1 font-weight-bold text-center">
          <b-icon
            :icon="value ? 'check2-square' : 'x-square'"
            :variant="value ? 'success' : 'danger'"
          ></b-icon>
        </div>
      </template>

      <template #cell(lockoutEnd)="{ value, index, unformatted, item }">
        <div class="d-flex justify-content-center text-center pt-1">
          <hsm-button
            :id="`user-lockout-${index}`"
            class="mr-3"
            size="sm"
            :icon="unformatted ? 'lock-fill' : 'unlock-fill'"
            text=""
            spacing="0"
            :variant="`outline-${unformatted ? 'danger' : 'primary'}`"
            :textVariant="unformatted ? 'danger' : 'primary'"
            :hoverVariant="unformatted ? 'danger' : 'primary'"
            hoverTextVariant="light"
            fontWeight="bold"
            @click="handleEndLockout(item)"
          ></hsm-button>
          <b-tooltip
            v-if="unformatted"
            :target="`user-lockout-${index}`"
            placement="top"
            :variant="unformatted ? 'danger' : 'primary'"
          >
            {{ value }}
          </b-tooltip>
        </div>
      </template>

      <template #selectionActions>
        <hsm-button
          v-show="!checkSelectedItemEmailConfirmed()"
          class="mr-2"
          size="sm"
          icon="shield-check"
          variant="success"
          textVariant="light"
          fontWeight="bold"
          spacing="1"
          @click="handleSelectedItemMarkEmailConfirmed()"
        >
          {{ $t("action.markEmailConfirmed") }}
        </hsm-button>
        <hsm-button
          v-show="dataTable.selectedItems.length > 0"
          class="mr-2"
          size="sm"
          icon="flag-fill"
          variant="main"
          textVariant="light"
          fontWeight="bold"
          spacing="1"
          @click="handleSelectedItemSetRole()"
        >
          {{ $t("action.setRole") }}
        </hsm-button>
      </template>
    </hsm-data-table>
    <hsm-dialog
      :visible.sync="changeRoleDialogVisible"
      icon="flag-fill"
      :title="$t('title.changeUserRole')"
      title-variant="warning"
      :show-footer="true"
      @confirm="onChangeRoleConfirmed()"
    >
      <b-form name="changeRole">
        <b-form-group
          id="group-roles"
          :label="$t('label.role')"
          label-for="input-role"
        >
          <b-form-select
            :options="roleOptions"
            v-model="selectedRole"
          ></b-form-select>
        </b-form-group>
      </b-form>
    </hsm-dialog>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import HSMUser from "@/models/api/User";
import AppModule from "@/store/modules/app";
import UserModule from "@/store/modules/user";
import RoleModule from "@/store/modules/role";
import {
  BFormSelectOption,
  BTable,
  BvTableCtxObject,
  BvTableFieldArray,
  BvTableFormatterCallback,
} from "bootstrap-vue";
import App from "@/App.vue";
import HSMDataTable, {
  HSMDataTableFieldDefinition,
  HSMDataTableFilterCriteria,
  HSMDataTableItem,
  HSMDataTableLabels,
} from "@/components/HSMDataTable.vue";
import HSMGenerateSecurityCode from "@/components/HSMGenerateSecurityCode.vue";
import { Ref } from "vue-property-decorator";
import HSMRole from "@/models/api/Role";

interface UserTableItem extends HSMDataTableItem {
  username: string;
  email: string;
  role: string;
  activated: boolean;
  emailConfirmed: boolean;
  lockoutEnd: Date | null;
  registeredAt: Date;
}

@Component({
  components: {
    "hsm-generate-security-code": HSMGenerateSecurityCode,
  },
})
export default class User extends Vue {
  readonly fieldDefinitions: HSMDataTableFieldDefinition[] = [
    { key: "username", label: "field.username", sortable: true },
    {
      key: "email",
      label: "field.email",
      sortable: true,
      hasCustomRenderer: true,
    },
    {
      key: "role",
      label: "field.role",
      sortable: true,
    },
    {
      key: "activated",
      label: "field.activated",
      sortable: true,
      searchable: false,
      hasCustomRenderer: true,
    },
    {
      key: "emailConfirmed",
      label: "field.emailConfirmed",
      sortable: true,
      searchable: false,
      hasCustomRenderer: true,
    },
    {
      key: "lockoutEnd",
      label: "field.lockout",
      sortable: true,
      hasCustomRenderer: true,
      formatter: this.formatDate,
    },
    {
      key: "registeredAt",
      label: "field.registeredAt",
      sortable: true,
      formatter: this.formatDate,
    },
  ];
  readonly labels: HSMDataTableLabels = {
    item: "label.user",
  };
  readonly criteriaOptions: HSMDataTableFilterCriteria[] = [
    { value: "id", text: "field.id" },
    { value: "name", text: "field.name" },
    { value: "username", text: "field.username" },
    { value: "email", text: "field.email" },
    { value: "role", text: "field.role" },
    { value: "lockoutEnd", text: "field.lockout" },
    { value: "registeredAt", text: "field.registeredAt" },
  ];

  changeRoleDialogVisible = false;
  selectedRole: HSMRole | null = null;

  @Ref()
  dataTable!: HSMDataTable;

  created() {
    AppModule.showLoading(this.$t("loading.data").toString());
  }

  mounted() {
    AppModule.hideLoading();
  }

  get users() {
    return UserModule.users;
  }

  get roles() {
    return RoleModule.roles;
  }

  get roleOptions() {
    return this.roles.map((r) => ({
      value: r,
      text: r.name,
    }));
  }

  async fetchEntriesAsync() {
    return UserModule.fetchUsersAsync();
  }

  async deleteEntryAsync(entry: HSMUser) {
    return UserModule.deleteUserAsync(entry.id);
  }

  async handleToggleActivated(item: UserTableItem) {
    AppModule.showLoading(this.$t("loading.processing").toString());

    if (item.activated) {
      await UserModule.inactivateUserAsync(item.id);
    } else {
      await UserModule.activateUserAsync(item.id);
    }

    this.dataTable.notifyViewUpdates();

    AppModule.hideLoading();
  }

  checkSelectedItemEmailConfirmed() {
    if (this.dataTable.selectedItems.length === 0) {
      return true;
    }

    return (this.dataTable.selectedItems[0] as UserTableItem).emailConfirmed;
  }

  async handleSelectedItemMarkEmailConfirmed() {
    if (this.dataTable.selectedItems.length === 0) {
      return;
    }

    const item = this.dataTable.selectedItems[0] as UserTableItem;
    if (!item.emailConfirmed) {
      AppModule.showLoading(this.$t("loading.processing").toString());

      await UserModule.markEmailConfirmedAsync(item.id);

      this.dataTable.notifyViewUpdates();

      AppModule.hideLoading();
    }
  }

  async handleEndLockout(item: UserTableItem) {
    if (item.lockoutEnd && item.lockoutEnd.getTime() > Date.now()) {
      AppModule.showLoading(this.$t("loading.processing").toString());

      await UserModule.endLockoutAsync(item.id);

      this.dataTable.notifyViewUpdates();

      AppModule.hideLoading();
    }
  }

  async handleSelectedItemSetRole() {
    if (this.dataTable.selectedItems.length > 0) {
      const item = this.dataTable.selectedItems[0] as UserTableItem;

      AppModule.showLoading(this.$t("loading.general").toString());

      await RoleModule.fetchRolesAsync();
      this.selectedRole = this.roles.find((r) => r.name === item.role) ?? null;

      AppModule.hideLoading();

      this.changeRoleDialogVisible = true;
    }
  }

  async onChangeRoleConfirmed() {
    if (this.selectedRole && this.dataTable.selectedItems.length > 0) {
      const item = this.dataTable.selectedItems[0] as UserTableItem;

      AppModule.showLoading(this.$t("loading.processing").toString());

      const response = await UserModule.setUserRoleAsync({
        id: item.id,
        inputModel: {
          role: this.selectedRole.id,
        },
      });
      if (response.isSuccess) {
        this.dataTable.notifyViewUpdates();
        this.changeRoleDialogVisible = false;
      } else {
        this.$hsmMessageDialog(
          this.$t("title.error").toString(),
          response.errorMessage ?? this.$t("error.changeRole").toString()
        );
      }

      AppModule.hideLoading();
    }
  }

  mapTableItems(user: HSMUser): UserTableItem {
    return {
      id: user.id,
      name: user.name,
      username: user.username,
      email: user.email,
      role: user.roles[0]?.name,
      activated: user.is_active,
      emailConfirmed: user.email_confirmed,
      lockoutEnd: user.lockout_end,
      registeredAt: user.registered_at,
      action: {
        edit: false,
        delete: true,
      },
      raw: user,
    };
  }

  formatDate(date: Date | null, key: string, item: any) {
    if (date) {
      return new Date(date).toLocaleDateString(this.$i18n.locale);
    }

    return "---";
  }

  rowClass(item: UserTableItem) {
    if (!item.activated) {
      return "table-secondary";
    }

    if (item.lockoutEnd !== null && item.lockoutEnd.getTime() > Date.now()) {
      return "table-warning";
    }

    return;
  }
}
</script>

<style lang="scss" scoped></style>
>
