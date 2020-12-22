using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class ServicesApiLogEvents
    {
        public static readonly int BaseId = 3;

        public static readonly EventId GetAllServices
            = CreateEventId(LogEventType.Success, 0, nameof(GetAllServices));
        public static readonly EventId GetAllServiceGroups
            = CreateEventId(LogEventType.Success, 1, nameof(GetAllServiceGroups));

        public static readonly EventId GetServiceByID
            = CreateEventId(LogEventType.Success, 2, nameof(GetServiceByID));
        public static readonly EventId GetServiceGroupByID
            = CreateEventId(LogEventType.Success, 3, nameof(GetServiceGroupByID));

        public static readonly EventId AddService
            = CreateEventId(LogEventType.Success, 4, nameof(AddService));
        public static readonly EventId AddServiceGroup
            = CreateEventId(LogEventType.Success, 5, nameof(AddServiceGroup));

        public static readonly EventId EditService
            = CreateEventId(LogEventType.Success, 6, nameof(EditService));
        public static readonly EventId EditServiceGroup
            = CreateEventId(LogEventType.Success, 7, nameof(EditServiceGroup));

        public static readonly EventId DeleteService
            = CreateEventId(LogEventType.Success, 8, nameof(DeleteService));
        public static readonly EventId DeleteServiceGroup
            = CreateEventId(LogEventType.Success, 9, nameof(DeleteServiceGroup));


        public static readonly EventId ServiceNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(ServiceNotFound));
        public static readonly EventId ServiceGroupNotFound
            = CreateEventId(LogEventType.ClientError, 1, nameof(ServiceGroupNotFound));


        public static readonly EventId AddServiceError
            = CreateEventId(LogEventType.ServerError, 0, nameof(AddServiceError));
        public static readonly EventId AddServiceGroupError
            = CreateEventId(LogEventType.ServerError, 1, nameof(AddServiceGroupError));

        public static readonly EventId EditServiceError
            = CreateEventId(LogEventType.ServerError, 2, nameof(EditServiceError));
        public static readonly EventId EditServiceGroupError
            = CreateEventId(LogEventType.ServerError, 3, nameof(EditServiceGroupError));

        public static readonly EventId DeleteServiceError
            = CreateEventId(LogEventType.ServerError, 4, nameof(DeleteServiceError));
        public static readonly EventId DeleteServiceGroupError
            = CreateEventId(LogEventType.ServerError, 5, nameof(DeleteServiceGroupError));


        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
