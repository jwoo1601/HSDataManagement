using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models.Identity
{
    public static class PermissionTypes
    {
        public static readonly string Create = "create";
        public static readonly string Read = "read";
        public static readonly string Update = "update";
        public static readonly string Delete = "delete";
        public static readonly string Manage = "manage";
    }

    public class PermissionGroup
    {
        private ICollection<string> permissionTypes;
        public string GroupName { get; }
        public IEnumerable<string> Permissions
            => permissionTypes.AsEnumerable();
        public IEnumerable<string> DecoratedNames
            => permissionTypes.Select(t => $"{GroupName}.{t}");

        public PermissionGroup(string groupName)
        {
            permissionTypes = new HashSet<string>();
            GroupName = groupName;
        }

        public PermissionGroup AddPermissionType(string permissionType)
        {
            permissionTypes.Add(permissionType);

            return this;
        }

        public PermissionGroup AddCRUDTypes()
        {
            AddPermissionType(PermissionTypes.Create)
            .AddPermissionType(PermissionTypes.Read)
            .AddPermissionType(PermissionTypes.Update)
            .AddPermissionType(PermissionTypes.Delete);

            return this;
        }
    }
}
