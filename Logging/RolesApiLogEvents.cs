using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class RolesApiLogEvents
    {
        public static readonly int BaseId = 9;

        public static readonly EventId GetAllRoles
            = CreateEventId(LogEventType.Success, 0, nameof(GetAllRoles));
        public static readonly EventId GetRoleByID
            = CreateEventId(LogEventType.Success, 1, nameof(GetRoleByID));
        public static readonly EventId DeleteRole
            = CreateEventId(LogEventType.Success, 4, nameof(DeleteRole));


        public static readonly EventId RoleNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(RoleNotFound));


        public static readonly EventId GetAllRolesError
            = CreateEventId(LogEventType.ServerError, 0, nameof(GetAllRolesError));
        public static readonly EventId GetRoleByIDError
            = CreateEventId(LogEventType.ServerError, 1, nameof(GetRoleByIDError));
        public static readonly EventId DeleteRoleError
            = CreateEventId(LogEventType.ServerError, 4, nameof(DeleteRoleError));

        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
