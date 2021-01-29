using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class UsersApiLogEvents
    {
        public static readonly int BaseId = 8;

        public static readonly EventId GetAllUsers
            = CreateEventId(LogEventType.Success, 0, nameof(GetAllUsers));
        public static readonly EventId GetUserByID
            = CreateEventId(LogEventType.Success, 1, nameof(GetUserByID));
        public static readonly EventId InactivateUser
            = CreateEventId(LogEventType.Success, 2, nameof(InactivateUser));
        public static readonly EventId EndLockout
            = CreateEventId(LogEventType.Success, 3, nameof(EndLockout));
        public static readonly EventId MarkEmailConfirmed
            = CreateEventId(LogEventType.Success, 4, nameof(MarkEmailConfirmed));
        public static readonly EventId DeleteUser
            = CreateEventId(LogEventType.Success, 5, nameof(DeleteUser));
        public static readonly EventId ActivateUser
            = CreateEventId(LogEventType.Success, 6, nameof(ActivateUser));
        public static readonly EventId SetUserRole
            = CreateEventId(LogEventType.Success, 7, nameof(SetUserRole));


        public static readonly EventId UserNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(UserNotFound));
        public static readonly EventId UserNotOnLockout
            = CreateEventId(LogEventType.ClientError, 1, nameof(UserNotOnLockout));
        public static readonly EventId EmailAlreadyConfirmed
            = CreateEventId(LogEventType.ClientError, 2, nameof(EmailAlreadyConfirmed));
        public static readonly EventId RoleNotFound
            = CreateEventId(LogEventType.ClientError, 3, nameof(RoleNotFound));


        public static readonly EventId GetAllUsersError
            = CreateEventId(LogEventType.ServerError, 0, nameof(GetAllUsersError));
        public static readonly EventId GetUserByIDError
            = CreateEventId(LogEventType.ServerError, 1, nameof(GetUserByIDError));
        public static readonly EventId InactivateUserError
            = CreateEventId(LogEventType.ServerError, 2, nameof(InactivateUserError));
        public static readonly EventId EndLockoutError
            = CreateEventId(LogEventType.ServerError, 3, nameof(EndLockoutError));
        public static readonly EventId MarkEmailConfirmedError
            = CreateEventId(LogEventType.ServerError, 4, nameof(MarkEmailConfirmedError));
        public static readonly EventId DeleteUserError
            = CreateEventId(LogEventType.ServerError, 5, nameof(DeleteUserError));
        public static readonly EventId ActivateUserError
            = CreateEventId(LogEventType.ServerError, 6, nameof(ActivateUserError));
        public static readonly EventId SetUserRoleError
            = CreateEventId(LogEventType.ServerError, 7, nameof(SetUserRoleError));

        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
