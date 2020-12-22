using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class ReportsLogEvents
    {
        public static readonly int BaseId = 6;

        public static readonly EventId GetReportByID
            = CreateEventId(LogEventType.Success, 0, nameof(GetReportByID));
        public static readonly EventId GenerateOperationLog
            = CreateEventId(LogEventType.Success, 1, nameof(GenerateOperationLog));


        public static readonly EventId ReportNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(ReportNotFound));
        public static readonly EventId InvalidDate
            = CreateEventId(LogEventType.ClientError, 1, nameof(InvalidDate));


        public static readonly EventId GetReportByIDError
            = CreateEventId(LogEventType.ServerError, 0, nameof(GetReportByIDError));
        public static readonly EventId GenerateOperationLogError
            = CreateEventId(LogEventType.ServerError, 1, nameof(GenerateOperationLogError));


        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
