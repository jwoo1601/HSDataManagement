using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class CustomersApiLogEvents
    {
        public static readonly int BaseId = 1;

        public static readonly EventId GetAllCustomers
            = CreateEventId(LogEventType.Success, 0, nameof(GetAllCustomers));
        public static readonly EventId GetCustomerByID
            = CreateEventId(LogEventType.Success, 1, nameof(GetCustomerByID));
        public static readonly EventId AddCustomer
            = CreateEventId(LogEventType.Success, 2, nameof(AddCustomer));
        public static readonly EventId EditCustomer
            = CreateEventId(LogEventType.Success, 3, nameof(EditCustomer));
        public static readonly EventId SetOptions
            = CreateEventId(LogEventType.Success, 4, nameof(SetOptions));
        public static readonly EventId AssignServices
            = CreateEventId(LogEventType.Success, 5, nameof(AssignServices));
        public static readonly EventId DeleteCustomer
            = CreateEventId(LogEventType.Success, 6, nameof(DeleteCustomer));


        public static readonly EventId CustomerNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(CustomerNotFound));


        public static readonly EventId GetAllCustomersError
            = CreateEventId(LogEventType.ServerError, 0, nameof(GetAllCustomersError));
        public static readonly EventId GetCustomerByIDError
            = CreateEventId(LogEventType.ServerError, 1, nameof(GetCustomerByIDError));
        public static readonly EventId AddCustomerError
            = CreateEventId(LogEventType.ServerError, 2, nameof(AddCustomerError));
        public static readonly EventId EditCustomerError
            = CreateEventId(LogEventType.ServerError, 3, nameof(EditCustomerError));
        public static readonly EventId SetOptionsError
            = CreateEventId(LogEventType.ServerError, 4, nameof(SetOptionsError));
        public static readonly EventId AssignServicesError
            = CreateEventId(LogEventType.ServerError, 5, nameof(AssignServicesError));
        public static readonly EventId DeleteCustomerError
            = CreateEventId(LogEventType.ServerError, 6, nameof(DeleteCustomerError));

        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
