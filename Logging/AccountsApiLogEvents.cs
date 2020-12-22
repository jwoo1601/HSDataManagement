using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class AccountsApiLogEvents
    {
        public static readonly int BaseId = 0;

        public static readonly EventId GetAllSecurityCodeList
            = CreateEventId(LogEventType.Success, 0, nameof(GetAllSecurityCodeList));
        public static readonly EventId GenerateSecurityCode
            = CreateEventId(LogEventType.Success, 1, nameof(GenerateSecurityCode));
        public static readonly EventId InvalidateSecurityCode
            = CreateEventId(LogEventType.Success, 2, nameof(InvalidateSecurityCode));
        public static readonly EventId Register
            = CreateEventId(LogEventType.Success, 3, nameof(Register));

        public static readonly EventId SendEmailAddressVerificationRequest
            = CreateEventId(LogEventType.Action, 0, nameof(SendEmailAddressVerificationRequest));

        public static readonly EventId SecurityCodeNotRegistered
            = CreateEventId(LogEventType.ClientError, 0, nameof(SecurityCodeNotRegistered));
        public static readonly EventId InvalidSecurityCode
            = CreateEventId(LogEventType.ClientError, 1, nameof(InvalidSecurityCode));


        public static readonly EventId GetAllSecurityCodeListError
            = CreateEventId(LogEventType.ServerError, 0, nameof(GetAllSecurityCodeListError));
        public static readonly EventId GenerateSecurityCodeError
            = CreateEventId(LogEventType.ServerError, 1, nameof(GenerateSecurityCodeError));
        public static readonly EventId InvalidateSecurityCodeError
            = CreateEventId(LogEventType.ServerError, 2, nameof(InvalidateSecurityCodeError));
        public static readonly EventId RegisterError
            = CreateEventId(LogEventType.ServerError, 3, nameof(RegisterError));


        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
