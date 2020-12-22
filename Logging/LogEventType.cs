using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public enum LogEventType : int
    {
        Success = 1,
        Warning = 2,
        ClientError = 4,
        ServerError = 5,
        Fatal = 6,
        Action = 7,

        Debug = 9
    }

    /*
     * EventId Format: xyyyzzz
     * x: LogEventType (0-9)
     * y: BaseNumber (0-999)
     * z: Event Identifier (unique only within the same event category) (0-999)
     */
    public class LogEvents
    {
        /**
         * <param name="baseNumber">id used to uniquely identify the event category</param>
         * <param name="id">id used to uniquely identify the event within the same category</param>
         */
        public static EventId CreateEventId(
            LogEventType eventType,
            int baseNumber,
            int id,
            string name
        )
        {
            return new EventId(
                (int)eventType * 1000000 + baseNumber * 1000 + id,
                name
            );
        }
    }
}
