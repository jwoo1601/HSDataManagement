using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class MealsApiLogEvents
    {
        public static readonly int BaseId = 4;

        public static readonly EventId GetAllMeals
            = CreateEventId(LogEventType.Success, 0, nameof(GetAllMeals));
        public static readonly EventId GetAllMealPackages
            = CreateEventId(LogEventType.Success, 1, nameof(GetAllMealPackages));

        public static readonly EventId GetMealByID
            = CreateEventId(LogEventType.Success, 2, nameof(GetMealByID));
        public static readonly EventId GetMealPackageByID
            = CreateEventId(LogEventType.Success, 3, nameof(GetMealPackageByID));

        public static readonly EventId AddMeal
            = CreateEventId(LogEventType.Success, 4, nameof(AddMeal));
        public static readonly EventId AddMealPackage
            = CreateEventId(LogEventType.Success, 5, nameof(AddMealPackage));

        public static readonly EventId EditMeal
            = CreateEventId(LogEventType.Success, 6, nameof(EditMeal));
        public static readonly EventId EditMealPackage
            = CreateEventId(LogEventType.Success, 7, nameof(EditMealPackage));

        public static readonly EventId DeleteMeal
            = CreateEventId(LogEventType.Success, 8, nameof(DeleteMeal));
        public static readonly EventId DeleteMealPackage
            = CreateEventId(LogEventType.Success, 9, nameof(DeleteMealPackage));


        public static readonly EventId MealNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(MealNotFound));
        public static readonly EventId MealPackageNotFound
            = CreateEventId(LogEventType.ClientError, 1, nameof(MealPackageNotFound));


        public static readonly EventId AddMealError
            = CreateEventId(LogEventType.ServerError, 0, nameof(AddMealError));
        public static readonly EventId AddMealPackageError
            = CreateEventId(LogEventType.ServerError, 1, nameof(AddMealPackageError));

        public static readonly EventId EditMealError
            = CreateEventId(LogEventType.ServerError, 2, nameof(EditMealError));
        public static readonly EventId EditMealPackageError
            = CreateEventId(LogEventType.ServerError, 3, nameof(EditMealPackageError));

        public static readonly EventId DeleteMealError
            = CreateEventId(LogEventType.ServerError, 4, nameof(DeleteMealError));
        public static readonly EventId DeleteMealPackageError
            = CreateEventId(LogEventType.ServerError, 5, nameof(DeleteMealPackageError));


        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
