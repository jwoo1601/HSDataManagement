using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models.Identity
{
    public static class HSMPermissionSections
    {
        public static readonly string Role = "role";
        public static readonly string SecurityCode = "security-code";
        public static readonly string User = "user";
        public static readonly string Customer = "customer";
        public static readonly string Service = "service";
        public static readonly string Food = "food";
        public static readonly string Meal = "meal";
        public static readonly string WeeklyMenu = "weekly-menu";
        public static readonly string Hangfire = "hangfire";
    }

    public static class HSMPermissionGroups
    {
        public static PermissionGroup NewPermissionGroup(string sectionName, string permissionType)
        {
            return new PermissionGroup(sectionName).AddPermissionType(permissionType);
        }

        public static PermissionGroup NewCRUDPermissionGroup(string sectionName)
        {
            return new PermissionGroup(sectionName).AddCRUDTypes();
        }

        public static IEnumerable<PermissionGroup> DefaultUserPermissions
            => new[]
            {
                NewPermissionGroup(HSMPermissionSections.Customer, PermissionTypes.Read),
                NewPermissionGroup(HSMPermissionSections.Service, PermissionTypes.Read),
                NewPermissionGroup(HSMPermissionSections.Food, PermissionTypes.Read),
                NewPermissionGroup(HSMPermissionSections.Meal, PermissionTypes.Read),
                NewPermissionGroup(HSMPermissionSections.WeeklyMenu, PermissionTypes.Read)
            };

        public static IEnumerable<PermissionGroup> DefaultAdminPermissions
            => new[]
            {
                NewPermissionGroup(HSMPermissionSections.Role, PermissionTypes.Read),
                NewCRUDPermissionGroup(HSMPermissionSections.Customer),
                NewCRUDPermissionGroup(HSMPermissionSections.Service),
                NewCRUDPermissionGroup(HSMPermissionSections.Food),
                NewCRUDPermissionGroup(HSMPermissionSections.Meal),
                NewCRUDPermissionGroup(HSMPermissionSections.WeeklyMenu)
            };

        public static IEnumerable<PermissionGroup> DefaultMasterPermissions
            => new[]
            {
                NewCRUDPermissionGroup(HSMPermissionSections.Role),
                NewCRUDPermissionGroup(HSMPermissionSections.SecurityCode),
                NewCRUDPermissionGroup(HSMPermissionSections.User),
                NewCRUDPermissionGroup(HSMPermissionSections.Customer),
                NewCRUDPermissionGroup(HSMPermissionSections.Service),
                NewCRUDPermissionGroup(HSMPermissionSections.Food),
                NewCRUDPermissionGroup(HSMPermissionSections.Meal),
                NewCRUDPermissionGroup(HSMPermissionSections.WeeklyMenu),
                NewPermissionGroup(HSMPermissionSections.Hangfire, PermissionTypes.Manage)
            };
    }
}
