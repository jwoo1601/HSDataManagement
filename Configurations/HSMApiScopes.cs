using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HyosungManagement.Configurations
{
    public enum HSMApiScopeType
    {
        Oidc,
        Identity,
        Main
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HSMApiScopeAttribute : Attribute
    {
        public HSMApiScopeType ScopeType { get; set; }

        public HSMApiScopeAttribute(HSMApiScopeType scopeType)
        {
            ScopeType = scopeType;
        }
    }

    public static class HSMApiScopes
    {
        /**
         * <summary>Lists all the existing api scopes in HSM.</summary>
         */
        public static IEnumerable<string> ListAllScopes()
        {
            var allScopeTypes = Enum.GetValues(typeof(HSMApiScopeType)).Cast<HSMApiScopeType>();

            return allScopeTypes.SelectMany(
                st => ListScopes(st)
            );
        }

        public static IEnumerable<string> ListScopes(HSMApiScopeType scopeType)
        {
            var innerClasses = typeof(HSMApiScopes)
                                .GetTypeInfo()
                                .DeclaredNestedTypes
                                .Where(
                                    t =>
                                        t.IsNestedPublic &&
                                        t.GetCustomAttribute<HSMApiScopeAttribute>()?.ScopeType == scopeType
                                );

            return innerClasses.SelectMany(
                c => ListMemberScopes(c)
            );
        }

        public static IEnumerable<string> ListScopes<TScope>()
        {
            return ListMemberScopes(
                typeof(TScope).GetTypeInfo()
            );
        }

        static IEnumerable<string> ListMemberScopes(TypeInfo typeInfo)
        {
            return typeInfo
                    .DeclaredFields
                    .Where(
                        // we only want public static string fields
                        f => f.IsPublic && f.IsStatic && f.FieldType == typeof(string)
                    )
                    .Select(
                        f => f.GetValue(null)
                    )
                    .Where(v => v != null)
                    .Cast<string>()
                    .Distinct();
        }

        // non-public classes are not exposed

        [HSMApiScope(HSMApiScopeType.Oidc)]
        public class OpenId
        {
            public static readonly string Oidc = "openid";
            public static readonly string Profile = "profile";
            public static readonly string Email = "email";
            public static readonly string Role = "role";
            public static readonly string SecurityCode = "security_code";
        }

        [HSMApiScope(HSMApiScopeType.Identity)]
        public class UserRole
        {
            static readonly string baseName = "user_role";

            public static readonly string FullAccess = baseName;
            public static readonly string Add = $"add:{baseName}";
            public static readonly string View = $"view:{baseName}";
            public static readonly string Edit = $"edit:{baseName}";
            public static readonly string Delete = $"delete:{baseName}";
        }

        [HSMApiScope(HSMApiScopeType.Identity)]
        public class UserSecurityCode
        {
            static readonly string baseName = "user_security_code";

            public static readonly string FullAccess = baseName;
            public static readonly string Add = $"add:{baseName}";
            public static readonly string View = $"view:{baseName}";
            public static readonly string Invalidate = $"invalidate:{baseName}";
        }

        [HSMApiScope(HSMApiScopeType.Main)]
        public class User
        {
            static readonly string baseName = "user";

            public static readonly string FullAccess = baseName;
            public static readonly string Add = $"add:{baseName}";
            public static readonly string View = $"view:{baseName}";
            public static readonly string Edit = $"edit:{baseName}";
            public static readonly string Delete = $"delete:{baseName}";
        }

        [HSMApiScope(HSMApiScopeType.Main)]
        public class Customer // includes CustomerTag
        {
            static readonly string baseName = "customer";

            public static readonly string FullAccess = baseName;
            public static readonly string Add = $"add:{baseName}";
            public static readonly string View = $"view:{baseName}";
            public static readonly string Edit = $"edit:{baseName}";
            public static readonly string SetOptions = $"set_options:{baseName}";
            public static readonly string Delete = $"delete:{baseName}";
            public static readonly string AssignService = $"assign_service:{baseName}";
        }

        [HSMApiScope(HSMApiScopeType.Main)]
        public class Service // includes ServiceGroup
        {
            static readonly string baseName = "service";

            public static readonly string FullAccess = baseName;
            public static readonly string Add = $"add:{baseName}";
            public static readonly string View = $"view:{baseName}";
            public static readonly string Edit = $"edit:{baseName}";
            public static readonly string Delete = $"delete:{baseName}";
        }

        [HSMApiScope(HSMApiScopeType.Main)]
        public static class Food // includes FoodCategory, FoodIngredient, and FoodIngredientCategory
        {
            static readonly string baseName = "food";

            public static readonly string FullAccess = baseName;
            public static readonly string Add = $"add:{baseName}";
            public static readonly string View = $"view:{baseName}";
            public static readonly string Edit = $"edit:{baseName}";
            public static readonly string Delete = $"delete:{baseName}";
        }

        [HSMApiScope(HSMApiScopeType.Main)]
        public static class Meal // includes MealPackage
        {
            static readonly string baseName = "meal";

            public static readonly string FullAccess = baseName;
            public static readonly string Add = $"add:{baseName}";
            public static readonly string View = $"view:{baseName}";
            public static readonly string Edit = $"edit:{baseName}";
            public static readonly string Delete = $"delete:{baseName}";


        }

        [HSMApiScope(HSMApiScopeType.Main)]
        public static class WeeklyMenu // => DailyMenu, OperationLog
        {
            static readonly string baseName = "weekly_menu";

            public static readonly string FullAccess = baseName;
            public static readonly string Add = $"add:{baseName}";
            public static readonly string View = $"view:{baseName}";
            public static readonly string Edit = $"edit:{baseName}";
            public static readonly string Delete = $"delete:{baseName}";
        }
    }
}
