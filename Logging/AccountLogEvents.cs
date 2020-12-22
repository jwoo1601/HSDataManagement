using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class AccountLogEvents
    {
        public static readonly int BaseId = 7;

        public static readonly EventId VerifyEmailAddress
            = CreateEventId(LogEventType.Success, 0, nameof(VerifyEmailAddress));
        public static readonly EventId ResendEmailAddressVerificationRequest
            = CreateEventId(LogEventType.Success, 1, nameof(ResendEmailAddressVerificationRequest));
        public static readonly EventId Register
            = CreateEventId(LogEventType.Success, 2, nameof(Register));

        public static readonly EventId SendEmailAddressVerificationRequest
            = CreateEventId(LogEventType.Action, 0, nameof(SendEmailAddressVerificationRequest));
        public static readonly EventId LoginFailure
            = CreateEventId(LogEventType.Action, 1, nameof(LoginFailure));

        public static readonly EventId SecurityCodeNotRegistered
            = CreateEventId(LogEventType.ClientError, 0, nameof(SecurityCodeNotRegistered));
        public static readonly EventId InvalidSecurityCode
            = CreateEventId(LogEventType.ClientError, 1, nameof(InvalidSecurityCode));

        public static readonly EventId VerifyEmailAddressError
            = CreateEventId(LogEventType.ServerError, 0, nameof(VerifyEmailAddressError));
        public static readonly EventId ResendEmailAddressVerificationRequestError
            = CreateEventId(LogEventType.ServerError, 1, nameof(ResendEmailAddressVerificationRequestError));
        public static readonly EventId RegisterError
            = CreateEventId(LogEventType.ServerError, 2, nameof(RegisterError));

        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
