using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class MenuApiLogEvents
    {
        public static readonly int BaseId = 5;

        public static readonly EventId GetMenusInPeriod
            = CreateEventId(LogEventType.Success, 0, nameof(GetMenusInPeriod));
        public static readonly EventId GetLogsInPeriod
            = CreateEventId(LogEventType.Success, 1, nameof(GetLogsInPeriod));

        public static readonly EventId GetMenuByDate
            = CreateEventId(LogEventType.Success, 2, nameof(GetMenuByDate));

        public static readonly EventId AddMenu
            = CreateEventId(LogEventType.Success, 4, nameof(AddMenu));
        public static readonly EventId AddOperationLog
            = CreateEventId(LogEventType.Success, 5, nameof(AddOperationLog));
        public static readonly EventId AddPreservationLog
            = CreateEventId(LogEventType.Success, 6, nameof(AddPreservationLog));
        public static readonly EventId AddInspectionLog
            = CreateEventId(LogEventType.Success, 7, nameof(AddInspectionLog));

        public static readonly EventId EditMenu
            = CreateEventId(LogEventType.Success, 8, nameof(EditMenu));
        public static readonly EventId EditOperationLog
            = CreateEventId(LogEventType.Success, 9, nameof(EditOperationLog));
        public static readonly EventId EditPreservationLog
            = CreateEventId(LogEventType.Success, 10, nameof(EditPreservationLog));
        public static readonly EventId EditInspectionLog
            = CreateEventId(LogEventType.Success, 11, nameof(EditInspectionLog));

        public static readonly EventId DeleteMenu
            = CreateEventId(LogEventType.Success, 12, nameof(DeleteMenu));
        public static readonly EventId DeleteOperationLog
            = CreateEventId(LogEventType.Success, 13, nameof(DeleteOperationLog));
        public static readonly EventId DeletePreservationLog
            = CreateEventId(LogEventType.Success, 14, nameof(DeletePreservationLog));
        public static readonly EventId DeleteInspectionLog
            = CreateEventId(LogEventType.Success, 15, nameof(DeleteInspectionLog));

        public static readonly EventId GetOperationLogs
            = CreateEventId(LogEventType.Success, 16, nameof(GetOperationLogs));
        public static readonly EventId GetPreservationLogs
            = CreateEventId(LogEventType.Success, 17, nameof(GetPreservationLogs));
        public static readonly EventId GetInspectionLogs
            = CreateEventId(LogEventType.Success, 18, nameof(GetInspectionLogs));


        public static readonly EventId MenuNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(MenuNotFound));
        public static readonly EventId OperationLogNotFound
            = CreateEventId(LogEventType.ClientError, 1, nameof(OperationLogNotFound));
        public static readonly EventId PreservationLogNotFound
            = CreateEventId(LogEventType.ClientError, 2, nameof(PreservationLogNotFound));
        public static readonly EventId InspectionLogNotFound
            = CreateEventId(LogEventType.ClientError, 3, nameof(InspectionLogNotFound));
        public static readonly EventId InvalidStartDate
            = CreateEventId(LogEventType.ClientError, 4, nameof(InvalidStartDate));
        public static readonly EventId InvalidEndDate
            = CreateEventId(LogEventType.ClientError, 5, nameof(InvalidEndDate));
        public static readonly EventId InvalidLogType
            = CreateEventId(LogEventType.ClientError, 6, nameof(InvalidLogType));
        public static readonly EventId InvalidDate
            = CreateEventId(LogEventType.ClientError, 7, nameof(InvalidDate));
        public static readonly EventId InvalidMealType
            = CreateEventId(LogEventType.ClientError, 8, nameof(InvalidMealType));
        public static readonly EventId InvalidMealCategory
            = CreateEventId(LogEventType.ClientError, 9, nameof(InvalidMealCategory));
        public static readonly EventId DuplicateMenu
            = CreateEventId(LogEventType.ClientError, 10, nameof(DuplicateMenu));
        public static readonly EventId DuplicateOperationLog
            = CreateEventId(LogEventType.ClientError, 11, nameof(DuplicateOperationLog));
        public static readonly EventId DuplicatePreservationLog
            = CreateEventId(LogEventType.ClientError, 12, nameof(DuplicatePreservationLog));
        public static readonly EventId MealPackageNotFound
            = CreateEventId(LogEventType.ClientError, 13, nameof(MealPackageNotFound));
        public static readonly EventId EmployeeRoleNotFound
            = CreateEventId(LogEventType.ClientError, 14, nameof(EmployeeRoleNotFound));


        public static readonly EventId AddMenuError
            = CreateEventId(LogEventType.ServerError, 0, nameof(AddMenuError));
        public static readonly EventId AddOperationLogError
            = CreateEventId(LogEventType.ServerError, 1, nameof(AddOperationLogError));
        public static readonly EventId AddPreservationLogError
            = CreateEventId(LogEventType.ServerError, 2, nameof(AddPreservationLogError));
        public static readonly EventId AddInspectionLogError
            = CreateEventId(LogEventType.ServerError, 3, nameof(AddInspectionLogError));

        public static readonly EventId EditMenuError
            = CreateEventId(LogEventType.ServerError, 4, nameof(EditMenuError));
        public static readonly EventId EditOperationLogError
            = CreateEventId(LogEventType.ServerError, 5, nameof(EditOperationLogError));
        public static readonly EventId EditPreservationLogError
            = CreateEventId(LogEventType.ServerError, 6, nameof(EditPreservationLogError));
        public static readonly EventId EditInspectionLogError
            = CreateEventId(LogEventType.ServerError, 7, nameof(EditInspectionLogError));

        public static readonly EventId DeleteMenuError
            = CreateEventId(LogEventType.ServerError, 8, nameof(DeleteMenuError));
        public static readonly EventId DeleteOperationLogError
            = CreateEventId(LogEventType.ServerError, 9, nameof(DeleteOperationLogError));
        public static readonly EventId DeletePreservationLogError
            = CreateEventId(LogEventType.ServerError, 10, nameof(DeletePreservationLogError));
        public static readonly EventId DeleteInspectionLogError
            = CreateEventId(LogEventType.ServerError, 11, nameof(DeleteInspectionLogError));


        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
